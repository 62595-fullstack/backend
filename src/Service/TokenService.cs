using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Models.User;
using System.Security.Claims;
using System.Text;

public sealed class TokenService(IConfiguration configuration)
{
	private readonly IConfiguration config = configuration;

	public string CreateToken(Users user)
	{
		string secretKey = config["Jwt:Secret"]!;
		SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(secretKey));

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