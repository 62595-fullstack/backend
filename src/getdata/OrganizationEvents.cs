


using Microsoft.EntityFrameworkCore;
using Models.OrganizationEvent;
using Models.UserEventBinding;

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
                Console.WriteLine(ex.ToString());
                return new List<OrganizationEvents>();
            }
        }

        public async Task createOrganizationEvents(OrganizationEvents organizationEvents)
        {
            DatabaseContext db = new DatabaseContext();
            await db.OrganizationEvent.AddAsync(organizationEvents);
            await db.SaveChangesAsync();
        }



        public async Task<bool> userJoinEvent(int userId, int organizationId)
        {
            try
            {
                DatabaseContext db = new DatabaseContext();              
                UserEventBindings ueb = new UserEventBindings
                {
                  UserId = userId,
                  OrganizationEventsId = organizationId
                };

                await db.UserEventBinding.AddAsync(ueb);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }



    }
}
