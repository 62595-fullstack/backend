namespace Models.Notification;

public class Notifications
{
	public int Id { get; set; }

	public required string UserId { get; set; }

	public required string Type { get; set; }

	public required string Message { get; set; }

	public string? ActorUserId { get; set; }

	public bool Read { get; set; } = false;

	public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
