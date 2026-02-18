using System.Text.Json;
using Microsoft.EntityFrameworkCore;

DummyData.Initialize();

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/posts", (x) =>
{
	try
	{
		using (DatabaseContext db = new DatabaseContext())
		{
			var posts = db.Post.ToListAsync();
			string jsonString = JsonSerializer.Serialize(posts);
			return posts;
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine(ex.Message);
		return null;
	}
})
.WithName("GetPosts");

app.Run();
