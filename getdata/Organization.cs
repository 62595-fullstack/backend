using Microsoft.EntityFrameworkCore;
using Models.Organization;

namespace backend.getdata
{
    public class DataOrganization
    {
        public async Task<Organizations> CreateOrganization(Organizations organizations)
        {
            try
            {
                DatabaseContext db = new DatabaseContext();
                
                await db.AddAsync(organizations);

                await db.SaveChangesAsync();


                return await db.Organization.Where(o => o.Id == organizations.Id).FirstAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return organizations;
            }
        }


        public async Task<Organizations?> GetOrganizationByName(string OrganizationName)
        {
            try
            {
                DatabaseContext db = new DatabaseContext();
                
                await db.AddAsync(OrganizationName);

                await db.SaveChangesAsync();
                
                return await db.Organization.TakeLast(1).FirstAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
