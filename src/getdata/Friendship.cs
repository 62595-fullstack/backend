using Dto;
using Microsoft.EntityFrameworkCore;
using Models.User;
using Models.UserFriendship;

namespace backend.getdata;

public class DataFriendship
{
	private static (string userAId, string userBId) NormalizePair(string firstUserId, string secondUserId)
	{
		return string.CompareOrdinal(firstUserId, secondUserId) <= 0
			? (firstUserId, secondUserId)
			: (secondUserId, firstUserId);
	}

	private static UserSummaryDto ToUserSummaryDto(Users user)
	{
		return new UserSummaryDto(
			user.Id,
			user.Email ?? string.Empty,
			user.FirstName,
			user.UserName ?? user.FirstName,
			user.Age
		);
	}

	private static FriendSummaryDto ToFriendSummaryDto(Users user, DateTime friendsSince)
	{
		return new FriendSummaryDto(
			user.Id,
			user.Email ?? string.Empty,
			user.FirstName,
			user.UserName ?? user.FirstName,
			user.Age,
			friendsSince
		);
	}

	public async Task<List<UserSummaryDto>> SearchUsers(string currentUserId, string? query)
	{
		using DatabaseContext db = new();

		IQueryable<Users> usersQuery = db.User
			.AsNoTracking()
			.Where(user => user.Id != currentUserId);

		if (!string.IsNullOrWhiteSpace(query))
		{
			string search = query.Trim().ToLower();
			usersQuery = usersQuery.Where(user =>
				(user.FirstName != null && user.FirstName.ToLower().Contains(search)) ||
				(user.UserName != null && user.UserName.ToLower().Contains(search)) ||
				(user.Email != null && user.Email.ToLower().Contains(search)));
		}

		List<Users> users = await usersQuery
			.OrderBy(user => user.FirstName)
			.ThenBy(user => user.UserName)
			.Take(25)
			.ToListAsync();

		return users.Select(ToUserSummaryDto).ToList();
	}

	public async Task<List<FriendSummaryDto>> GetFriendsForUser(string userId)
	{
		using DatabaseContext db = new();

		List<UserFriendships> friendships = await db.UserFriendship
			.AsNoTracking()
			.Where(friendship => friendship.UserAId == userId || friendship.UserBId == userId)
			.OrderByDescending(friendship => friendship.CreatedDate)
			.ToListAsync();

		List<string> friendIds = friendships
			.Select(friendship => friendship.UserAId == userId ? friendship.UserBId : friendship.UserAId)
			.Distinct()
			.ToList();

		Dictionary<string, Users> usersById = await db.User
			.AsNoTracking()
			.Where(user => friendIds.Contains(user.Id))
			.ToDictionaryAsync(user => user.Id);

		return friendships
			.Select(friendship =>
			{
				string friendId = friendship.UserAId == userId ? friendship.UserBId : friendship.UserAId;
				return usersById.TryGetValue(friendId, out Users? friend)
					? ToFriendSummaryDto(friend, friendship.CreatedDate)
					: null;
			})
			.Where(friend => friend != null)
			.Cast<FriendSummaryDto>()
			.ToList();
	}

	public async Task<FriendSummaryDto?> AddFriend(string currentUserId, string friendUserId)
	{
		if (string.IsNullOrWhiteSpace(friendUserId) || currentUserId == friendUserId)
		{
			return null;
		}

		using DatabaseContext db = new();

		Users? currentUser = await db.User.FirstOrDefaultAsync(user => user.Id == currentUserId);
		Users? friendUser = await db.User.FirstOrDefaultAsync(user => user.Id == friendUserId);

		if (currentUser == null || friendUser == null)
		{
			return null;
		}

		(string userAId, string userBId) = NormalizePair(currentUserId, friendUserId);
		UserFriendships? existingFriendship = await db.UserFriendship
			.AsNoTracking()
			.FirstOrDefaultAsync(friendship => friendship.UserAId == userAId && friendship.UserBId == userBId);

		if (existingFriendship != null)
		{
			return ToFriendSummaryDto(friendUser, existingFriendship.CreatedDate);
		}

		UserFriendships friendship = new()
		{
			UserAId = userAId,
			UserBId = userBId,
			CreatedDate = DateTime.UtcNow
		};

		db.UserFriendship.Add(friendship);
		await db.SaveChangesAsync();

		return ToFriendSummaryDto(friendUser, friendship.CreatedDate);
	}

	public async Task<bool> RemoveFriend(string currentUserId, string friendUserId)
	{
		if (string.IsNullOrWhiteSpace(friendUserId) || currentUserId == friendUserId)
		{
			return false;
		}

		using DatabaseContext db = new();
		(string userAId, string userBId) = NormalizePair(currentUserId, friendUserId);

		UserFriendships? friendship = await db.UserFriendship
			.FirstOrDefaultAsync(existing => existing.UserAId == userAId && existing.UserBId == userBId);

		if (friendship == null)
		{
			return false;
		}

		db.UserFriendship.Remove(friendship);
		await db.SaveChangesAsync();
		return true;
	}
}
