using backend.getdata;
using Dto;
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
		})
		.WithName("GetAttachment");

		group.MapDelete("/{attachmentId}", async (int attachmentId) =>
		{
			try
			{
				DataAttachment dam = new DataAttachment();
				bool success = await dam.DeleteAttachment(attachmentId);
				if (!success)
				{
					return Results.NotFound();
				}
				return Results.Ok();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return Results.BadRequest();
			}
		})
		.WithName("DeleteAttachment");

		group.MapPost("/profilepicture/{organizationId}", async Task<IResult> (AttachmentDto attachment, int organizationId) =>
		{
			try
			{
				DataAttachment dam = new DataAttachment();
				bool success = await dam.AddOrganizationCoverPhoto(attachment, organizationId);
				if (success) return Results.InternalServerError();
				return Results.Ok();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return Results.InternalServerError();
			}
		})
		.WithName("PostOrganizationProfilepicture")
		.DisableAntiforgery();

		group.MapPost("/coverphoto/{organizationId}", async Task<IResult> (AttachmentDto attachment, int organizationId) =>
			{
				try
				{
					DataAttachment dam = new DataAttachment();
					bool success = await dam.AddOrganizationCoverPhoto(attachment, organizationId);
					if (success) return Results.InternalServerError();
					return Results.Ok();
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					return Results.InternalServerError();
				}
			})
			.WithName("PostOrganizationCoverphoto")
			.DisableAntiforgery();

		return group;
	}

}