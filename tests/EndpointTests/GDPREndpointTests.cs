using Dto;
using System.Net.Http.Json;

namespace tests.EndpointsTests;

[Collection("httpclient collection")]
public class GDPREndpointTests(HttpClientFixture httpClientFixture)
{
	private readonly HttpClient client = httpClientFixture.client;

	[Fact]
	public async Task Delete_MissingUser_ReturnNull()
	{
		// Arrange
		string email = "incorrectemail";

		// Act
		HttpResponseMessage response = await client.DeleteAsync(
				$"GDPR/{email}",
				TestContext.Current.CancellationToken);
		string? content = await response.Content.ReadAsStringAsync(
				TestContext.Current.CancellationToken);
		Assert.NotNull(content);
		int rowsDeleted = int.Parse(content);

		// Assert
		Assert.True(response.IsSuccessStatusCode);
		Assert.NotNull(content);
		Assert.Equal(0, rowsDeleted);
	}

	[Fact]
	public async Task Delete_ExistingUser_ReturnSuccess()
	{
		// Arrange
		RegisterCredentialsDto registerDto = new RegisterCredentialsDto
		(
			FirstName: "Bobby",
			LastName: "Dobby",
			Email: "bobby@hotmail.com",
			Password: "123password",
			Age: 67
		);
		HttpResponseMessage response1 = await client.PostAsJsonAsync(
				"register",
				registerDto,
				TestContext.Current.CancellationToken);

		// Act
		HttpResponseMessage response2 = await client.DeleteAsync(
				$"GDPR/{registerDto.Email}",
				TestContext.Current.CancellationToken);
		string? content = await response2.Content.ReadAsStringAsync(
				TestContext.Current.CancellationToken);
		Assert.NotNull(content);
		int rowsDeleted = int.Parse(content);

		// Assert
		Assert.True(response1.IsSuccessStatusCode);
		Assert.Equal(1, rowsDeleted);
	}
}