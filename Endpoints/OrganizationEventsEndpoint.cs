using backend.getdata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.OrganizationEvent;
using Models.UserEventBinding;
using Newtonsoft.Json;
using Npgsql;
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
				DataOrganizationEvents organizationData = new DataOrganizationEvents();
				List<OrganizationEvents> allOrganizations = await organizationData.getOrganizationEvents(organizationId);
				return JsonConvert.SerializeObject(allOrganizations);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return "{}";
			}
		})
		.WithName("getOrganizationEvents");

		group.MapPost("/", async Task<IResult> ([FromBody] OrganizationEvents oe) =>
		{
			try
			{
				DataOrganizationEvents doe = new DataOrganizationEvents();
				await doe.createOrganizationEvents(oe);
				return Results.Ok();
			}
			catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: "23503" } pgEx)
			{
				Console.WriteLine(ex.ToString());
				return Results.BadRequest($"Organization with ID {oe.OrganizationId} does not exist.");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return Results.Problem(ex.Message);
			}
		})
		.WithName("PostOrganizationEvents");

		group.MapPost("/{UserEventBinding}", async Task<string> (string userEventBinding) =>
		{
			try
			{
				UserEventBindings? ueb = JsonConvert.DeserializeObject<UserEventBindings>(userEventBinding);
				DataOrganizationEvents doe = new DataOrganizationEvents();
				
				if (ueb != null)
				{
					await doe.userJoinEvent(ueb.UserId, ueb.OrganizationEventsId);
				}
				else
				{
					return HttpStatusCode.InternalServerError.ToString();
				}

				return HttpStatusCode.OK.ToString();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return HttpStatusCode.InternalServerError.ToString();
			}
		})
		.WithName("UserJoinEvent");

		return group;
	}
}


