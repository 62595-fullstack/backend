using backend.getdata;
using Models.Post;
using Models.Attachment;
using Newtonsoft.Json;
using System.Net;

namespace Endpoints;

public static class AttachmentEndpoint
{
	public static RouteGroupBuilder MapAttachmentEndpoints(this RouteGroupBuilder group)
	{
		// group.MapGet("/", async Task<Attachment> () => {
		// });
		group.MapPost("/{postsId}", async Task<string> (int postId, IFormFile file) =>
		{
			try
			{
				DataAttachment dam = new DataAttachment();
				await dam.SaveFileToPost(file, postId);

				return HttpStatusCode.OK.ToString();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return HttpStatusCode.InternalServerError.ToString();
			}
		})
		.WithName("PostAttachment")
		.DisableAntiforgery();
		return group;
	}
}
