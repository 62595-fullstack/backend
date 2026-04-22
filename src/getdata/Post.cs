using Dto;
using Microsoft.EntityFrameworkCore;
using Models.Post;

namespace backend.getdata
{
	public class Post
	{
		public async Task<List<Posts>?> getAllPost()
		{
			try
			{
				DatabaseContext db = new DatabaseContext();

				Task<List<Posts>> posts = db.Post.ToListAsync();


				return await posts;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}

		public async Task<List<Posts>?> getPostByOrganization(int id)
		{
			try
			{
				DatabaseContext db = new DatabaseContext();

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
				DatabaseContext db = new DatabaseContext();

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
				DatabaseContext db = new DatabaseContext();

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
}