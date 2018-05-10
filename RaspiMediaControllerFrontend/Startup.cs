using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RaspiMediaControllerFrontend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
            });

            if (HybridSupport.IsElectronActive)
            {
                Task.Run(async () =>
                {
                    var browserWindowOptions =
                        new BrowserWindowOptions
                        {
                            Show = true,
                            Kiosk = true,
                            //Fullscreen = true,
                           // AutoHideMenuBar = true,
                            AlwaysOnTop = true,
                            DarkTheme = true,
                            DisableAutoHideCursor = false,
                            EnableLargerThanScreen = true,
                            //Frame = false
                        };
                    var mainWindow = await Electron.WindowManager
                        .CreateWindowAsync(browserWindowOptions);
                    mainWindow.SetKiosk(true);
                    mainWindow.OnReadyToShow +=
                        () => mainWindow.Show();
                });
            }
        }
    }
}
