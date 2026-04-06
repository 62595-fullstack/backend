using backend.getdata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.OrganizationEvent;
using Models.UserEventBinding;
using Newtonsoft.Json;
using System.Net;

namespace Endpoints;

public static class OrganizationEventsEndpoint
{
	public static RouteGroupBuilder MapOrganizationEventsEndpoints(this RouteGroupBuilder group)
	{
		group.MapGet("/event/{id}", async Task<IResult> (int id) =>
		{
			try
			{
				DataOrganizationEvents doe = new DataOrganizationEvents();
				OrganizationEvents? ev = await doe.getOrganizationEventById(id);
				if (ev == null) return Results.NotFound();
				return Results.Ok(ev);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return Results.Problem(ex.Message);
			}
		})
		.WithName("getOrganizationEventById");

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
			//Console.WriteLine($"[POST /OrganizationEvents] Id={oe.Id} OrganizationId={oe.OrganizationId} Title={oe.Title}");
			try
			{
				using DatabaseContext db = new();

				bool organizationExists = await db.Organization.AnyAsync(o => o.Id == oe.OrganizationId);
				if (!organizationExists) return Results.BadRequest($"Organization with ID {oe.OrganizationId} does not exist.");

				DataOrganizationEvents doe = new();
				await doe.createOrganizationEvents(oe);
				return Results.Ok();
			}
			catch (DbUpdateException ex)
			{
				Console.WriteLine(ex.ToString());
				var detail = ex.InnerException?.Message ?? ex.Message;
				return Results.Problem(detail);
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