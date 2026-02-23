

using Microsoft.EntityFrameworkCore;
using Models.User;

namespace backend.getdata
{
    public class DataUserOrganizationBinding
    {
        public async Task<List<Models.UserOrganizationBinding.UserOrganizationBindings>> getUserOrganizationForOrganization(int organizationId)
        {
            try
            {
                DatabaseContext db = new DatabaseContext();
                return await db.UserOrganizationBinding.Where(x => x.OrganizationId == organizationId).ToListAsync();;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Models.UserOrganizationBinding.UserOrganizationBindings>();
            }
        }

    }
}