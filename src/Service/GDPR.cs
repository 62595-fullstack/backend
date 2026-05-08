using Microsoft.EntityFrameworkCore;

namespace Services;

public sealed class DataGDPR(DatabaseContext db)
{
	private readonly DatabaseContext db = db;

	public async Task<int?> DeleteUserAccount(string userId)
	{
		try
		{
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
	public async Task<int?> DeleteUserAccountByEmail(string email)
	{
		try
		{
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
				// Don't save changes since email is unique, only one user 
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