using Dto;
using Models.Post;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace tests.EndpointsTests;

[Collection("httpclient collection")]
public class PostEndpointTest(HttpClientFixture httpClientFixture)
{
	private readonly HttpClient client = httpClientFixture.client;

	[Fact]
	public async Task Post_SendPost_ReturnSuccess()
	{
		// Arrange
		PostDto post = new PostDto
		(
			Title: "Test Post",
			BodyText: "Test Bodytext",
			UserId: 1000,
			OrganizationEventId: 1000
		);
		// Act
		HttpResponseMessage response = await client.PostAsJsonAsync(
				"posts",
				post,
				TestContext.Current.CancellationToken);
		// Assert
		Assert.True(response.IsSuccessStatusCode);
	}

	[Fact]
	public async Task Post_SendWrongPost_ReturnUnsuccessful()
	{
		// Arrange
		StringContent httpContentPost = new StringContent(
				"", new MediaTypeHeaderValue("application/json"));
		// Act
		HttpResponseMessage response = await client.PostAsync(
				"posts",
				httpContentPost,
				TestContext.Current.CancellationToken);
		// Assert
		Assert.False(response.IsSuccessStatusCode);
	}

	[Fact]
	public async Task Get_ReturnPosts()
	{
		// Act
		HttpResponseMessage response = await client.GetAsync(
				"posts",
				TestContext.Current.CancellationToken);
		string jsonPost = await response.Content.ReadAsStringAsync(
				TestContext.Current.CancellationToken);
		// Assert
		Assert.True(response.IsSuccessStatusCode);
		Assert.NotNull(jsonPost);
	}

	// [Fact]
	// public async Task GetPostByIdTest()
	// {
	//     // Act
	//     HttpResponseMessage response = await client.GetAsync(
	//             "posts/" + Convert.ToString(1000),
	//             TestContext.Current.CancellationToken);
	//     string jsonPost = await response.Content.ReadAsStringAsync(
	//             TestContext.Current.CancellationToken);
	//     Console.WriteLine(jsonPost);
	//     Posts? post = JsonConvert.DeserializeObject<Posts>(jsonPost);
	//     // Assert
	//     Assert.True(response.IsSuccessStatusCode);
	//     Assert.NotNull(post);
	//     Assert.Equal("Test Post", post.Title);
	// }
}