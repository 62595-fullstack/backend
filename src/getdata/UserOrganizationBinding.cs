
using Dto;
using Microsoft.EntityFrameworkCore;
using Models.Organization;
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
				return await db.UserOrganizationBinding.Where(x => x.UserId == userId).ToListAsync();
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
					.FirstOrDefaultAsync(x => x.UserId == userId && x.OrganizationId == organizationId);
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
					.FirstOrDefaultAsync(x => x.UserId == userId && x.OrganizationId == organizationId);
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

		public async Task<bool> setUserToOrganization(string userId, int organizationId, int roleId)
		{
			DatabaseContext db = new DatabaseContext();
			UserOrganizationBindings uob = new UserOrganizationBindings();

			uob.OrganizationId = organizationId;
			uob.UserId = userId;
			uob.RoleId = roleId;

			await db.UserOrganizationBinding.AddAsync(uob);
			await db.SaveChangesAsync();

			Users? joiner = await db.User.FirstOrDefaultAsync(u => u.Id == userId);
			Organizations? org = await db.Organization.FindAsync(organizationId);
			List<string> adminUserIds = await db.UserOrganizationBinding
				.Where(b => b.OrganizationId == organizationId && b.RoleId == 1000 && b.UserId != null)
				.Select(b => b.UserId!)
				.ToListAsync();

			if (joiner != null && org != null && adminUserIds.Count > 0)
			{
				string joinerName = $"{joiner.FirstName} {joiner.LastName}";
				DataNotification notificationData = new();
				foreach (string adminUserId in adminUserIds)
				{
					if (adminUserId == userId) continue;
					NotificationDto notification = await notificationData.Create(
						userId: adminUserId,
						type: "organization_join",
						message: $"{joinerName} joined your organization '{org.Name}'.",
						actorUserId: userId
					);
					NotificationStream.Publish(adminUserId, notification);
				}
			}

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

				List<string> userIds = bindings.Select(b => b.UserId!).Distinct().ToList();
				List<Users> users = await db.User.Where(u => userIds.Contains(u.Id)).ToListAsync();
				Dictionary<string, Users> userMap = users.ToDictionary(u => u.Id);

				return bindings
					.Select(b =>
					{
						string uid = b.UserId!;
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

		public async Task<bool> updateUserRoleInOrganization(string userId, int organizationId, int roleId)
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