using backend.getdata;
using Dto;
using Models.Post;
using Newtonsoft.Json;

namespace Endpoints;

public static class PostEndpoint
{
	public static RouteGroupBuilder MapPostEndpoints(this RouteGroupBuilder group)
	{
		group.MapGet("/", async Task<string> () =>
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

		group.MapPost("/", async Task<IResult> (PostDto p) =>
		{
			try
			{
				if (p != null)
				{
					Post pd = new Post();
					bool success = await pd.AddPost(p);
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