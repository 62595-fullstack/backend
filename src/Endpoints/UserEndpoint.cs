using backend.getdata;
using Dto;
using Models.Post;
using Models.User;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Endpoints;

public static class UserEndpoint
{
	public static RouteGroupBuilder MapUserEndpoints(this RouteGroupBuilder group)
	{
		group.MapGet("/", async Task<IResult> (ClaimsPrincipal user, string? query, DataFriendship friendshipData) =>
		{
			string? currentUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
			if (currentUserId == null) return Results.Unauthorized();

			List<UserSummaryDto> users = await friendshipData.SearchUsers(currentUserId, query);
			return Results.Ok(users);
		})
		.WithName("SearchUsers");

		group.MapGet("/me", async Task<IResult> (ClaimsPrincipal user, DataUser userData) =>
		{
			string? currentUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
			if (currentUserId == null) return Results.Unauthorized();

			Users? u = await userData.GetUserById(currentUserId);
			if (u == null) return Results.NotFound();

			return Results.Ok(new UserSummaryDto(u.Id, u.Email ?? "", u.FirstName, u.LastName, u.UserName ?? u.FirstName, u.DateOfBirth, u.Bio));
		})
		.WithName("GetMe");

		group.MapPatch("/me", async Task<IResult> (ClaimsPrincipal user, UpdateProfileDto request, DataUser userData) =>
		{
			string? currentUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
			if (currentUserId == null) return Results.Unauthorized();

			Users? u = await userData.UpdateProfile(currentUserId, request);
			if (u == null) return Results.NotFound();

			return Results.Ok(new UserSummaryDto(u.Id, u.Email ?? "", u.FirstName, u.LastName, u.UserName ?? u.FirstName, u.DateOfBirth, u.Bio));
		})
		.WithName("UpdateMe");

		group.MapGet("/me/friends", async Task<IResult> (ClaimsPrincipal user, DataFriendship friendshipData) =>
		{
			string? currentUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
			if (currentUserId == null) return Results.Unauthorized();

			List<FriendSummaryDto> friends = await friendshipData.GetFriendsForUser(currentUserId);
			return Results.Ok(friends);
		})
		.WithName("GetMyFriends");

		group.MapPost("/me/friends", async Task<IResult> (ClaimsPrincipal user, AddFriendDto request, DataFriendship friendshipData) =>
		{
			string? currentUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
			if (currentUserId == null) return Results.Unauthorized();

			FriendSummaryDto? friend = await friendshipData.AddFriend(currentUserId, request.FriendUserId);
			return friend == null
				? Results.BadRequest("Unable to create friendship.")
				: Results.Ok(friend);
		})
		.WithName("AddFriend");

		group.MapDelete("/me/friends/{friendUserId}", async Task<IResult> (ClaimsPrincipal user, string friendUserId, DataFriendship friendshipData) =>
		{
			string? currentUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
			if (currentUserId == null) return Results.Unauthorized();

			bool removed = await friendshipData.RemoveFriend(currentUserId, friendUserId);
			return removed ? Results.Ok() : Results.NotFound();
		})
		.WithName("RemoveFriend");

		group.MapGet("/{userId}", async Task<IResult> (string userId, DataUser userData) =>
		{
			Users? u = await userData.GetUserById(userId);
			if (u == null) return Results.NotFound();

			return Results.Ok(new UserSummaryDto(u.Id, u.Email ?? "", u.FirstName, u.LastName, u.UserName ?? u.FirstName, u.DateOfBirth, u.Bio));
		})
		.WithName("GetUserById");

		group.MapGet("/{userId}/friends", async Task<IResult> (string userId, DataFriendship friendshipData) =>
		{
			List<FriendSummaryDto> friends = await friendshipData.GetFriendsForUser(userId);
			return Results.Ok(friends);
		})
		.WithName("GetFriendsByUser");

		group.MapGet("/{userId}/posts", async Task<IResult> (string userId, DataPost postData) =>
		{
			List<Posts> posts = await postData.GetPostsByUser(userId);
			return Results.Ok(JsonConvert.SerializeObject(posts));
		})
		.WithName("GetPostsByUser");

		return group;
	}
}