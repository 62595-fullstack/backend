using backend.getdata;
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
				int? nrRowsDeleted = await organizationData.DeleteUserAccountByEmail(email);
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