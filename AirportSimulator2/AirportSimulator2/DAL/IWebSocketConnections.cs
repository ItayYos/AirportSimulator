using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace AirportSimulator2.DAL
{
    public interface IWebSocketConnections
    {
        ConcurrentDictionary<string, WebSocket> Sockets { get; }
    }
}
