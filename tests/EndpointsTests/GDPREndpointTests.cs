namespace tests.EndpointsTests;

[Collection("httpclient collection")]
public class GDPREndpointTests(HttpClientFixture httpClientFixture)
{
	private readonly HttpClient client = httpClientFixture.client;
}
