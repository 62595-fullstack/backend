

using Microsoft.EntityFrameworkCore;


namespace backend.getdata
{
	public class DataGDPR
	{
		public async Task<int?> DeleteUserAcount(int userId)
		{
			try
			{
				DatabaseContext db = new DatabaseContext();

				var user = await db.User.Where(x => int.Parse(x.Id) == userId).ExecuteDeleteAsync();

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

	}
}