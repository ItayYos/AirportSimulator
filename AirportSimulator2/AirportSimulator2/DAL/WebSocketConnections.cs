using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace AirportSimulator2.DAL
{
    public class WebSocketConnections : IWebSocketConnections
    {
        /* Holds a dictionary of active web sockets connections. */
        private static ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();
        public ConcurrentDictionary<string, WebSocket> Sockets
        {
            get
            {
                return _sockets;
            }
        }
    }
}
