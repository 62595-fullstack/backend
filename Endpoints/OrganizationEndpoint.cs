using backend.getdata;
using Models.Organization;
using Newtonsoft.Json;
using System.Net;

namespace Endpoints;

public static class OrganizationEndpoint
{
	public static RouteGroupBuilder MapOrganizationEndpoints(this RouteGroupBuilder group)
	{
		group.MapGet("/", async Task<string> () =>
		{
			try
			{
				using (DatabaseContext db = new DatabaseContext())
				{
					DataOrganization organizationData = new DataOrganization();
					List<Organizations>? allOrganizations = await organizationData.GetAllOrganization();
					return JsonConvert.SerializeObject(allOrganizations);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return "{}";
			}
		})
		.WithName("GetOrganizations");

		group.MapPost("/", async Task<string> (string organizations) =>
		{
			try
			{
				using (DatabaseContext db = new DatabaseContext())
				{
					Organizations? o = JsonConvert.DeserializeObject<Organizations>(organizations);

					if (o != null)
					{
						db.Organization.Add(o);
					}
					else
					{
						return HttpStatusCode.InternalServerError.ToString();
					}

					return HttpStatusCode.OK.ToString();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return HttpStatusCode.InternalServerError.ToString();
			}
		})
		.WithName("PostOrganizations");

		group.MapGet("/{id}", async Task<string> (int id) =>
		{
			try
			{
				using (DatabaseContext db = new DatabaseContext())
				{

					DataOrganization organizationData = new DataOrganization();
					Organizations? allOrganizations = await organizationData.GetOrganizationById(id);
					return JsonConvert.SerializeObject(allOrganizations);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return "{}";
			}
		})
		.WithName("GetOrganizationsById");

		group.MapDelete("/{id}", async Task<string> (int id) =>
		{
			try
			{
				using (DatabaseContext db = new DatabaseContext())
				{
					DataOrganization organizationData = new DataOrganization();
					bool allOrganizations = await organizationData.DeleteOrganization(id);
					return JsonConvert.SerializeObject(allOrganizations);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return "{}";
			}
		})
		.WithName("DeleteOrganizationsById");

		return group;
	}
}
