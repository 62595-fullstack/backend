using Dto;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace tests.EndpointsTests;

[Collection("httpClientCollection")]
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
}