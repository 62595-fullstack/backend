using backend.getdata;
using Dto;
using System.Security.Claims;
using System.Text.Json;

namespace Endpoints;

public static class NotificationsEndpoint
{
	private static readonly JsonSerializerOptions _streamJsonOptions = new(JsonSerializerDefaults.Web);

	public static RouteGroupBuilder MapNotificationsEndpoints(this RouteGroupBuilder group)
	{
		group.MapGet("/", async Task<IResult> (ClaimsPrincipal user) =>
		{
			string? currentUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
			if (currentUserId == null) return Results.Unauthorized();

			DataNotification notificationData = new();
			List<NotificationDto> notifications = await notificationData.GetForUser(currentUserId);
			return Results.Ok(notifications);
		})
		.WithName("ListNotifications");

		group.MapPost("/{notificationId:int}/read", async Task<IResult> (ClaimsPrincipal user, int notificationId) =>
		{
			string? currentUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
			if (currentUserId == null) return Results.Unauthorized();

			DataNotification notificationData = new();
			bool ok = await notificationData.MarkRead(currentUserId, notificationId);
			return ok ? Results.Ok() : Results.NotFound();
		})
		.WithName("MarkNotificationRead");

		group.MapPost("/read-all", async Task<IResult> (ClaimsPrincipal user) =>
		{
			string? currentUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
			if (currentUserId == null) return Results.Unauthorized();

			DataNotification notificationData = new();
			int updated = await notificationData.MarkAllRead(currentUserId);
			return Results.Ok(new { updated });
		})
		.WithName("MarkAllNotificationsRead");

		group.MapGet("/stream", async (HttpContext http, ClaimsPrincipal user, CancellationToken ct) =>
		{
			string? currentUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(currentUserId))
			{
				http.Response.StatusCode = StatusCodes.Status401Unauthorized;
				return;
			}

			http.Response.Headers.Append("Content-Type", "text/event-stream");
			http.Response.Headers.Append("Cache-Control", "no-cache");
			http.Response.Headers.Append("X-Accel-Buffering", "no");

			(Guid connectionId, var reader) = NotificationStream.Subscribe(currentUserId);

			try
			{
				await http.Response.WriteAsync(": connected\n\n", ct);
				await http.Response.Body.FlushAsync(ct);

				await foreach (object payload in reader.ReadAllAsync(ct))
				{
					string json = JsonSerializer.Serialize(payload, _streamJsonOptions);
					await http.Response.WriteAsync($"data: {json}\n\n", ct);
					await http.Response.Body.FlushAsync(ct);
				}
			}
			catch (OperationCanceledException) { }
			finally
			{
				NotificationStream.Unsubscribe(currentUserId, connectionId);
			}
		})
		.WithName("StreamNotifications");

		return group;
	}
}
