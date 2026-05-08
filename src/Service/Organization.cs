using Microsoft.EntityFrameworkCore;
using Models.Organization;

namespace Services;

public class DataOrganization(DatabaseContext db)
{
	public async Task<Organizations> CreateOrganization(Organizations organizations)
	{
		try
		{
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