using Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using System.Text;

JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
	ContractResolver = new CamelCasePropertyNamesContractResolver()
};

WebApplicationBuilder builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
	Args = args,
	WebRootPath = "../wwwroot"
});

IConfigurationRoot config = new ConfigurationBuilder()
					.AddJsonFile("appsettings.json")
					.AddEnvironmentVariables()
					.AddUserSecrets(Assembly.GetExecutingAssembly())
					.Build();

string programPort = int.TryParse(config["programPort"], out int configuredPort)
	? configuredPort.ToString()
	: "5000";
string host = config["host"] ?? "localhost";

builder.WebHost.UseUrls($"http://localhost:{programPort}");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
	{
		Type = SecuritySchemeType.Http,
		Scheme = "bearer",
		BearerFormat = "JWT",
		Description = "JWT Authorization header using the Bearer scheme."
	});
	options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
	{
		[new OpenApiSecuritySchemeReference("bearer", document)] = []
	});
});
builder.Services.ConfigureHttpJsonOptions(options =>
{
	options.SerializerOptions.PropertyNameCaseInsensitive = true;
});
builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(policy =>
	{
		policy.WithOrigins($"http://{host}:3000")
			  .AllowAnyHeader()
			  .AllowAnyMethod();
	});
});

builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
			{
				options.RequireHttpsMetadata = false;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Secret"]!)),
					ValidIssuer = $"http://{host}:{programPort}",
					ValidAudience = config["Jwt:Audience"],
					ClockSkew = TimeSpan.Zero,
					ValidIssuers = [
					$"http://{host}:{programPort}"
					],
				};
			});
builder.Services.AddAuthorization();

WebApplication app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	using (DatabaseContext db = new())
	{
		await db.Database.MigrateAsync();
	}

	await DummyData.Initialize();
	app.UseSwagger();
	// SwaggerUI can be viewed at http://localhost:{port}
	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
		options.RoutePrefix = string.Empty;
	});
}
else
{
	app.UseHttpsRedirection();
}

app.MapGroup("/posts")
	.RequireAuthorization()
	.MapPostEndpoints();
app.MapGroup("/attachments")
	.RequireAuthorization()
	.MapAttachmentEndpoints();
app.MapGroup("/organizations")
	.RequireAuthorization()
	.MapOrganizationEndpoints();
app.MapGroup("/UserOrganizationBinding")
	.RequireAuthorization()
	.MapUserOrganizationBindingEndpoints();
app.MapGroup("/OrganizationEvents")
	.RequireAuthorization()
	.MapOrganizationEventsEndpoints();
app.MapGroup("/users")
	.RequireAuthorization()
	.MapUserEndpoints();
app.MapGroup("/GDPR")
	.RequireAuthorization()
	.MapGDPREndpoints();
app.MapGroup("").MapLoginEndpoint();
app.Run();
