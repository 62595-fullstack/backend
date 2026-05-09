using Dto;
using Microsoft.EntityFrameworkCore;
using Models.Notification;

namespace backend.getdata;

public class DataNotification
{
	private static NotificationDto ToDto(Notifications notification) => new(
		notification.Id,
		notification.UserId,
		notification.Type,
		notification.Message,
		notification.ActorUserId,
		notification.Read,
		notification.CreatedDate
	);

	public async Task<List<NotificationDto>> GetForUser(string userId)
	{
		await using DatabaseContext db = new();
		return await db.Notification
			.AsNoTracking()
			.Where(notification => notification.UserId == userId)
			.OrderByDescending(notification => notification.CreatedDate)
			.Take(100)
			.Select(notification => new NotificationDto(
				notification.Id,
				notification.UserId,
				notification.Type,
				notification.Message,
				notification.ActorUserId,
				notification.Read,
				notification.CreatedDate))
			.ToListAsync();
	}

	public async Task<NotificationDto> Create(string userId, string type, string message, string? actorUserId)
	{
		await using DatabaseContext db = new();
		Notifications notification = new()
		{
			UserId = userId,
			Type = type,
			Message = message,
			ActorUserId = actorUserId,
		};
		db.Notification.Add(notification);
		await db.SaveChangesAsync();
		return ToDto(notification);
	}

	public async Task<bool> MarkRead(string userId, int notificationId)
	{
		await using DatabaseContext db = new();
		Notifications? notification = await db.Notification
			.FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);
		if (notification == null) return false;
		if (notification.Read) return true;
		notification.Read = true;
		await db.SaveChangesAsync();
		return true;
	}

	public async Task<int> MarkAllRead(string userId)
	{
		await using DatabaseContext db = new();
		return await db.Notification
			.Where(n => n.UserId == userId && !n.Read)
			.ExecuteUpdateAsync(setters => setters.SetProperty(n => n.Read, true));
	}

	public async Task<bool> Delete(string userId, int notificationId)
	{
		await using DatabaseContext db = new();
		Notifications? notification = await db.Notification
			.FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);
		if (notification == null) return false;
		db.Notification.Remove(notification);
		await db.SaveChangesAsync();
		return true;
	}
}
