using backend.getdata;
using Microsoft.EntityFrameworkCore;
using Models.Post;
using Newtonsoft.Json;
using System.Net;

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

		group.MapGet("/{organizationsId}", async Task<string> (int organizationsId) =>
		{
			try
			{
					Post p = new Post();
					List<Posts>? posts = await p.getPostByOrganization(organizationsId);
					return JsonConvert.SerializeObject(posts);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return "{}";
			}
		})
		.WithName("GetPostsFromOrganizationsId");

		group.MapPost("/", async Task<string> (string post) =>
		{
			try
			{
				using (DatabaseContext db = new DatabaseContext())
				{
					Posts? p = JsonConvert.DeserializeObject<Posts>(post);

					if (p != null)
					{
						Post pd = new Post();
						await pd.getPostByOrganization(p);
					}
					else
					{
						return HttpStatusCode.InternalServerError.ToString();
					}
					return HttpStatusCode.OK.ToString();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return HttpStatusCode.InternalServerError.ToString();
			}
		})
		.WithName("PostPosts");

		return group;
	}
}
