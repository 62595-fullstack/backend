using Dto;
using Microsoft.EntityFrameworkCore;
using Models.User;
using Models.UserOrganizationBinding;

namespace Services;

public class DataUserOrganizationBinding(DatabaseContext db)
{
	private readonly DatabaseContext db = db;

	public async Task<List<UserOrganizationBindings>> getUserOrganizationForOrganization(int organizationId)
	{
		try
		{
			return await db.UserOrganizationBinding.Where(x => x.OrganizationId == organizationId).ToListAsync();
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			return new List<UserOrganizationBindings>();
		}
	}

	public async Task<List<UserOrganizationBindings>> getAllUserOrganizationBindingsForUser(string userId)
	{
		try
		{
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
		UserOrganizationBindings uob = new UserOrganizationBindings
		{
			OrganizationId = organizationId,
			UserId = userId,
			RoleId = roleId
		};

		await db.UserOrganizationBinding.AddAsync(uob);
		await db.SaveChangesAsync();

		return true;
	}

	public async Task<List<OrgMemberDto>> getOrganizationMembersWithDetails(int organizationId)
	{
		try
		{
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