using backend.getdata;
using System.Net.WebSockets;
namespace Endpoints;

public static class MessageEndpoint
{
	public static RouteGroupBuilder MapMessagesEndpoints(this RouteGroupBuilder group)
	{
		group.MapGet("/", async Task<string> (HttpContext httpcontext) =>
		{
			try
			{
				if (httpcontext.WebSockets.IsWebSocketRequest)
				{
					using var webSocket = await httpcontext.WebSockets.AcceptWebSocketAsync();
					DataMessages dm = new DataMessages();
					

					return await dm.getMessages("1"); 
				}
				else
				{
					httpcontext.Response.StatusCode = StatusCodes.Status400BadRequest;
				}
                
				return "2";
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return "404";
			}
		})
		.WithName("getMessages");

		return group;
	}
}