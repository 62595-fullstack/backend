

using Microsoft.EntityFrameworkCore;
using Models.OrganizationEvent;
using Models.User;

namespace backend.getdata
{
    public class DataGDPR
    {
        public async Task<int?> DeleteUserAcount(int userId)
        {
            try
            {
                DatabaseContext db = new DatabaseContext();
                
                var user = await db.User.Where(x => x.Id == userId).ExecuteDeleteAsync();


                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

    }
}