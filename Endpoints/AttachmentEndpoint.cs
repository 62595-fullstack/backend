using backend.getdata;
using Models.Post;
using Models.Attachment;
using Newtonsoft.Json;
using System.Net;
using Models.Attachment;

namespace Endpoints;

public static class AttachmentEndpoint
{
	public static RouteGroupBuilder MapAttachmentEndpoints(this RouteGroupBuilder group)
	{
		group.MapGet("/{attachmentId}", async Task<Attachments> (int attachmentId) =>
		{
			try
			{
				DataAttachment dam = new DataAttachment();
				Attachments am = await dam.GetAttachment(attachmentId);
				return am;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		});

		group.MapPost("/{postsId}", async Task<string> (int postId, IFormFile file) =>
		{
			try
			{
				DataAttachment dam = new DataAttachment();
				await dam.SaveFileToPost(file, postId);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return HttpStatusCode.InternalServerError.ToString();
			}
			return HttpStatusCode.OK.ToString();
		})
		.WithName("PostAttachment")
		.DisableAntiforgery();
		return group;
	}
}
