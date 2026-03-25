using backend.getdata;
using Newtonsoft.Json;

namespace Endpoints;

public static class GDPREndpoint
{
	public static RouteGroupBuilder MapGDPREndpoints(this RouteGroupBuilder group)
	{
		group.MapDelete("/{userId}", async Task<string> (int userId) =>
		{
			try
			{
				using (DatabaseContext db = new DatabaseContext())
				{
					DataGDPR organizationData = new DataGDPR();
					int? allOrganizations = await organizationData.DeleteUserAcount(userId);
					return JsonConvert.SerializeObject(allOrganizations);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return "{}";
			}
		})
		.WithName("DeleteGDPR");

		return group;
	}
}