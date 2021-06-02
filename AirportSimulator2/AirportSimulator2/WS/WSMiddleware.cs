using AirportSimulator2.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace AirportSimulator2.WS
{
    public class WSMiddleware
    {
        private IWebSocketHandler _handler;
        private IAirportLog _logger;
        private RequestDelegate _next;
        public WSMiddleware(IWebSocketHandler handler, IAirportLog logger, RequestDelegate next)
        {
            _handler = handler;
            _logger = logger;
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/wsConnect")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    try
                    {
                        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        //var handler = app.ApplicationServices.GetRequiredService<IWebSocketHandler>();

                        TaskCompletionSource<object> socketTask = new TaskCompletionSource<object>();
                        await _handler.WsConnectAsync(context, webSocket);
                        await socketTask.Task;
                    }

                    catch (Exception ex)
                    {
                        //Console.WriteLine(ex.Message);
                        await _logger.LogError(ex.Message);
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}
