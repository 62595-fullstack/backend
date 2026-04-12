namespace tests.EndpointsTests;

[Collection("httpclient collection")]
public class GDPREndpointTests(HttpClientFixture httpClientFixture)
{
	private readonly HttpClient client = httpClientFixture.client;

	[Fact]
	public void Test()
	{

		// [DefaultValue("Bob")] string firstName,
		// [DefaultValue("Bob@hotmail.com")] string email,
		// [DefaultValue("12345password")] string password,
		// [DefaultValue(25)] int age) =>
		//
		// HttpContent httpContent = new HttpContent();
		// client.PostAsync('register',

	}
}