namespace tests.EndpointsTests;

[Collection("httpclient collection")]
public class OrganizationEventsEndpointTests(HttpClientFixture httpClientFixture)
{
	private readonly HttpClient client = httpClientFixture.client;
}