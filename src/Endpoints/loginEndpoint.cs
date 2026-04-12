using backend.getdata;
using Credentials;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Models.User;
using System.ComponentModel;
using System.Security.Claims;
using System.Text;

namespace Endpoints;

public static class loginEndpoint
{
	public static RouteGroupBuilder MaploginEndpoint(this RouteGroupBuilder group)
	{
		group.MapPost("/register", async Task<IResult> (
					[DefaultValue("Bob")] string firstName,
					[DefaultValue("Bob@hotmail.com")] string email,
					[DefaultValue("12345password")] string password,
					[DefaultValue(25)] int age) =>
		{
			try
			{
				Users u = new Users
				{
					FirstName = firstName,
					Email = email,
					Age = age,
					PasswordHash = password,
				};
				DataUser ud = new DataUser();
				bool success = await ud.setUsers(u);
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

		group.MapPost("/login", async Task<IResult> ([FromBody] LoginCredentials loginCredentials) =>
		{
			try
			{
				DataUser ud = new DataUser();
				Users? u = await ud.getUserByEmail(loginCredentials.Email);
				if (u == null) return Results.BadRequest();
				bool correctPassword = await ud.loginUsers(loginCredentials.Email, loginCredentials.Password);
				return correctPassword ? Results.Ok(CreateToken(u)) : Results.BadRequest();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return Results.BadRequest(ex.Message);
			}
		})
			.WithName("Login");

		return group;
	}

	static private string CreateToken(Users user)
	{
		IConfigurationRoot config = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json")
			.Build();

		string secretKey = config["Jwt:Secret"]!;
		SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

		var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

		var tokenDiscriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity([
					new Claim(JwtRegisteredClaimNames.Sub, user.Id),
					new Claim(JwtRegisteredClaimNames.Email, user.Email!),
			]),
			Expires = DateTime.UtcNow.AddMinutes(60),
			SigningCredentials = credentials,
			Issuer = config["Jwt:Issuer"],
			Audience = config["Jwt:Audience"],
		};

		var handler = new JsonWebTokenHandler();

		string token = handler.CreateToken(tokenDiscriptor);

		return token;
	}
}