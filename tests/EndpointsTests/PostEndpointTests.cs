using Models.Post;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace tests.EndpointsTests;

[Collection("httpclient collection")]
public class PostEndpointTest(HttpClientFixture httpClientFixture)
{
	private readonly HttpClient client = httpClientFixture.client;

	[Fact]
	public async Task PostPostTest1()
	{
		Console.WriteLine("Should be second");
		// Arrange
		Posts post = new Posts
		{
			Id = 12345,
			Title = "Test Post",
			CreatedDate = DateTime.UtcNow,
			UserId = 1000,
			OrganizationEventId = 1000
		};
		string jsonPost = JsonConvert.SerializeObject(post);
		StringContent httpContentPost = new(
				jsonPost,
				new MediaTypeHeaderValue("application/json")
			);
		// Act
		HttpResponseMessage response = await client.PostAsync(
				"posts",
				httpContentPost,
				TestContext.Current.CancellationToken);
		Console.WriteLine(httpContentPost);
		Console.WriteLine(httpContentPost.Headers);
		// Assert
		Assert.True(response.IsSuccessStatusCode);
	}

	[Fact]
	public async Task PostPostTest2()
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
	public async Task GetPostTest()
	{
		// Act
		HttpResponseMessage response = await client.GetAsync(
				"posts",
				TestContext.Current.CancellationToken);
		string jsonPost = await response.Content.ReadAsStringAsync(
				TestContext.Current.CancellationToken);
		List<Posts>? posts = JsonConvert.DeserializeObject<List<Posts>>(jsonPost);
		// Assert
		Assert.True(response.IsSuccessStatusCode);
		Assert.NotNull(posts);
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