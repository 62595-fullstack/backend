using backend.getdata;
using Dto;
using Models.Organization;
using Models.UserOrganizationBinding;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;

namespace Endpoints;

public static class OrganizationEndpoint
{
	public static RouteGroupBuilder MapOrganizationEndpoints(this RouteGroupBuilder group)
	{
		group.MapGet("/", async Task<IResult> () =>
		{
			try
			{
				DataOrganization organizationData = new DataOrganization();
				List<Organizations>? allOrganizations = await organizationData.GetAllOrganization();
				string allOrganizationsJson = JsonConvert.SerializeObject(allOrganizations);
				return Results.Ok(allOrganizationsJson);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return Results.BadRequest();
			}
		})
		.WithName("GetOrganizations");

		group.MapPost("/", async Task<IResult> ([Microsoft.AspNetCore.Mvc.FromBody] Organizations o, ClaimsPrincipal user) =>
		{
			try
			{
				string? userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null) return Results.Unauthorized();

				DataOrganization DO = new();
				Organizations created = await DO.CreateOrganization(o);

				DataUserOrganizationBinding duob = new();
				await duob.setUserToOrganization(userId, created.Id, 1000);

				return Results.Ok(JsonConvert.SerializeObject(created));
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return Results.Problem(ex.Message);
			}
		})
		.WithName("PostOrganizations")
		.RequireAuthorization();

		group.MapGet("/{id}", async Task<string> (int id) =>
		{
			try
			{
				DataOrganization organizationData = new DataOrganization();
				Organizations? allOrganizations = await organizationData.GetOrganizationById(id);
				return JsonConvert.SerializeObject(allOrganizations);

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return "{}";
			}
		})
		.WithName("GetOrganizationsById");

		group.MapPatch("/{id}", async Task<IResult> (int id, UpdateOrganizationDto request, ClaimsPrincipal user) =>
		{
			try
			{
				string? userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null) return Results.Unauthorized();

				DataUserOrganizationBinding duob = new();
				UserOrganizationBindings? binding = await duob.getUserOrganizationBindingForUser(userId, id);
				if (binding is not { RoleId: 1000 }) return Results.Forbid();

				DataOrganization organizationData = new();
				Organizations? updated = await organizationData.UpdateDescription(id, request.Description);
				if (updated == null) return Results.NotFound();
				
				return Results.Ok(JsonConvert.SerializeObject(updated));
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return Results.Problem(ex.Message);
			}
		})
		.WithName("PatchOrganization")
		.RequireAuthorization();

		group.MapDelete("/{id}", async Task<string> (int id) =>
		{
			try
			{
				DataOrganization organizationData = new DataOrganization();
				bool allOrganizations = await organizationData.DeleteOrganization(id);
				return JsonConvert.SerializeObject(allOrganizations);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return "{}";
			}
		})
		.WithName("DeleteOrganizationsById");

		return group;
	}
}