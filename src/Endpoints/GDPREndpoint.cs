using backend.getdata;
using Newtonsoft.Json;

namespace Endpoints;

public static class GDPREndpoint
{
	public static RouteGroupBuilder MapGDPREndpoints(this RouteGroupBuilder group)
	{
		group.MapDelete("/{email}", async Task<IResult> (string email) =>
		{
			try
			{
				DataGDPR organizationData = new DataGDPR();
				int? nrRowsDeleted = await organizationData.DeleteUserAcountByEmail(email);
				return Results.Ok(nrRowsDeleted);
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