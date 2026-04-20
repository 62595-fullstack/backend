using backend.getdata;
using Microsoft.EntityFrameworkCore;
using Models.UserOrganizationBinding;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Endpoints;

public static class UserOrganizationBinding
{
	public static RouteGroupBuilder MapUserOrganizationBindingEndpoints(this RouteGroupBuilder group)
	{
		group.MapGet("/me", async Task<IResult> (ClaimsPrincipal user) =>
		{
			try
			{
				string? userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null) return Results.Unauthorized();

				using DatabaseContext db = new();
				List<UserOrganizationBindings> bindings = await db.UserOrganizationBinding
					.Where(b => b.UserId == int.Parse(userId))
					.ToListAsync();
				return Results.Ok(bindings);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return Results.Problem(ex.Message);
			}
		})
		.WithName("getAllUserOrganizationBindingsForCurrentUser");

		group.MapGet("/{organizationId}/me", async Task<IResult> (int organizationId, ClaimsPrincipal user) =>
		{
			try
			{
				string? userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null) return Results.Unauthorized();

				DataUserOrganizationBinding organizationData = new DataUserOrganizationBinding();
				UserOrganizationBindings? binding = await organizationData.getUserOrganizationBindingForUser(userId, organizationId);
				if (binding == null) return Results.NotFound();
				return Results.Ok(binding);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return Results.Problem(ex.Message);
			}
		})
		.WithName("getUserOrganizationBindingForCurrentUser");

		group.MapGet("/{organizationId}", async Task<string> (int organizationId) =>
		{
			try
			{
				using (DatabaseContext db = new DatabaseContext())
				{
					DataUserOrganizationBinding organizationData = new DataUserOrganizationBinding();
					List<UserOrganizationBindings> allOrganizations = await organizationData.getUserOrganizationForOrganization(organizationId);
					return JsonConvert.SerializeObject(allOrganizations);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return "{}";
			}
		})
		.WithName("getUserOrganizationBinding");

		group.MapPost("/{userId}/{organizationId}/{roleId}", async Task<string> (int userId, int organizationId, int roleId) =>
		{
			try
			{
				using (DatabaseContext db = new DatabaseContext())
				{
					DataUserOrganizationBinding organizationData = new DataUserOrganizationBinding();
					bool successful = await organizationData.setUserToOrganization(userId, organizationId, roleId);
					return JsonConvert.SerializeObject(successful);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return "{}";
			}
		})
		.WithName("setUserToOrganization");

		return group;
	}
}