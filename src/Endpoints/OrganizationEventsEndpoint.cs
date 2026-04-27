using backend.getdata;
using Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.OrganizationEvent;
using Models.UserEventBinding;
using Models.UserOrganizationBinding;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;

namespace Endpoints;

public static class OrganizationEventsEndpoint
{
	public record UpdateOrganizationEventDto(string? Description, string? Rules, string? BracketResults);

	public static RouteGroupBuilder MapOrganizationEventsEndpoints(this RouteGroupBuilder group)
	{
		group.MapGet("/event/{id}", async Task<IResult> (int id) =>
		{
			try
			{
				DataOrganizationEvents doe = new();
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

		group.MapGet("/{organizationId}", async Task<string> (int organizationId) =>
		{
			try
			{
				DataOrganizationEvents organizationData = new();
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

		group.MapPost("/", async Task<IResult> ([FromBody] OrganizationEvents oe, ClaimsPrincipal user) =>
		{
			try
			{
				string? userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null) return Results.Unauthorized();

				DataUserOrganizationBinding duob = new();
				UserOrganizationBindings? binding = await duob.getUserOrganizationBindingForUser(userId, oe.OrganizationId);
				if (binding == null) return Results.Forbid();

				oe.UserOrganizationBindingId = binding.Id;
				oe.CreatedDate = DateTime.SpecifyKind(oe.CreatedDate, DateTimeKind.Utc);
				oe.StartDate = DateTime.SpecifyKind(oe.StartDate, DateTimeKind.Utc);

				DataOrganizationEvents doe = new();
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

		group.MapPatch("/{id}", async Task<IResult> (int id, [FromBody] UpdateOrganizationEventDto update) =>
		{
			try
			{
				using DatabaseContext db = new();
				OrganizationEvents? ev = await db.OrganizationEvent.FindAsync(id);
				if (ev == null) return Results.NotFound();

				if (update.Description != null) ev.Description = update.Description;
				if (update.Rules != null) ev.Rules = update.Rules;
				if (update.BracketResults != null) ev.BracketResults = update.BracketResults;

				await db.SaveChangesAsync();
				return Results.NoContent();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return Results.Problem(ex.Message);
			}
		})
		.WithName("UpdateOrganizationEvent");

		group.MapDelete("/{id}", async Task<IResult> (int id, ClaimsPrincipal user) =>
		{
			try
			{
				string? userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null) return Results.Unauthorized();

				DataOrganizationEvents doe = new();
				OrganizationEvents? ev = await doe.getOrganizationEventById(id);
				if (ev == null) return Results.NotFound();

				DataUserOrganizationBinding duob = new();
				UserOrganizationBindings? binding = await duob.getUserOrganizationBindingById(ev.UserOrganizationBindingId);
				if (binding == null || binding.UserId != int.Parse(userId))
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

		group.MapPatch("/{id}", async Task<IResult> (int id, [FromBody] UpdateEventRequest req, ClaimsPrincipal user) =>
		{
			try
			{
				string? userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null) return Results.Unauthorized();

				DataOrganizationEvents doe = new();
				OrganizationEvents? ev = await doe.getOrganizationEventById(id);
				if (ev == null) return Results.NotFound();

				DataUserOrganizationBinding duob = new();
				UserOrganizationBindings? binding = await duob.getUserOrganizationBindingById(ev.UserOrganizationBindingId);
				if (binding == null || binding.UserId != int.Parse(userId))
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
