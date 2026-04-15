namespace tests.EndpointsTests;

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
}