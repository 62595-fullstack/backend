using System.Net;
using Microsoft.EntityFrameworkCore;
using Models.User;
using Models.Organization;
using Newtonsoft.Json;
using backend.getdata;
using Models.UserOrganizationBinding;
using Models.OrganizationEvent;
using Endpoint.PostEndpoint;
using Endpoint.OrganizationEndpoint;

DummyData.Initialize();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	// SwaggerUI can be viewed at http://localhost:{port}
	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
		options.RoutePrefix = string.Empty;
	});
}

app.UseHttpsRedirection();

app.RegisterPostEndpoints();
app.RegisterOrganizationEndpoints();

#region UserOrganizationBinding

app.MapGet("/UserOrganizationBinding/{organizationId}", async Task<string> (int organizationId) =>
{
	try
	{
		using (DatabaseContext db = new DatabaseContext())
		{
			DataUserOrganizationBinding organizationData = new DataUserOrganizationBinding();
			List<UserOrganizationBindings> allOrganizations = await organizationData.getUserOrganizationForOrganization(organizationId);
			return JsonConvert.SerializeObject(allOrganizations);
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine(ex.Message);
		return "{}";
	}
})
.WithName("getUserOrganizationBinding");

#endregion


#region OrganizationEvents

app.MapGet("/OrganizationEvents/{organizationId}", async Task<string> (int organizationId) =>
{
	try
	{
		using (DatabaseContext db = new DatabaseContext())
		{
			DataOrganizationEvents organizationData = new DataOrganizationEvents();
			List<OrganizationEvents> allOrganizations = await organizationData.getOrganizationEvents(organizationId);
			return JsonConvert.SerializeObject(allOrganizations);
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine(ex.Message);
		return "{}";
	}
})
.WithName("getOrganizationEvents");

app.MapPost("/OrganizationEvents", async Task<string> (string organizationEvent) =>
{
	try
	{
		using (DatabaseContext db = new DatabaseContext())
		{
			OrganizationEvents? oe = JsonConvert.DeserializeObject<OrganizationEvents>(organizationEvent);
			db.OrganizationEvent.Add(oe);
			return HttpStatusCode.OK.ToString();
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine(ex.Message);
		return HttpStatusCode.InternalServerError.ToString();
	}
})
.WithName("PostOrganizationEvents");

#endregion


#region GDPR

app.MapDelete("/GDPR/{userId}", async Task<string> (int userId) =>
{
	try
	{
		using (DatabaseContext db = new DatabaseContext())
		{
			DataGDPR organizationData = new DataGDPR();
			int? allOrganizations = await organizationData.DeleteUserAcount(userId);
			return JsonConvert.SerializeObject(allOrganizations);
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine(ex.Message);
		return "{}";
	}
})
.WithName("DeleteGDPR");

#endregion




app.Run();
