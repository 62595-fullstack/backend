using backend.getdata;
using Models.Organization;
using Models.UserOrganizationBinding;
using Newtonsoft.Json;
using System.Net;

namespace Endpoint.OrganizationEndpoint;

public static class UserOrganizationBinding
{
	public static RouteGroupBuilder MapUserOrganizationBindingEndpoints(this RouteGroupBuilder group)
	{
		group.MapGet("/{organizationId}", async Task<string> (int organizationId) =>
		{
			try
			{
				using (DatabaseContext db = new DatabaseContext())
				{
					DataUserOrganizationBinding organizationData = new DataUserOrganizationBinding();
					List<UserOrganizationBindings> allOrganizations = await organizationData.getUserOrganizationForOrganization(organizationId);
					return JsonConvert.SerializeObject(allOrganizations);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return "{}";
			}
		})
		.WithName("getUserOrganizationBinding");

		return group;
	}
}
