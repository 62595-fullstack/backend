

using Microsoft.EntityFrameworkCore;


namespace backend.getdata
{
	public class DataGDPR
	{
		public async Task<int?> DeleteUserAcount(string userId)
		{
			try
			{
				DatabaseContext db = new DatabaseContext();

				var user = await db.User.Where(x => x.Id == userId).ExecuteDeleteAsync();

				if (user == 0)
				{
					return 0;
				}

				await db.SaveChangesAsync();
				return 1;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}
		public async Task<int?> DeleteUserAcountByEmail(string email)
		{
			try
			{
				DatabaseContext db = new DatabaseContext();

				var user = await db.User.Where(x => x.Email == email).ExecuteDeleteAsync();

				if (user == 0)
				{
					return 0;
				}
				else if (user == 1)
				{
					await db.SaveChangesAsync();
					return 1;
				}
				else
				{
					// Don't save changes since email is unqiue, only one user 
					// should be able to be deleted at most
					return user;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}

	}
}