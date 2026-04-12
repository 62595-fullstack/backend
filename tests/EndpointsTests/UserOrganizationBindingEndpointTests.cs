namespace tests.EndpointsTests;

[Collection("httpclient collection")]
public class UserOrganizationBindingEndpointTests(HttpClientFixture httpClientFixture)
{
	private readonly HttpClient client = httpClientFixture.client;
}
