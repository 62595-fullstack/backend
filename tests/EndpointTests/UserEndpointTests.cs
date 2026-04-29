using Dto;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace tests.EndpointsTests;

[Collection("httpclient collection")]
public class UserEndpointTests(HttpClientFixture httpClientFixture)
{
	private readonly HttpClient client = httpClientFixture.client;

	[Fact]
	public async Task Get_Users_ReturnUsers()
	{
		HttpResponseMessage response = await client.GetAsync(
			"users?query=frisk",
			TestContext.Current.CancellationToken);
		List<UserSearchResultDto>? users = await response.Content.ReadFromJsonAsync<List<UserSearchResultDto>>(
			cancellationToken: TestContext.Current.CancellationToken);

		Assert.True(response.IsSuccessStatusCode);
		Assert.NotNull(users);
		Assert.Contains(users, user => user.FirstName == "Frisk");
	}

	[Fact]
	public async Task Get_MyFriends_ReturnSeededFriendships()
	{
		HttpResponseMessage response = await client.GetAsync(
			"users/me/friends",
			TestContext.Current.CancellationToken);
		List<FriendSummaryDto>? friends = await response.Content.ReadFromJsonAsync<List<FriendSummaryDto>>(
			cancellationToken: TestContext.Current.CancellationToken);

		Assert.True(response.IsSuccessStatusCode);
		Assert.NotNull(friends);
		Assert.Contains(friends, friend => friend.Email == "friskfyr@friskefyre.com");
	}

	[Fact]
	public async Task Post_And_Delete_Friend_Works()
	{
		HttpResponseMessage addResponse = await client.PostAsJsonAsync(
			"users/me/friends",
			new AddFriendDto("9003"),
			TestContext.Current.CancellationToken);
		string addResponseBody = await addResponse.Content.ReadAsStringAsync(
			TestContext.Current.CancellationToken);

		Assert.True(
			addResponse.IsSuccessStatusCode,
			$"POST /users/me/friends failed with {(int)addResponse.StatusCode} {addResponse.StatusCode}: {addResponseBody}");

		FriendSummaryDto? addedFriend = JsonSerializer.Deserialize<FriendSummaryDto>(
			addResponseBody,
			new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});
		Assert.NotNull(addedFriend);
		Assert.Equal("9003", addedFriend.Id);

		HttpResponseMessage deleteResponse = await client.DeleteAsync(
			"users/me/friends/9003",
			TestContext.Current.CancellationToken);

		Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
	}
}