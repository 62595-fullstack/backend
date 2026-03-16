using backend.getdata;
using Models.Attachment;
using System.Net;

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
		})
		.WithName("GetAttachment");


		group.MapDelete("/{attachmentId}", async Task<string> (int attachmentId) =>
		{
			try
			{
				DataAttachment dam = new DataAttachment();
				await dam.DeleteAttachment(attachmentId);
				return HttpStatusCode.OK.ToString();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return HttpStatusCode.InternalServerError.ToString();
			}
		})
		.WithName("DeleteAttachment");

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
