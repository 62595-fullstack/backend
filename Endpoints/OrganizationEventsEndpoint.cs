using backend.getdata;
using Models.OrganizationEvent;
using Newtonsoft.Json;
using System.Net;

namespace Endpoints;

public static class OrganizationEventsEndpoint
{
	public static RouteGroupBuilder MapOrganizationEventsEndpoints(this RouteGroupBuilder group)
	{
		group.MapGet("/{organizationId}", async Task<string> (int organizationId) =>
		{
			try
			{
				using (DatabaseContext db = new DatabaseContext())
				{
					DataOrganizationEvents organizationData = new DataOrganizationEvents();
					List<OrganizationEvents> allOrganizations = await organizationData.getOrganizationEvents(organizationId);
					return JsonConvert.SerializeObject(allOrganizations);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return "{}";
			}
		})
		.WithName("getOrganizationEvents");

		group.MapPost("/", async Task<string> (string organizationEvent) =>
		{
			try
			{
				using (DatabaseContext db = new DatabaseContext())
				{
					OrganizationEvents? oe = JsonConvert.DeserializeObject<OrganizationEvents>(organizationEvent);
					db.OrganizationEvent.Add(oe);
					return HttpStatusCode.OK.ToString();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return HttpStatusCode.InternalServerError.ToString();
			}
		})
		.WithName("PostOrganizationEvents");

		return group;
	}
}


