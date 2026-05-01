using backend.getdata;
using System.Net.WebSockets;
namespace Endpoints;

public static class MessageEndpoint
{
	public static RouteGroupBuilder MapMessagesEndpoints(this RouteGroupBuilder group)
	{
		group.MapGet("/{UserId}", async Task<int> (WebSocket websocket) =>
		{
			try
			{

                // DatabaseContext db = new DatabaseContext();

                // var message = db.


				// websocket.SendAsync()
				return 0;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return 404;
			}
		})
		.WithName("getMessages");

		return group;
	}
}