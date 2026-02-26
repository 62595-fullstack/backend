using Microsoft.EntityFrameworkCore;
using Models.Post;
using Models.Organization;
using Newtonsoft.Json;
using backend.getdata;
using Models.UserOrganizationBinding;
using Models.OrganizationEvent;

DummyData.Initialize();

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi("/api/swagger/v1/swagger.json");

	// SwaggerUI can be viewed at http://localhost:{port}/api/
	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("/api/swagger/v1/swagger.json", "v1");
		options.RoutePrefix = "api";
	});
}

app.UseHttpsRedirection();


#region  posts
app.MapGet("/api/posts", string () =>
{
	try
	{
		using (DatabaseContext db = new DatabaseContext())
		{
			Task<List<Posts>> posts = db.Post.ToListAsync();
			return JsonConvert.SerializeObject(posts);
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine(ex.Message);
		return "{}";
	}
})
.WithName("GetPosts");

app.MapGet("/api/posts/{organizationsId}", string (int organizationsId) =>
{
	try
	{
		using (DatabaseContext db = new DatabaseContext())
		{
			Task<List<Posts>> posts = db.Post.ToListAsync();
			return JsonConvert.SerializeObject(posts);
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine(ex.Message);
		return "{}";
	}
})
.WithName("GetPostsFromOrganizationsId");

#endregion



#region organizations
app.MapGet("/api/organizations", async Task<string> () =>
{
	try
	{
		using (DatabaseContext db = new DatabaseContext())
		{
			DataOrganization organizationData = new DataOrganization();
			List<Organizations>? allOrganizations = await organizationData.GetAllOrganization();
			return JsonConvert.SerializeObject(allOrganizations);
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine(ex.Message);
		return "{}";
	}
})
.WithName("Getorganizations");

app.MapGet("/api/organizations/{id}", async Task<string> (int id) =>
{
	try
	{
		using (DatabaseContext db = new DatabaseContext())
		{

			DataOrganization organizationData = new DataOrganization();
			Organizations? allOrganizations = await organizationData.GetOrganizationById(id);
			return JsonConvert.SerializeObject(allOrganizations);
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine(ex.Message);
		return "{}";
	}
})
.WithName("GetOrganizationsById");

app.MapDelete("/api/organizations/{id}", async Task<string> (int id) =>
{
	try
	{
		using (DatabaseContext db = new DatabaseContext())
		{

			DataOrganization organizationData = new DataOrganization();
			bool allOrganizations = await organizationData.DeleteOrganization(id);
			return JsonConvert.SerializeObject(allOrganizations);
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine(ex.Message);
		return "{}";
	}
})
.WithName("DeleteOrganizationsById");

#endregion


#region UserOrganizationBinding

app.MapGet("/api/UserOrganizationBinding/{organizationId}", async Task<string> (int organizationId) =>
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

app.MapGet("/api/OrganizationEvents/{organizationId}", async Task<string> (int organizationId) =>
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

#endregion



#region GDPR

app.MapDelete("/api/GDPR/{userId}", async Task<string> (int userId) =>
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
