
using Dto;
using Microsoft.EntityFrameworkCore;
using Models.User;
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

		public async Task<List<UserOrganizationBindings>> getAllUserOrganizationBindingsForUser(string userId)
		{
			try
			{
				DatabaseContext db = new DatabaseContext();
				return await db.UserOrganizationBinding.Where(x => x.UserId == int.Parse(userId)).ToListAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return new List<UserOrganizationBindings>();
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

		public async Task<bool> removeUserFromOrganization(string userId, int organizationId)
		{
			try
			{
				DatabaseContext db = new DatabaseContext();
				UserOrganizationBindings? binding = await db.UserOrganizationBinding
					.FirstOrDefaultAsync(x => x.UserId == int.Parse(userId) && x.OrganizationId == organizationId);
				if (binding == null) return false;

				db.UserOrganizationBinding.Remove(binding);
				await db.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		public async Task<bool> setUserToOrganization(int userId, int organizationId, int roleId)
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

		public async Task<List<OrgMemberDto>> getOrganizationMembersWithDetails(int organizationId)
		{
			try
			{
				DatabaseContext db = new();
				List<UserOrganizationBindings> bindings = await db.UserOrganizationBinding
					.Where(b => b.OrganizationId == organizationId && b.UserId != null)
					.ToListAsync();

				List<string> userIds = bindings.Select(b => b.UserId!.Value.ToString()).Distinct().ToList();
				List<Users> users = await db.User.Where(u => userIds.Contains(u.Id)).ToListAsync();
				Dictionary<string, Users> userMap = users.ToDictionary(u => u.Id);

				return bindings
					.Select(b =>
					{
						string uid = b.UserId!.Value.ToString();
						if (!userMap.TryGetValue(uid, out Users? user)) return null;
						string roleName = b.RoleId == 1000 ? "Admin" : "Member";
						return new OrgMemberDto(b.Id, uid, user.FirstName, user.LastName, b.RoleId, roleName);
					})
					.Where(m => m != null)
					.Select(m => m!)
					.ToList();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return [];
			}
		}

		public async Task<bool> updateUserRoleInOrganization(int userId, int organizationId, int roleId)
		{
			try
			{
				DatabaseContext db = new();
				UserOrganizationBindings? binding = await db.UserOrganizationBinding
					.FirstOrDefaultAsync(b => b.UserId == userId && b.OrganizationId == organizationId);
				if (binding == null) return false;
				binding.RoleId = roleId;
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