using Models.User;

namespace Models.UserFriendship;

public class UserFriendships
{
	public int Id { get; set; }

	public required string UserAId { get; set; }

	public required string UserBId { get; set; }

	public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

	public Users? UserA { get; set; }

	public Users? UserB { get; set; }
}