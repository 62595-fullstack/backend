

using Microsoft.EntityFrameworkCore;
using Models.UserOrganizationBinding;

namespace backend.getdata
{
	public class DataUserOrganizationBinding
	{
		public async Task<List<Models.UserOrganizationBinding.UserOrganizationBindings>> getUserOrganizationForOrganization(int organizationId)
		{
			try
			{
				DatabaseContext db = new DatabaseContext();
				return await db.UserOrganizationBinding.Where(x => x.OrganizationId == organizationId).ToListAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return new List<Models.UserOrganizationBinding.UserOrganizationBindings>();
			}
		}

		public async Task<UserOrganizationBindings?> getUserOrganizationBindingForUser(string userId, int organizationId)
		{
			try
			{
				DatabaseContext db = new DatabaseContext();
				return await db.UserOrganizationBinding
					.FirstOrDefaultAsync(x => x.UserId == int.Parse(userId) && x.OrganizationId == organizationId);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}

		public async Task<UserOrganizationBindings?> getUserOrganizationBindingById(int id)
		{
			try
			{
				DatabaseContext db = new DatabaseContext();
				return await db.UserOrganizationBinding.FindAsync(id);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}

		public async Task<bool> setUserToOrganization(int userId, int organizationId, int roleId)
		{
			try
			{
				DatabaseContext db = new DatabaseContext();
				UserOrganizationBindings uob = new UserOrganizationBindings();

				uob.OrganizationId = organizationId;
				uob.UserId = userId;
				uob.RoleId = roleId;

				await db.UserOrganizationBinding.AddAsync(uob);
				await db.SaveChangesAsync();

				return true;

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}
	}
}