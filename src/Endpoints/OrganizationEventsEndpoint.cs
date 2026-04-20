using backend.getdata;
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

				using DatabaseContext db = new();

				bool organizationExists = await db.Organization.AnyAsync(o => o.Id == oe.OrganizationId);
				if (!organizationExists) return Results.BadRequest($"Organization with ID {oe.OrganizationId} does not exist.");

				UserOrganizationBindings? binding = await db.UserOrganizationBinding
					.FirstOrDefaultAsync(b => b.UserId == int.Parse(userId) && b.OrganizationId == oe.OrganizationId);
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

		group.MapDelete("/{id}", async Task<IResult> (int id, ClaimsPrincipal user) =>
		{
			try
			{
				string? userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null) return Results.Unauthorized();

				using DatabaseContext db = new();
				OrganizationEvents? ev = await db.OrganizationEvent.FindAsync(id);
				if (ev == null) return Results.NotFound();

				UserOrganizationBindings? binding = await db.UserOrganizationBinding.FindAsync(ev.UserOrganizationBindingId);
				if (binding == null || binding.UserId != int.Parse(userId))
					return Results.Forbid();

				db.OrganizationEvent.Remove(ev);
				await db.SaveChangesAsync();
				return Results.Ok();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return Results.Problem(ex.Message);
			}
		})
		.WithName("DeleteOrganizationEvent");

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