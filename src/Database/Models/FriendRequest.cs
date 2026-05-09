namespace Models.FriendRequest;

public class FriendRequests
{
	public int Id { get; set; }

	public required string RequesterId { get; set; }

	public required string RecipientId { get; set; }

	public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
