

using Microsoft.EntityFrameworkCore;
using Models.OrganizationEvent;
using Models.User;

namespace backend.getdata
{
    public class DataOrganizationEvents
    {
        public async Task<List<OrganizationEvents>> getOrganizationEvents(int organizationId)
        {
            try
            {
                DatabaseContext db = new DatabaseContext();
                


                return await db.OrganizationEvent.Where(x => x.OrganizationId == organizationId).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<OrganizationEvents>();
            }
        }

    }
}