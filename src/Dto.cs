namespace Dto;

public record LoginCredentialsDto(string Email, string Password);
public record RegisterCredentialsDto(string Email, string Password, string FirstName, int Age);
public record PostDto(string Title, string BodyText, int UserId, int OrganizationEventId);