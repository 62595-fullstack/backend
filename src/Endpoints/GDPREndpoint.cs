using backend.getdata;
using Newtonsoft.Json;

namespace Endpoints;

public static class GDPREndpoint
{
	public static RouteGroupBuilder MapGDPREndpoints(this RouteGroupBuilder group)
	{
		group.MapDelete("/{userId}", async Task<IResult> (int userId) =>
		{
			try
			{
				DataGDPR organizationData = new DataGDPR();
				int? allOrganizations = await organizationData.DeleteUserAcount(userId);
				string allOrganizationsJson = JsonConvert.SerializeObject(allOrganizations);
				return Results.Ok(allOrganizationsJson);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return Results.BadRequest();
			}
		})
		.WithName("DeleteGDPR");

		return group;
	}
}