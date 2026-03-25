using Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
		policy.WithOrigins("http://localhost:3000")
			  .AllowAnyHeader()
			  .AllowAnyMethod();
	});
});

builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
			{
				IConfigurationRoot config = new ConfigurationBuilder()
					.AddJsonFile("appsettings.json")
					.Build();
				options.RequireHttpsMetadata = false;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Secret"]!)),
					ValidIssuer = config["Jwt:Issuers"],
					ValidAudience = config["Jwt:Audience"],
					ClockSkew = TimeSpan.Zero,
					ValidIssuers = [
					"http://localhost:5000"
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
	DummyData.Initialize();
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

app.MapGroup("/posts").MapPostEndpoints();
app.MapGroup("/attachments").MapAttachmentEndpoints();
app.MapGroup("/organizations").MapOrganizationEndpoints();
app.MapGroup("/UserOrganizationBinding").MapUserOrganizationBindingEndpoints();
app.MapGroup("/OrganizationEvents").MapOrganizationEventsEndpoints();
app.MapGroup("/GDPR").MapGDPREndpoints();
app.MapGroup("/login").MaploginEndpoint();
app.Run();