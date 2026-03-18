using backend.getdata;
using Models.User;
using Newtonsoft.Json;

namespace Endpoints;

public static class loginEndpoint
{
	public static RouteGroupBuilder MaploginEndpoint(this RouteGroupBuilder group)
	{
		group.MapPost("/{email}", async Task<string> (string email) =>
		{
			try
			{
				using (DatabaseContext db = new DatabaseContext())
				{

                    Users? u = JsonConvert.DeserializeObject<Users>(email);

                    if (u != null)
                    {
                        


					DataUser ud = new DataUser();
					bool userCreaded = await ud.setUsers(u);
					return userCreaded.ToString();
                    } 
                    else
                    {
                        return false.ToString();
                    }
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return "{}";
			}
		})
		.WithName("CreateUser");



        group.MapGet("/{email}", async Task<string> (string email) =>
		{
			try
			{
				using (DatabaseContext db = new DatabaseContext())
				{

                    Users? u = JsonConvert.DeserializeObject<Users>(email);

                    if (u != null)
                    {
                        


					DataUser ud = new DataUser();
					bool userCreaded = await ud.loginUsers(u);
					return userCreaded.ToString();
                    } 
                    else
                    {
                        return false.ToString();
                    }
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return "{}";
			}
		})
		.WithName("Login");

		return group;
	}
}
