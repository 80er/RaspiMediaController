using System;
using System.Net.Mime;
using System.Text;
using System.Threading;
using MediaControllerBackendServices.WeatherStation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;

namespace MediaControllerBackendServices
{
    class Program
    {
        static void Main(string[] args)
        {
            //var webHost = new WebHostBuilder()
            //    .UseKestrel()
            //    // TODO: get rid of this again after further development. just for testing ;)
            //    .UseUrls("http://*:5000")
            //    .UseStartup<Program>()
            //    .Build();
            //webHost.Run();
            string clientSecret = Environment.GetEnvironmentVariable("NETATMO_CLIENT_SECRET");
            string client = Environment.GetEnvironmentVariable("NETATMO_CLIENT");
            string user = Environment.GetEnvironmentVariable("NETATMO_USER");
            string password = Environment.GetEnvironmentVariable("NETATMO_PASSWORD");
            string device = Environment.GetEnvironmentVariable("NETATMO_DEVICE");
            var mainstation = WeatherStationFactory.Create(client, clientSecret, user, password, device);
            Console.WriteLine(mainstation.ToString());
            CreateMqtt();
        }

        private static async void CreateMqtt()
        {
            try
            {
                var mqttFactory = new MqttFactory();
                var options = new MqttClientOptionsBuilder().WithClientId("MediaServerBackend").WithTcpServer("127.0.0.1", 1883).WithCleanSession().Build();

                var client = mqttFactory.CreateMqttClient();

                var result = client.ConnectAsync(options).Result;
                var message = new MqttApplicationMessageBuilder().WithPayload("juhu").WithTopic("test_topic").Build();
                await client.PublishAsync(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
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
