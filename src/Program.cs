using backend.getdata;
using Endpoints;
using Models.User;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

DataUser du = new DataUser();

Users u = new Users
			{
				Email = "crazyfrog@hotmail.com",
				PasswordHash = "bingbing",
				FirstName = "Crazy",
				UserName = "Frog",
				Age = 2,
			};


await du.setUsers(u);





app.UseHttpsRedirection();

app.MapGroup("/posts").MapPostEndpoints();
app.MapGroup("/attachments").MapAttachmentEndpoints();
app.MapGroup("/organizations").MapOrganizationEndpoints();
app.MapGroup("/UserOrganizationBinding").MapUserOrganizationBindingEndpoints();
app.MapGroup("/OrganizationEvents").MapOrganizationEventsEndpoints();
app.MapGroup("/GDPR").MapGDPREndpoints();
app.MapGroup("/login").MaploginEndpoint();
app.Run();

