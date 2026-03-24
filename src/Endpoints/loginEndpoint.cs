using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.getdata;
using Microsoft.IdentityModel.Tokens;
using Models.User;
using Newtonsoft.Json;

namespace Endpoints;

public static class loginEndpoint
{
	public static RouteGroupBuilder MaploginEndpoint(this RouteGroupBuilder group)
	{
		group.MapPost("/{email}", async Task<IResult> (
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

		group.MapGet("/{email}", async Task<IResult> (
					[DefaultValue("Bob@hotmail.com")] string email,
					[DefaultValue("12345password")] string password) =>
			{
				try
				{
					DataUser ud = new DataUser();
					Users? u = await ud.getUserByEmail(email);
					if (u == null) return Results.BadRequest();
					bool correctPassword = await ud.loginUsers(email, password);
					return correctPassword ? Results.Ok(GenerateToken(u)) : Results.BadRequest();
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

	// Source: https://stackoverflow.com/questions/74441535/creating-and-validating-jwt-tokens-in-c-sharp-net
	static private string GenerateToken(Users user)
	{
		IConfigurationRoot config = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json")
			.Build();

		var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
		var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

		var claims = new[]
		{
		       new Claim(JwtRegisteredClaimNames.Sub, user.Email),
		       new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
		       // new Claim(ClaimTypes.Role, user.UserType),
		};

		var token = new JwtSecurityToken(
		    issuer: config["Jwt:Issuer"],
		    audience: config["Jwt:Audience"],
		    claims: claims,
		    expires: DateTime.Now.AddMinutes(30),
		    signingCredentials: credentials
		);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}
