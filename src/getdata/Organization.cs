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
				DatabaseContext db = new();

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
				DatabaseContext db = new();

				return await db.Organization.Where(o => o.Name == OrganizationName).FirstAsync();
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
				DatabaseContext db = new();

				return await db.Organization.ToListAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}

		public async Task<Organizations?> GetOrganizationById(int OrganizationId)
		{
			try
			{
				DatabaseContext db = new();

				return await db.Organization.Where(o => o.Id == OrganizationId).FirstAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}

		public async Task<Organizations?> UpdateDescription(int id, string? description)
		{
			try
			{
				DatabaseContext db = new();
				Organizations? org = await db.Organization.FindAsync(id);
				if (org == null) return null;
				org.Description = description ?? "";
				await db.SaveChangesAsync();
				return org;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}

		public async Task<bool> DeleteOrganization(int id)
		{
			try
			{
				DatabaseContext db = new();

				Organizations organizations = await db.Organization.Where(o => o.Id == id).FirstAsync();
				db.Organization.Remove(organizations);

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