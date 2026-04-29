using backend.getdata;
using Dto;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Models.User;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace Endpoints;

public static class loginEndpoint
{
	public static RouteGroupBuilder MapLoginEndpoint(this RouteGroupBuilder group)
	{
		group.MapPost("/register", async Task<IResult> (RegisterCredentialsDto registerDto) =>
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
				DataUser ud = new DataUser();
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

		group.MapPost("/login", async Task<IResult> (LoginCredentialsDto loginCredentials) =>
		{
			try
			{
				DataUser ud = new DataUser();
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

				return Results.Ok(CreateToken(u));
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

	static private string CreateToken(Users user)
	{
		IConfigurationRoot config = new ConfigurationBuilder()
		//AddedJsonFile("appsettings.json") because I could not login without it, idk.
			.AddJsonFile("appsettings.json")
			.AddUserSecrets(Assembly.GetExecutingAssembly())
			.AddEnvironmentVariables()
			.Build();

		string secretKey = config["Jwt:Secret"]!;
		SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

		var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

		string host = config["host"] ?? "localhost";
		string port = int.TryParse(config["programPort"], out int configuredPort)
			? configuredPort.ToString()
			: "5000";

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity([
					new Claim(JwtRegisteredClaimNames.Sub, user.Id),
					new Claim(JwtRegisteredClaimNames.Email, user.Email!),
			]),
			Expires = DateTime.UtcNow.AddMinutes(60),
			SigningCredentials = credentials,
			Issuer = "http://" + host + ":" + port,
			Audience = config["Jwt:Audience"],
		};

		var handler = new JsonWebTokenHandler();

		string token = handler.CreateToken(tokenDescriptor);

		return token;
	}
}
