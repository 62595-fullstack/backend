using backend.getdata;
using Dto;
using System.Security.Claims;

namespace Endpoints;

public static class UserEndpoint
{
	public static RouteGroupBuilder MapUserEndpoints(this RouteGroupBuilder group)
	{
		group.MapGet("/", async Task<IResult> (ClaimsPrincipal user, string? query) =>
		{
			string? currentUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
			if (currentUserId == null) return Results.Unauthorized();

			DataFriendship friendshipData = new();
			List<UserSummaryDto> users = await friendshipData.SearchUsers(currentUserId, query);
			return Results.Ok(users);
		})
		.WithName("SearchUsers");

		group.MapGet("/me/friends", async Task<IResult> (ClaimsPrincipal user) =>
		{
			string? currentUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
			if (currentUserId == null) return Results.Unauthorized();

			DataFriendship friendshipData = new();
			List<FriendSummaryDto> friends = await friendshipData.GetFriendsForUser(currentUserId);
			return Results.Ok(friends);
		})
		.WithName("GetMyFriends");

		group.MapPost("/me/friends", async Task<IResult> (ClaimsPrincipal user, AddFriendDto request) =>
		{
			string? currentUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
			if (currentUserId == null) return Results.Unauthorized();

			DataFriendship friendshipData = new();
			FriendSummaryDto? friend = await friendshipData.AddFriend(currentUserId, request.FriendUserId);
			return friend == null
				? Results.BadRequest("Unable to create friendship.")
				: Results.Ok(friend);
		})
		.WithName("AddFriend");

		group.MapDelete("/me/friends/{friendUserId}", async Task<IResult> (ClaimsPrincipal user, string friendUserId) =>
		{
			string? currentUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
			if (currentUserId == null) return Results.Unauthorized();

			DataFriendship friendshipData = new();
			bool removed = await friendshipData.RemoveFriend(currentUserId, friendUserId);
			return removed ? Results.Ok() : Results.NotFound();
		})
		.WithName("RemoveFriend");

		return group;
	}
}
