using Dto;
using Microsoft.EntityFrameworkCore;
using Models.FriendRequest;
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
			user.LastName,
			user.UserName ?? user.FirstName,
			user.DateOfBirth,
			user.Bio
		);
	}

	private static FriendSummaryDto ToFriendSummaryDto(Users user, DateTime friendsSince)
	{
		return new FriendSummaryDto(
			user.Id,
			user.Email ?? string.Empty,
			user.FirstName,
			user.LastName,
			user.UserName ?? user.FirstName,
			user.DateOfBirth,
			friendsSince
		);
	}

	public async Task<List<UserSummaryDto>> SearchUsers(string currentUserId, string? query)
	{
		await using DatabaseContext db = new();

		IQueryable<Users> usersQuery = db.User
			.AsNoTracking()
			.Where(user => user.Id != currentUserId);

		if (!string.IsNullOrWhiteSpace(query))
		{
			string search = query.Trim().ToLower();
			usersQuery = usersQuery.Where(user =>
				(user.FirstName + " " + user.LastName).ToLower().Contains(search));
		}

		return await usersQuery
			.OrderBy(user => user.FirstName)
			.ThenBy(user => user.UserName)
			.Take(25)
			.Select(user => new UserSummaryDto(
				user.Id,
				user.Email ?? string.Empty,
				user.FirstName,
				user.LastName,
				user.UserName ?? user.FirstName,
				user.DateOfBirth,
				user.Bio))
			.ToListAsync();
	}

	public async Task<List<FriendSummaryDto>> GetFriendsForUser(string userId)
	{
		await using DatabaseContext db = new();

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

	public enum FriendshipStatus { None, Friends, OutgoingPending, IncomingPending }

	public async Task<FriendshipStatus> GetStatusBetween(string userAId, string userBId)
	{
		if (string.IsNullOrWhiteSpace(userAId) || string.IsNullOrWhiteSpace(userBId) || userAId == userBId)
		{
			return FriendshipStatus.None;
		}

		await using DatabaseContext db = new();
		(string lo, string hi) = NormalizePair(userAId, userBId);

		bool friends = await db.UserFriendship
			.AsNoTracking()
			.AnyAsync(f => f.UserAId == lo && f.UserBId == hi);
		if (friends) return FriendshipStatus.Friends;

		bool outgoing = await db.FriendRequest
			.AsNoTracking()
			.AnyAsync(r => r.RequesterId == userAId && r.RecipientId == userBId);
		if (outgoing) return FriendshipStatus.OutgoingPending;

		bool incoming = await db.FriendRequest
			.AsNoTracking()
			.AnyAsync(r => r.RequesterId == userBId && r.RecipientId == userAId);
		if (incoming) return FriendshipStatus.IncomingPending;

		return FriendshipStatus.None;
	}

	public async Task<List<FriendRequestDto>> GetIncomingRequests(string userId)
	{
		await using DatabaseContext db = new();
		List<FriendRequests> requests = await db.FriendRequest
			.AsNoTracking()
			.Where(r => r.RecipientId == userId)
			.OrderByDescending(r => r.CreatedDate)
			.ToListAsync();

		List<string> requesterIds = requests.Select(r => r.RequesterId).Distinct().ToList();
		Dictionary<string, Users> usersById = await db.User
			.AsNoTracking()
			.Where(u => requesterIds.Contains(u.Id))
			.ToDictionaryAsync(u => u.Id);

		return requests
			.Select(r => usersById.TryGetValue(r.RequesterId, out Users? requester)
				? new FriendRequestDto(r.Id, r.RequesterId, requester.FirstName, requester.LastName, r.CreatedDate)
				: null)
			.Where(dto => dto != null)
			.Cast<FriendRequestDto>()
			.ToList();
	}

	/// <summary>
	/// Sends a friend request, or returns "Friends" if a reverse-request was auto-accepted.
	/// </summary>
	public async Task<FriendshipStatus> SendFriendRequest(string requesterId, string recipientId)
	{
		if (string.IsNullOrWhiteSpace(recipientId) || requesterId == recipientId)
		{
			return FriendshipStatus.None;
		}

		await using DatabaseContext db = new();

		Users? requester = await db.User.FirstOrDefaultAsync(user => user.Id == requesterId);
		Users? recipient = await db.User.FirstOrDefaultAsync(user => user.Id == recipientId);
		if (requester == null || recipient == null) return FriendshipStatus.None;

		(string lo, string hi) = NormalizePair(requesterId, recipientId);
		bool alreadyFriends = await db.UserFriendship
			.AsNoTracking()
			.AnyAsync(f => f.UserAId == lo && f.UserBId == hi);
		if (alreadyFriends) return FriendshipStatus.Friends;

		FriendRequests? reverseRequest = await db.FriendRequest
			.FirstOrDefaultAsync(r => r.RequesterId == recipientId && r.RecipientId == requesterId);
		if (reverseRequest != null)
		{
			await CompleteFriendship(db, reverseRequest.RequesterId, reverseRequest.RecipientId);
			return FriendshipStatus.Friends;
		}

		FriendRequests? existingForward = await db.FriendRequest
			.AsNoTracking()
			.FirstOrDefaultAsync(r => r.RequesterId == requesterId && r.RecipientId == recipientId);
		if (existingForward != null) return FriendshipStatus.OutgoingPending;

		FriendRequests request = new()
		{
			RequesterId = requesterId,
			RecipientId = recipientId,
			CreatedDate = DateTime.UtcNow,
		};
		db.FriendRequest.Add(request);
		await db.SaveChangesAsync();

		DataNotification notificationData = new();
		NotificationDto notification = await notificationData.Create(
			userId: recipientId,
			type: "friend_request",
			message: $"{requester.FirstName} {requester.LastName} sent you a friend request.",
			actorUserId: requesterId
		);
		NotificationStream.Publish(recipientId, notification);

		return FriendshipStatus.OutgoingPending;
	}

	public async Task<bool> AcceptFriendRequest(string recipientId, string requesterId)
	{
		await using DatabaseContext db = new();

		FriendRequests? request = await db.FriendRequest
			.FirstOrDefaultAsync(r => r.RequesterId == requesterId && r.RecipientId == recipientId);
		if (request == null) return false;

		await CompleteFriendship(db, requesterId, recipientId);
		return true;
	}

	public async Task<bool> DeclineFriendRequest(string recipientId, string requesterId)
	{
		await using DatabaseContext db = new();

		FriendRequests? request = await db.FriendRequest
			.FirstOrDefaultAsync(r => r.RequesterId == requesterId && r.RecipientId == recipientId);
		if (request == null) return false;

		db.FriendRequest.Remove(request);
		await db.SaveChangesAsync();
		return true;
	}

	public async Task<bool> CancelFriendRequest(string requesterId, string recipientId)
	{
		await using DatabaseContext db = new();

		FriendRequests? request = await db.FriendRequest
			.FirstOrDefaultAsync(r => r.RequesterId == requesterId && r.RecipientId == recipientId);
		if (request == null) return false;

		db.FriendRequest.Remove(request);
		await db.SaveChangesAsync();
		return true;
	}

	/// <summary>
	/// Creates the friendship row, deletes any pending requests between the pair, and notifies the original requester.
	/// Caller is responsible for confirming the request exists.
	/// </summary>
	private async Task CompleteFriendship(DatabaseContext db, string requesterId, string recipientId)
	{
		(string lo, string hi) = NormalizePair(requesterId, recipientId);

		List<FriendRequests> openRequests = await db.FriendRequest
			.Where(r => (r.RequesterId == requesterId && r.RecipientId == recipientId)
					 || (r.RequesterId == recipientId && r.RecipientId == requesterId))
			.ToListAsync();
		db.FriendRequest.RemoveRange(openRequests);

		UserFriendships friendship = new()
		{
			UserAId = lo,
			UserBId = hi,
			CreatedDate = DateTime.UtcNow,
		};
		db.UserFriendship.Add(friendship);
		await db.SaveChangesAsync();

		Users? recipient = await db.User.AsNoTracking().FirstOrDefaultAsync(u => u.Id == recipientId);
		string recipientName = recipient != null ? $"{recipient.FirstName} {recipient.LastName}" : "Someone";

		DataNotification notificationData = new();
		NotificationDto notification = await notificationData.Create(
			userId: requesterId,
			type: "friend_added",
			message: $"{recipientName} accepted your friend request.",
			actorUserId: recipientId
		);
		NotificationStream.Publish(requesterId, notification);
	}

	public async Task<bool> RemoveFriend(string currentUserId, string friendUserId)
	{
		if (string.IsNullOrWhiteSpace(friendUserId) || currentUserId == friendUserId)
		{
			return false;
		}

		await using DatabaseContext db = new();
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
