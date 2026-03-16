using System.Net.Http.Headers;

namespace testing;

public class UnitTest1
{
    static HttpClient client;

    public UnitTest1()
    {
        client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5000/")
        };
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    [Fact]
    public async Task Test1()
    {
        HttpResponseMessage response = await client.GetAsync("posts/", TestContext.Current.CancellationToken);
        Console.WriteLine(response.StatusCode);
        Assert.True(response.IsSuccessStatusCode);
        // Assert.True(true);
    }
}
