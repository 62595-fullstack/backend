using backend.getdata;
using Dto;
using Models.User;

namespace Endpoints;

public static class loginEndpoint
{
	public static RouteGroupBuilder MapLoginEndpoint(this RouteGroupBuilder group)
	{
		group.MapPost("/register", async Task<IResult> (RegisterCredentialsDto registerDto, DataUser ud) =>
		{
			if (string.IsNullOrWhiteSpace(registerDto.Email))
				return Results.BadRequest("Email is required.");
			if (string.IsNullOrWhiteSpace(registerDto.Password))
				return Results.BadRequest("Password is required.");
			if (string.IsNullOrWhiteSpace(registerDto.FirstName))
				return Results.BadRequest("First name is required.");
			if (string.IsNullOrWhiteSpace(registerDto.LastName))
				return Results.BadRequest("Last name is required.");

			try
			{
				bool success = await ud.AddUsers(registerDto);
				return success ?
					Results.Ok() :
					Results.BadRequest("Failed to register user");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return Results.BadRequest(ex.Message);
			}
		})
		.WithName("CreateUser");

		group.MapPost("/login", async Task<IResult> (LoginCredentialsDto loginCredentials, TokenService tokenService, DataUser ud) =>
		{
			try
			{
				Users? u = await ud.getUserByEmail(loginCredentials.Email);
				if (u == null)
				{
					Console.WriteLine($"[login] user not found: {loginCredentials.Email}");
					return Results.Unauthorized();
				}
				bool correctPassword = await ud.loginUsers(loginCredentials.Email, loginCredentials.Password);
				if (!correctPassword)
				{
					Console.WriteLine($"[login] bad password for: {loginCredentials.Email}");
					return Results.Unauthorized();
				}

				return Results.Ok(tokenService.CreateToken(u));
			}
			catch (Exception ex)
			{
				Console.WriteLine($"[login] {ex}");
				return Results.Problem(ex.Message);
			}
		})
			.WithName("Login");

		return group;
	}
}