using backend.getdata;
using Models.Post;
using Models.Attachment;
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

		group.MapPost("/", async Task<string> (string post, IFormFile file) =>
		{
			try
			{

				using (DatabaseContext db = new DatabaseContext())
				{
					Posts? p = JsonConvert.DeserializeObject<Posts>(post);

					if (p != null)
					{
						if (file != null)
						{
							// db.Attachment.Add
							using (var stream = file.OpenReadStream())
							{
								using (var memoryStream = new MemoryStream())
								{
									await stream.CopyToAsync(memoryStream);
									byte[] fileBytes = memoryStream.ToArray();
									Attachments am = new Attachments
									{
										FileName = file.FileName,
										FileType = file.GetType().ToString(),
										Content = fileBytes,
										CreatedDate = DateTime.UtcNow,
										PostId = p.Id,
									};
								}
							}
						}

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
		.WithName("PostPosts")
		.DisableAntiforgery();

		return group;
	}
}
