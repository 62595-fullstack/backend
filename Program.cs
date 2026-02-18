using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Models.Post;

DummyData.Initialize();

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

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

app.Run();
