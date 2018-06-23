using System;
using System.Net.Mime;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MediaControllerBackendServices
{
    class Program
    {
        static void Main(string[] args)
        {
            var webHost = new WebHostBuilder()
                .UseKestrel()
                // TODO: get rid of this again after further development. just for testing ;)
                .UseUrls("http://*:5000")
                .UseStartup<Program>()
                .Build();
            webHost.Run();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSignalR();
            services.AddSingleton(typeof(TimeHubUpdateSingleton), typeof(TimeHubUpdateSingleton));
            services.AddSingleton(typeof(TemperatureUpdateSingleton), typeof(TemperatureUpdateSingleton));
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // next two lines to enforce loading of timerhub
            var foo = app.ApplicationServices;
            foo.GetService(typeof(TimeHubUpdateSingleton));
            foo.GetService(typeof(TemperatureUpdateSingleton));
            loggerFactory.AddConsole(LogLevel.Information);
            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:61813", "http://localhost:8001", "http://localhost:8002", "http://localhost:8003")
                    .AllowAnyHeader().AllowAnyMethod().AllowCredentials();
            });
            app.UseSignalR(routes =>
            {
                routes.MapHub<TimeHub>("/timehub");
                routes.MapHub<TemperatureHub>("/temperaturehub");
            });
            app.UseMvc();
        }
    }
}
