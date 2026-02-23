using System.Text.Json;
using backend.getdata;
using Microsoft.EntityFrameworkCore;
using Models.Organization;
using Models.Post;

DummyData.Initialize();

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();


#region  posts
app.MapGet("/posts", string () =>
{
	try
	{
		using (DatabaseContext db = new DatabaseContext())
		{
			Task<List<Posts>> posts = db.Post.ToListAsync();
			return JsonSerializer.Serialize(posts);
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
			return JsonSerializer.Serialize(posts);
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
				return JsonSerializer.Serialize(allOrganizations);
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
				return JsonSerializer.Serialize(allOrganizations);
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
				return JsonSerializer.Serialize(allOrganizations);
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
