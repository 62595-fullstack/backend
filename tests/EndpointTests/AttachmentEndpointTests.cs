using Dto;
using System.Net.Http.Json;

namespace tests.EndpointsTests;

[Collection("httpclient collection")]
public class AttachmentEndpointTests(HttpClientFixture httpClientFixture)
{
	private readonly HttpClient client = httpClientFixture.client;

	[Fact]
	public async Task Get_Attachments_ReturnAttachments()
	{
		// Act
		int attachmentId = 1000;
		HttpResponseMessage response = await client.GetAsync(
				$"attachments/{attachmentId}",
				TestContext.Current.CancellationToken);
		string? content = await response.Content.ReadAsStringAsync(
				TestContext.Current.CancellationToken);
		// List<Organizations>? organizations = JsonConvert.DeserializeObject<List<Organizations>>(content);
		// Assert
		Assert.True(response.IsSuccessStatusCode);
		Assert.NotNull(content);
		// Assert.NotNull(organizations);
	}

	[Theory]
	[InlineData("coverphoto")]
	[InlineData("profilepicture")]
	public async Task Post_CoverPhoto_ReturnSuccess(string endpoint)
	{
		// Arrange
		string fileName = "AttachmentEndpointTests";
		string fileType = ".cs";
		byte[]? bytes = await File.ReadAllBytesAsync($"../../../EndpointTests/{fileName}{fileType}", TestContext.Current.CancellationToken);
		AttachmentDto attachment = new AttachmentDto
		(
				Content: bytes,
				FileName: fileName,
				FileType: fileType
		);
		Assert.NotNull(attachment);
		int organizationId = 1000;
		// Act
		HttpResponseMessage response = await client.PostAsJsonAsync(
				$"attachments/{endpoint}/{organizationId}",
				attachment,
				TestContext.Current.CancellationToken);
		string? content = await response.Content.ReadAsStringAsync(
				TestContext.Current.CancellationToken);
		// Assert
		Console.WriteLine("response.IsSuccessStatusCode: " + response.StatusCode.ToString());
		Assert.True(response.IsSuccessStatusCode);
		Console.WriteLine("content: " + content.ToString());
		Assert.NotNull(content);
	}

}