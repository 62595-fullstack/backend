using backend.getdata;
using Dto;
using Models.Post;
using Newtonsoft.Json;

namespace Endpoints;

public static class PostEndpoint
{
	public static RouteGroupBuilder MapPostEndpoints(this RouteGroupBuilder group)
	{
		group.MapGet("/", async Task<IResult> (DataPost dataPost) =>
		{
			try
			{
				List<Posts>? allPost = await dataPost.getAllPost();
				string allPostsJson = JsonConvert.SerializeObject(allPost);
				return Results.Ok(allPostsJson);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return Results.BadRequest();
			}
		})
		.WithName("GetPosts");

		group.MapGet("/{organizationsId}", async Task<IResult> (int organizationsId, DataPost dataPost) =>
		{
			try
			{
				List<Posts>? posts = await dataPost.getPostByOrganization(organizationsId);
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

		group.MapPost("/", async Task<IResult> (PostDto p, DataPost dataPost) =>
		{
			try
			{
				if (p != null)
				{
					bool success = await dataPost.AddPost(p);
					if (!success)
					{
						return Results.BadRequest();
					}
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