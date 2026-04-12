namespace tests.EndpointsTests;

[Collection("httpclient collection")]
public class OrganizationEndpointTests(HttpClientFixture httpClientFixture)
{
	private readonly HttpClient client = httpClientFixture.client;
}
