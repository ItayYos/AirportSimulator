using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AirportSimulator2.WS
{
    public interface IWebSocketHandler
    {
        Task WsConnectAsync(HttpContext context, System.Net.WebSockets.WebSocket webSocket);
        Task SendAllAsync(WSClientReport report);
    }
}
