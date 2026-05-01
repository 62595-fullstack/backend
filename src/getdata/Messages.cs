
using Microsoft.EntityFrameworkCore;

namespace backend.getdata
{
	public class DataMessages
	{
		public async Task<string?> getMessages(string userId)
		{
			try
			{
				DatabaseContext db = new DatabaseContext();

                // var messages = db.Message.Where(m )

				return "HEJ Emil";
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}
	}
}