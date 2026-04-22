namespace Dto;

public record LoginCredentialsDto(string Email, string Password);
public record RegisterCredentialsDto(string Email, string Password, string FirstName, string LastName, DateOnly DateOfBirth);
public record PostDto(string Title, string BodyText, string UserId, int OrganizationEventId);
public record AttachmentDto(string FileName, string FileType, byte[] Content);
public record UserSummaryDto(string Id, string Email, string FirstName, string LastName, string UserName, DateOnly DateOfBirth, string? Bio);
public record UpdateProfileDto(string? Bio);
public record UserSearchResultDto(string Id, string FirstName, string LastName);
public record FriendSummaryDto(string Id, string Email, string FirstName, string LastName, string UserName, DateOnly DateOfBirth, DateTime FriendsSince);
public record AddFriendDto(string FriendUserId);
public record UpdateEventRequest(string? Description, string? Rules);
public record UpdateOrganizationDto(string? Description);