using Dto;
using Microsoft.EntityFrameworkCore;
using Models.Post;

namespace Services;

public class DataPost(DatabaseContext db)
{
	private readonly DatabaseContext db = db;

	public async Task<List<Posts>?> getAllPost()
	{
		try
		{
			Task<List<Posts>> posts = db.Post.ToListAsync();

			return await posts;
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			return null;
		}
	}

	public async Task<List<Posts>> GetPostsByUser(string userId)
	{
		return await db.Post
			.AsNoTracking()
			.Where(p => p.UserId == userId)
			.OrderByDescending(p => p.CreatedDate)
			.ToListAsync();
	}

	public async Task<List<Posts>?> getPostByOrganization(int id)
	{
		try
		{
			Task<List<Posts>> posts = db.Post.Include(p => p.OrganizationEvent).Where(p => p.OrganizationEvent != null && p.OrganizationEvent.OrganizationId == id).ToListAsync();

			return await posts;
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			return null;
		}
	}

	public async Task<bool> getPostByOrganization(Posts post)
	{
		try
		{
			await db.Post.AddAsync(post);
			await db.SaveChangesAsync();

			return true;
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			return false;
		}
	}

	public async Task<bool> AddPost(PostDto post)
	{
		try
		{
			await db.Post.AddAsync(new Posts
			{
				Title = post.Title,
				BodyText = post.BodyText,
				UserId = post.UserId,
				OrganizationEventId = post.OrganizationEventId,
			});
			return true;
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			return false;
		}
	}
}