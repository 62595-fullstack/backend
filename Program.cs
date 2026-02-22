using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Models.Post;

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
