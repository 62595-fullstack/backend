using Endpoints;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
    ContractResolver = new CamelCasePropertyNamesContractResolver()
};

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
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

app.UseHttpsRedirection();

app.MapGroup("/posts").MapPostEndpoints();
app.MapGroup("/attachments").MapAttachmentEndpoints();
app.MapGroup("/organizations").MapOrganizationEndpoints();
app.MapGroup("/UserOrganizationBinding").MapUserOrganizationBindingEndpoints();
app.MapGroup("/OrganizationEvents").MapOrganizationEventsEndpoints();
app.MapGroup("/GDPR").MapGDPREndpoints();

app.Run();

