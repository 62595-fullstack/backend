namespace Dto;

public record LoginCredentialsDto(string Email, string Password);
public record RegisterCredentialsDto(string Email, string Password, string FirstName, int Age);
public record PostDto(string Title, string BodyText, int UserId, int OrganizationEventId);
public record AttachmentDto(string FileName, string FileType, byte[] Content);
public record UserSummaryDto(string Id, string Email, string FirstName, string UserName, int Age);
public record FriendSummaryDto(string Id, string Email, string FirstName, string UserName, int Age, DateTime FriendsSince);
public record AddFriendDto(string FriendUserId);