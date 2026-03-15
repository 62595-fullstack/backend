using backend.getdata;
using Models.User;
using Newtonsoft.Json;

namespace Endpoints;

public static class UserEndpoint
{
	public static RouteGroupBuilder MapUserEndpoint(this RouteGroupBuilder group)
	{
		group.MapPost("/", async Task<string> (string userData) =>
		{
			try
			{
                Users? u = JsonConvert.DeserializeObject<Users>(userData);
				
                if(u != null)
                {
                    
                    using (DatabaseContext db = new DatabaseContext())
                    {
                        DataUser du = new DataUser();
                        bool created = await du.setUsers(u);

                        return System.Net.HttpStatusCode.OK.ToString();
                    }
                } 
                else
                {
                    throw new Exception("No userData");
                }
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return System.Net.HttpStatusCode.InternalServerError.ToString();
			}
		})
		.WithName("CreateUser");

		return group;
	}
}
