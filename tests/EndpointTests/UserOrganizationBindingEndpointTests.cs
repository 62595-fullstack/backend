namespace tests.EndpointsTests;

[Collection("httpclient collection")]
public class UserOrganizationBindingEndpointTests(HttpClientFixture httpClientFixture)
{
	private readonly HttpClient client = httpClientFixture.client;

	[Fact]
	public async Task Get_Organizations_ReturnOrganizations()
	{
		// Arrange
		int organizationId = 1000;
		// Act
		HttpResponseMessage response = await client.GetAsync(
				$"UserOrganizationBinding/{organizationId}",
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