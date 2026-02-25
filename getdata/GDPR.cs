

using Microsoft.EntityFrameworkCore;
using Models.OrganizationEvent;
using Models.User;

namespace backend.getdata
{
    public class DataGDPR
    {
        public async Task<Users?> DeleteUserAcount(int userId)
        {
            try
            {
                DatabaseContext db = new DatabaseContext();
                
                var bob = await db.User.Where(x => x.Id == userId).ExecuteDeleteAsync();


                return bob;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<OrganizationEvents>();
            }
        }

    }
}