using backend.getdata;
using System.Net;
using Models.Post;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace Endpoints;

public static class PostEndpoint
{
	public static RouteGroupBuilder MapPostEndpoints(this RouteGroupBuilder group)
	{

		group.MapGet("/", [Authorize] async Task<string> () =>
		// group.MapGet("/", async Task<string> () =>
		{
			try
			{
				Post p = new Post();
				List<Posts>? allPost = await p.getAllPost();
				return JsonConvert.SerializeObject(allPost);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return "{}";
			}
		})
		.WithName("GetPosts");

		group.MapGet("/{organizationsId}", async Task<IResult> (int organizationsId) =>
		{
			try
			{
				Post p = new Post();
				List<Posts>? posts = await p.getPostByOrganization(organizationsId);
				string jsonPosts = JsonConvert.SerializeObject(posts);
				return Results.Ok(jsonPosts);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return Results.InternalServerError();
			}
		})
		.WithName("GetPostsFromOrganizationsId");

		group.MapPost("/", async Task<IResult> (HttpRequest requestPost) =>
		{
			try
			{
				Posts? p = await requestPost.ReadFromJsonAsync<Posts>();
				// Posts? p = JsonConvert.DeserializeObject<Posts>(post.Body.ToString());
				if (p != null)
				{
					Post pd = new Post();
					await pd.getPostByOrganization(p);
				}
				else
				{
					return Results.BadRequest();
				}
				return Results.Ok();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return Results.InternalServerError();
			}
		})
		.WithName("PostPosts");

		return group;
	}
}
