using Microsoft.EntityFrameworkCore;
using Models.Post;
using Models.Organization;
using Newtonsoft.Json;

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


#region  posts
app.MapGet("/posts", string () =>
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

app.MapGet("/posts/{organizationsId}", string (int organizationsId) =>
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
app.MapGet("/organizations", async Task<string> () =>
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

app.MapGet("/organizations/{id}", async Task<string> (int id) =>
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

app.MapDelete("/organizations/{id}", async Task<string> (int id) =>
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















app.Run();
