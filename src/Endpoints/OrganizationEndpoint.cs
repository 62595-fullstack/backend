using backend.getdata;
using Models.Organization;
using Newtonsoft.Json;
using System.Net;

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

		group.MapPost("/", async Task<string> ([Microsoft.AspNetCore.Mvc.FromBody] Organizations o) =>
		{
			try
			{
				DataOrganization DO = new DataOrganization();
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