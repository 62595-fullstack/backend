namespace tests.EndpointsTests;

[Collection("httpClientCollection")]
public class AttachmentEndpointTests(HttpClientFixture httpClientFixture)
{
	private readonly HttpClient client = httpClientFixture.client;

	[Fact]
	public async Task Get_Attachments_ReturnAttachments()
	{
		// Act
		int attachmentId = 1000;
		HttpResponseMessage response = await client.GetAsync(
				$"attachments/{attachmentId}",
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