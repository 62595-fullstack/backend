using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;

namespace tests;

public class HttpClientFixture : IAsyncLifetime
{
	public readonly HttpClient client;
	public string? jwtToken = null;

	public HttpClientFixture()
	{
		IConfigurationRoot config = new ConfigurationBuilder()
					.AddEnvironmentVariables()
					.AddUserSecrets(Assembly.GetExecutingAssembly())
					.Build();
		client = new HttpClient
		{
			BaseAddress = new Uri($"http://{config["testHost"]}:{config["testPort"]}")
		};
	}

	[Fact]
	public async ValueTask InitializeAsync()
	{
		// Arrange
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