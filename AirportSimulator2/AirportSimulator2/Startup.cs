using AirportSimulator2.AirportLayoutBuilder;
using AirportSimulator2.BL;
using AirportSimulator2.DAL;
using AirportSimulator2.Services;
using AirportSimulator2.WS;
using Logger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace AirportSimulator2
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IControlTower, ControlTower>();
            services.AddTransient<IWebSocketConnections, WebSocketConnections>();
            services.AddTransient<IAirportLogic, AirportLogic>();
            services.AddTransient<IAirportBuilder, AirportBuilder>();
            services.AddTransient<IWebSocketHandler, WebSocketHandler>();
            services.AddTransient<IAirportLog, AirportLogger>();
            services.AddTransient<ILoggerService, LoggerService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };
            app.UseWebSockets(webSocketOptions);

            app.UseMiddleware<WSMiddleware>();

            /*
            app.Use(async (context, next) =>
            {
                // ws://www.yourdomian.com/wsConnect  
                if (context.Request.Path == "/wsConnect")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        try
                        {
                            WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                            var handler = app.ApplicationServices.GetRequiredService<IWebSocketHandler>();

                            TaskCompletionSource<object> socketTask = new TaskCompletionSource<object>();
                            handler.WsConnectAsync(context, webSocket);
                            await socketTask.Task;
                        }
                        
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }
            });
            */
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Airport simulator.");
                });
            });
        }
    }
}
