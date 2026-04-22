using backend.getdata;
using Dto;
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

				DataUserOrganizationBinding duob = new();
				List<UserOrganizationBindings> bindings = await duob.getAllUserOrganizationBindingsForUser(userId);
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

				DataUserOrganizationBinding organizationData = new();
				UserOrganizationBindings? binding = await organizationData.getUserOrganizationBindingForUser(userId, organizationId);
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
				DataUserOrganizationBinding organizationData = new();
				List<UserOrganizationBindings> allOrganizations = await organizationData.getUserOrganizationForOrganization(organizationId);
				return JsonConvert.SerializeObject(allOrganizations);
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
				DataUserOrganizationBinding organizationData = new();
				bool successful = await organizationData.setUserToOrganization(userId, organizationId, roleId);
				return JsonConvert.SerializeObject(successful);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return "{}";
			}
		})
		.WithName("setUserToOrganization");

		group.MapPost("/join/{organizationId}", async Task<IResult> (int organizationId, ClaimsPrincipal user) =>
		{
			try
			{
				string? userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null) return Results.Unauthorized();

				DataUserOrganizationBinding organizationData = new();
				UserOrganizationBindings? existing = await organizationData.getUserOrganizationBindingForUser(userId, organizationId);
				if (existing != null) return Results.Conflict("Already a member.");

				bool successful = await organizationData.setUserToOrganization(int.Parse(userId), organizationId, 999);
				return successful ? Results.Ok() : Results.Problem("Failed to join organization.");
			}
			catch (Exception ex)
			{
				string detail = ex.InnerException?.Message ?? ex.Message;
				Console.WriteLine(detail);
				return Results.Problem(detail);
			}
		})
		.WithName("joinOrganization");

		group.MapDelete("/leave/{organizationId}", async Task<IResult> (int organizationId, ClaimsPrincipal user) =>
		{
			try
			{
				string? userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null) return Results.Unauthorized();

				DataUserOrganizationBinding organizationData = new();
				bool successful = await organizationData.removeUserFromOrganization(userId, organizationId);
				return successful ? Results.Ok() : Results.NotFound("Binding not found.");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return Results.Problem(ex.Message);
			}
		})
		.WithName("leaveOrganization");

		group.MapGet("/{organizationId}/members", async Task<IResult> (int organizationId) =>
		{
			try
			{
				DataUserOrganizationBinding data = new();
				List<OrgMemberDto> members = await data.getOrganizationMembersWithDetails(organizationId);
				return Results.Ok(members);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return Results.Problem(ex.Message);
			}
		})
		.WithName("getOrganizationMembers");

		group.MapDelete("/{organizationId}/member/{userId}", async Task<IResult> (int organizationId, string userId, ClaimsPrincipal user) =>
		{
			try
			{
				string? adminId = user.FindFirstValue(ClaimTypes.NameIdentifier);
				if (adminId == null) return Results.Unauthorized();

				DataUserOrganizationBinding data = new();
				UserOrganizationBindings? adminBinding = await data.getUserOrganizationBindingForUser(adminId, organizationId);
				if (adminBinding?.RoleId != 1000) return Results.Forbid();

				bool success = await data.removeUserFromOrganization(userId, organizationId);
				return success ? Results.Ok() : Results.NotFound("Binding not found.");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return Results.Problem(ex.Message);
			}
		})
		.WithName("removeOrganizationMember");

		group.MapPatch("/{organizationId}/member/{userId}/role/{roleId}", async Task<IResult> (int organizationId, string userId, int roleId, ClaimsPrincipal user) =>
		{
			try
			{
				string? adminId = user.FindFirstValue(ClaimTypes.NameIdentifier);
				if (adminId == null) return Results.Unauthorized();

				DataUserOrganizationBinding data = new();
				UserOrganizationBindings? adminBinding = await data.getUserOrganizationBindingForUser(adminId, organizationId);
				if (adminBinding?.RoleId != 1000) return Results.Forbid();

				bool success = await data.updateUserRoleInOrganization(int.Parse(userId), organizationId, roleId);
				return success ? Results.Ok() : Results.NotFound("Binding not found.");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return Results.Problem(ex.Message);
			}
		})
		.WithName("updateOrganizationMemberRole");

		return group;
	}
}