using Microsoft.EntityFrameworkCore;
using Models.Post;
using Newtonsoft.Json;
using System.Net;

namespace Endpoint.PostEndpoint;

public static class PostEndpoint
{
	public static RouteGroupBuilder MapPostEndpoints(this RouteGroupBuilder group)
	{
		group.MapGet("/", string () =>
		{
			try
			{
				using (DatabaseContext db = new DatabaseContext())
				{
					Task<List<Posts>> posts = db.Post.ToListAsync();
					return JsonConvert.SerializeObject(posts);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return "{}";
			}
		})
		.WithName("GetPosts");

		group.MapGet("/{organizationsId}", string (int organizationsId) =>
		{
			try
			{
				using (DatabaseContext db = new DatabaseContext())
				{
					Task<List<Posts>> posts = db.Post.ToListAsync();
					return JsonConvert.SerializeObject(posts);
				}
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
					db.Post.Add(p);
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
