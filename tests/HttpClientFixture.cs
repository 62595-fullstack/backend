using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace tests;

public class HttpClientFixture : IAsyncLifetime
{
	public readonly HttpClient client;
	public string? jwtToken = null;

	public HttpClientFixture()
	{
		client = new HttpClient
		{
			BaseAddress = new Uri("http://localhost:5000")
		};
	}

	[Fact]
	public async ValueTask InitializeAsync()
	{
		// Arrange
		// LoginCredentials loginCredentials = new("crazyfrog@hotmail.com", "bingbing");
		var loginCredentials = new
		{
			Email = "crazyfrog@hotmail.com",
			Password = "bingbing"
		};
		// Act
		HttpResponseMessage response = await client.PostAsJsonAsync(
				"login",
				loginCredentials, // loginCredentials,
				TestContext.Current.CancellationToken);
		jwtToken = await response.Content.ReadFromJsonAsync<string>(cancellationToken: TestContext.Current.CancellationToken);
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

		// Assert
		Assert.NotNull(jwtToken);
		Assert.True(response.IsSuccessStatusCode);
	}

	public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}