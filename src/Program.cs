using backend.getdata;
using Endpoints;
using Models.User;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
    ContractResolver = new CamelCasePropertyNamesContractResolver()
};

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
	Args = args,
	WebRootPath = "../wwwroot"
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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

var app = builder.Build();

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

app.UseStaticFiles();
app.UseCors();

if (!app.Environment.IsDevelopment())
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

