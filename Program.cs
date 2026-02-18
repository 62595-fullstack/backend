using System.Text.Json;
using Microsoft.EntityFrameworkCore;

DummyData.Initialize();

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/posts", () =>
{
	using (var db = new DatabaseContext())
	{
		var posts = db.Post.ToListAsync();
		string jsonString = JsonSerializer.Serialize(posts);
	}
})
.WithName("GetPosts");

app.Run();
