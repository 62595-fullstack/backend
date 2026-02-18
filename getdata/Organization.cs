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
                
                return await db.Organization.Where(o=> o.Name == OrganizationName).FirstAsync();;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<Organizations>?> GetAllOrganization()
        {
            try
            {
                DatabaseContext db = new DatabaseContext();
                
                return await db.Organization.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
