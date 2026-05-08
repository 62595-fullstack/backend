using Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.OrganizationEvent;
using Models.UserEventBinding;
using Models.UserOrganizationBinding;
using Newtonsoft.Json;
using Services;
using System.Net;
using System.Security.Claims;

namespace Endpoints;

public static class OrganizationEventsEndpoint
{
	private static async Task<bool> IsEventOwner(OrganizationEvents ev, string userId, DataUserOrganizationBinding duob)
	{
		if (!int.TryParse(userId, out int parsedUserId))
		{
			return false;
		}

		UserOrganizationBindings? binding = await duob.getUserOrganizationBindingById(ev.UserOrganizationBindingId);
		return binding?.UserId == parsedUserId;
	}

	public static RouteGroupBuilder MapOrganizationEventsEndpoints(this RouteGroupBuilder group)
	{
		group.MapGet("/event/{id}", async Task<IResult> (int id, DataOrganizationEvents doe) =>
		{
			try
			{
				OrganizationEvents? ev = await doe.getOrganizationEventById(id);
				return ev == null ? Results.NotFound() : Results.Ok(ev);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return Results.Problem(ex.Message);
			}
		})
		.WithName("getOrganizationEventById");

		group.MapGet("/{organizationId}", async Task<string> (int organizationId, DataOrganizationEvents organizationData) =>
		{
			try
			{
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

		group.MapPost("/", async Task<IResult> ([FromBody] OrganizationEvents oe,
					ClaimsPrincipal user,
					DataUserOrganizationBinding duob,
					DataOrganizationEvents doe) =>
		{
			try
			{
				string? userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null) return Results.Unauthorized();

				UserOrganizationBindings? binding = await duob.getUserOrganizationBindingForUser(userId, oe.OrganizationId);
				if (binding == null) return Results.Forbid();

				oe.UserOrganizationBindingId = binding.Id;
				oe.CreatedDate = DateTime.SpecifyKind(oe.CreatedDate, DateTimeKind.Utc);
				oe.StartDate = DateTime.SpecifyKind(oe.StartDate, DateTimeKind.Utc);

				await doe.createOrganizationEvents(oe);
				return Results.Ok();
			}
			catch (DbUpdateException ex)
			{
				Console.WriteLine(ex.ToString());
				string detail = ex.InnerException?.Message ?? ex.Message;
				return Results.Problem(detail);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return Results.Problem(ex.Message);
			}
		})
		.WithName("PostOrganizationEvents");

		group.MapDelete("/{id}", async Task<IResult> (int id,
					DataOrganizationEvents doe,
					DataUserOrganizationBinding duob,
					ClaimsPrincipal user) =>
		{
			try
			{
				string? userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null) return Results.Unauthorized();

				OrganizationEvents? ev = await doe.getOrganizationEventById(id);
				if (ev == null) return Results.NotFound();

				if (!await IsEventOwner(ev, userId, duob))
					return Results.Forbid();

				await doe.deleteOrganizationEvent(id);
				return Results.Ok();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return Results.Problem(ex.Message);
			}
		})
		.WithName("DeleteOrganizationEvent");

		group.MapPatch("/{id}", async Task<IResult> (int id,
					[FromBody] UpdateEventRequest req,
					DataOrganizationEvents doe,
					DataUserOrganizationBinding duob,
					ClaimsPrincipal user) =>
		{
			try
			{
				string? userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null) return Results.Unauthorized();

				OrganizationEvents? ev = await doe.getOrganizationEventById(id);
				if (ev == null) return Results.NotFound();

				if (!await IsEventOwner(ev, userId, duob))
					return Results.Forbid();

				await doe.updateEvent(id, req);
				return Results.Ok();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return Results.Problem(ex.Message);
			}
		})
		.WithName("UpdateEvent");

		group.MapPost("/{UserEventBinding}", async Task<string> (string userEventBinding, DataOrganizationEvents doe) =>
		{
			try
			{
				UserEventBindings? ueb = JsonConvert.DeserializeObject<UserEventBindings>(userEventBinding);

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

		group.MapPost("/{eventId}/join", async Task<IResult> (int eventId, ClaimsPrincipal user, DataOrganizationEvents doe) =>
		{
			try
			{
				string? userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null || !int.TryParse(userId, out int parsedUserId))
					return Results.Unauthorized();

				if (await doe.isUserRegistered(parsedUserId, eventId))
					return Results.Conflict("Already registered for this event.");

				bool success = await doe.userJoinEvent(parsedUserId, eventId);
				return success ? Results.Ok() : Results.Problem("Failed to register.");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return Results.Problem(ex.Message);
			}
		})
		.WithName("JoinEvent");

		group.MapDelete("/{eventId}/join", async Task<IResult> (int eventId, ClaimsPrincipal user, DataOrganizationEvents doe) =>
		{
			try
			{
				string? userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null || !int.TryParse(userId, out int parsedUserId))
					return Results.Unauthorized();

				bool success = await doe.userLeaveEvent(parsedUserId, eventId);
				return success ? Results.Ok() : Results.NotFound();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return Results.Problem(ex.Message);
			}
		})
		.WithName("LeaveEvent");

		group.MapGet("/{eventId}/participants", async Task<IResult> (int eventId, DataOrganizationEvents doe) =>
		{
			try
			{
				var participants = await doe.getEventParticipants(eventId);
				return Results.Ok(participants);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return Results.Problem(ex.Message);
			}
		})
		.WithName("GetEventParticipants");

		group.MapGet("/{eventId}/is-registered", async Task<IResult> (int eventId, ClaimsPrincipal user, DataOrganizationEvents doe) =>
		{
			try
			{
				string? userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null || !int.TryParse(userId, out int parsedUserId))
					return Results.Unauthorized();

				bool registered = await doe.isUserRegistered(parsedUserId, eventId);
				return Results.Ok(registered);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return Results.Problem(ex.Message);
			}
		})
		.WithName("IsRegisteredForEvent");

		return group;
	}
}