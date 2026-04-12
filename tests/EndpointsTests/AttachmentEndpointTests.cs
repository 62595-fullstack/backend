namespace tests.EndpointsTests;

[Collection("httpclient collection")]
public class AttachmentEndpointTests(HttpClientFixture httpClientFixture)
{
	private readonly HttpClient client = httpClientFixture.client;
}
