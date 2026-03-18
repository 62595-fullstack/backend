using System.Net.Http.Headers;
using Models.Post;
using Newtonsoft.Json;

namespace tests;

public class PostEndpointTest
{
    private readonly HttpClient client;

    public PostEndpointTest()
    {
        client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5000")
        };
    }

    [Fact]
    public async Task PostPostTest1()
    {
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
        StringContent httpContentPost = new StringContent(
                jsonPost, new MediaTypeHeaderValue("application/json"));
        // Act
        HttpResponseMessage response = await client.PostAsync(
                "posts",
                httpContentPost,
                TestContext.Current.CancellationToken);
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
