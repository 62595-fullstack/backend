namespace tests.EndpointsTests;

using System.Net;
using System.Net.Http.Json;

[Collection("httpClientCollection")]
public class EventParticipantsEndpointTests(HttpClientFixture httpClientFixture)
{
    private readonly HttpClient client = httpClientFixture.client;

    [Fact]
    public async Task Get_EventParticipants_ReturnsOkWithList()
    {
        // Arrange
        int eventId = 1000;

        // Act
        HttpResponseMessage response = await client.GetAsync(
            $"OrganizationEvents/{eventId}/participants",
            TestContext.Current.CancellationToken);
        string? content = await response.Content.ReadAsStringAsync(
            TestContext.Current.CancellationToken);

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(content);
    }

    [Fact]
    public async Task Get_IsRegistered_WhenNotRegistered_ReturnsFalse()
    {
        // Arrange
        int eventId = 123;

        // Act
        HttpResponseMessage response = await client.GetAsync(
            $"OrganizationEvents/{eventId}/is-registered",
            TestContext.Current.CancellationToken);
        bool registered = await response.Content.ReadFromJsonAsync<bool>(
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.False(registered);
    }

    [Fact]
    public async Task Post_JoinEvent_ReturnsOk()
    {
        // Arrange
        int eventId = 9001;

        // Act
        HttpResponseMessage response = await client.PostAsync(
            $"OrganizationEvents/{eventId}/join",
            null,
            TestContext.Current.CancellationToken);

        // Cleanup
        await client.DeleteAsync(
            $"OrganizationEvents/{eventId}/join",
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Post_JoinEvent_WhenAlreadyRegistered_ReturnsConflict()
    {
        // Arrange
        int eventId = 9002;
        await client.PostAsync(
            $"OrganizationEvents/{eventId}/join",
            null,
            TestContext.Current.CancellationToken);

        // Act
        HttpResponseMessage response = await client.PostAsync(
            $"OrganizationEvents/{eventId}/join",
            null,
            TestContext.Current.CancellationToken);

        // Cleanup
        await client.DeleteAsync(
            $"OrganizationEvents/{eventId}/join",
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task Delete_LeaveEvent_ReturnsOk()
    {
        // Arrange
        int eventId = 9003;
        await client.PostAsync(
            $"OrganizationEvents/{eventId}/join",
            null,
            TestContext.Current.CancellationToken);

        // Act
        HttpResponseMessage response = await client.DeleteAsync(
            $"OrganizationEvents/{eventId}/join",
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Delete_LeaveEvent_WhenNotRegistered_ReturnsNotFound()
    {
        // Arrange
        int eventId = 9004;

        // Act
        HttpResponseMessage response = await client.DeleteAsync(
            $"OrganizationEvents/{eventId}/join",
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Get_IsRegistered_AfterJoining_ReturnsTrue()
    {
        // Arrange
        int eventId = 9005;
        await client.PostAsync(
            $"OrganizationEvents/{eventId}/join",
            null,
            TestContext.Current.CancellationToken);

        // Act
        HttpResponseMessage response = await client.GetAsync(
            $"OrganizationEvents/{eventId}/is-registered",
            TestContext.Current.CancellationToken);
        bool registered = await response.Content.ReadFromJsonAsync<bool>(
            cancellationToken: TestContext.Current.CancellationToken);

        // Cleanup
        await client.DeleteAsync(
            $"OrganizationEvents/{eventId}/join",
            TestContext.Current.CancellationToken);

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.True(registered);
    }
}
