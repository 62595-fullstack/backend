using Dto;
using Models.Organization;
using Models.UserOrganizationBinding;
using Newtonsoft.Json;
using Services;
using System.Net;
using System.Security.Claims;

namespace Endpoints;

public static class OrganizationEndpoint
{
	public static RouteGroupBuilder MapOrganizationEndpoints(this RouteGroupBuilder group)
	{
		group.MapGet("/", async Task<IResult> (DataOrganization organizationData) =>
		{
			try
			{
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

		group.MapPost("/", async Task<string> ([Microsoft.AspNetCore.Mvc.FromBody] Organizations o, DataOrganization DO) =>
		{
			try
			{
				await DO.CreateOrganization(o);
				return HttpStatusCode.OK.ToString();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return HttpStatusCode.InternalServerError.ToString();
			}
		})
		.WithName("PostOrganizations");

		group.MapGet("/{id}", async Task<string> (int id, DataOrganization organizationData) =>
		{
			try
			{
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

		group.MapPatch("/{id}", async Task<IResult> (int id,
					ClaimsPrincipal user,
					UpdateOrganizationDto request,
					DataUserOrganizationBinding duob,
					DataOrganization organizationData) =>
		{
			try
			{
				string? userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
				if (userId == null) return Results.Unauthorized();

				UserOrganizationBindings? binding = await duob.getUserOrganizationBindingForUser(userId, id);
				if (binding is not { RoleId: 1000 }) return Results.Forbid();

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

		group.MapDelete("/{id}", async Task<string> (int id, DataOrganization organizationData) =>
		{
			try
			{
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