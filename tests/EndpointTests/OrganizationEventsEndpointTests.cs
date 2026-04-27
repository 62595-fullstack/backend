namespace tests.EndpointsTests;

using System.Net.Http.Json;
using System.Text.Json;

[Collection("httpClientCollection")]
public class OrganizationEventsEndpointTests(HttpClientFixture httpClientFixture)
{
	private readonly HttpClient client = httpClientFixture.client;

	[Fact]
	public async Task Get_OrganizationEvents_ReturnOrganizationEvents()
	{
		// Act
		int organizationId = 1000;
		HttpResponseMessage response = await client.GetAsync(
				$"OrganizationEvents/{organizationId}",
				TestContext.Current.CancellationToken);
		string? content = await response.Content.ReadAsStringAsync(
				TestContext.Current.CancellationToken);
		// List<Organizations>? organizations = JsonConvert.DeserializeObject<List<Organizations>>(content);
		// Assert
		Assert.True(response.IsSuccessStatusCode);
		Assert.NotNull(content);
		// Assert.NotNull(organizations);
	}

	[Fact]
	public async Task Patch_BracketResults_PersistsOnEvent()
	{
		// Arrange
		int eventId = 123;
		string bracketResults = JsonSerializer.Serialize(new Dictionary<string, string>
		{
			["round-1-match-1"] = $"test-winner-{Guid.NewGuid()}"
		});

		var update = new
		{
			BracketResults = bracketResults
		};

		// Act
		HttpResponseMessage patchResponse = await client.PatchAsJsonAsync(
				$"OrganizationEvents/{eventId}",
				update,
				TestContext.Current.CancellationToken);

		HttpResponseMessage getResponse = await client.GetAsync(
				$"OrganizationEvents/event/{eventId}",
				TestContext.Current.CancellationToken);
		string eventJson = await getResponse.Content.ReadAsStringAsync(
				TestContext.Current.CancellationToken);

		Assert.True(patchResponse.IsSuccessStatusCode);
		Assert.True(getResponse.IsSuccessStatusCode);

		using JsonDocument document = JsonDocument.Parse(eventJson);
		JsonElement root = document.RootElement;
		string savedBracketResults = root.TryGetProperty("bracketResults", out JsonElement camelCaseValue)
			? camelCaseValue.GetString() ?? string.Empty
			: root.GetProperty("BracketResults").GetString() ?? string.Empty;

		// Assert
		Assert.Equal(bracketResults, savedBracketResults);
	}
}
