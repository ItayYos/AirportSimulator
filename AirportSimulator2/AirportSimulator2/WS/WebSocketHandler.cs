using AirportSimulator2.DAL;
using AirportSimulator2.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AirportSimulator2.WS
{
    public class WebSocketHandler : IWebSocketHandler
    {
        /* Web socket service, to handle active websocekts. */
        private IWebSocketConnections _socketConnections;
        private IAirportLog _logger;
        
        public WebSocketHandler(IWebSocketConnections socketConnections, IAirportLog logger)
        {
            _socketConnections = socketConnections;
            _logger = logger;
        }

        public async Task WsConnectAsync(HttpContext context, WebSocket webSocket) 
        {
            /* Tries to add a new connection to the active websocket connections list. */
            try
            {
                _socketConnections.Sockets.TryAdd(context.Connection.Id, webSocket);
            }
            catch (Exception e)
            {
                await _logger.LogError(e.Message);
            }

        }

        public async Task SendAllAsync(WSClientReport report)
        {
            /* Receives a message and send it to all currently active web socket connecitons. */
            foreach (WebSocket ws in _socketConnections.Sockets.Values)
            {
                try
                {
                    var jsonStr = JsonConvert.SerializeObject(report);
                    byte[] objInBytes = Encoding.UTF8.GetBytes(jsonStr);
                    ArraySegment<byte> objMsg2Client = new ArraySegment<byte>(objInBytes, 0, objInBytes.Length);
                    try
                    {
                        await ws.SendAsync(objInBytes, WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    catch (Exception ex)
                    {
                        await _logger.LogError(ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    await _logger.LogError(ex.Message);
                }
            }
        }
    }
}
