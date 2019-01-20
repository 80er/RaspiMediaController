using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Timers;
using Microsoft.AspNetCore.SignalR;

namespace MediaControllerBackendServices
{
    class TemperatureUpdateSingleton
    {
        private bool OnLinux { get; }
        Timer myTimer;
        private ICpuTemperatureReader TemperatureReader { get; }
        private FanController FanController { get; set; }
        public TemperatureUpdateSingleton(IHubContext<TimeHub> hubContext)
        {
            if (Directory.Exists("/etc") && Directory.Exists("/usr"))
            {
                OnLinux = true;
            }

            if (OnLinux)
            {
                TemperatureReader = new LinuxCpuTemperatureReader();
                FanController = new FanController();
            }
            else TemperatureReader = new WindowsCpuTemperatureReader();
            _hubContext = hubContext;
            StartTimer();
        }

        private IHubContext<TimeHub> _hubContext;

        public void StartTimer()
        {
            myTimer = new Timer
            {
                Interval = 2000
            };
            myTimer.Elapsed += TimerElapsed;
            myTimer.Start();
        }

        private async void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                var currentTemperature = TemperatureReader.GetCurrentTemperature();
                await _hubContext.Clients.All.SendAsync("UpdateTemperature", currentTemperature);
                if (currentTemperature.Temperature > 65.0 && FanController != null)
                {
                    FanController.On();
                }
                else if (currentTemperature.Temperature <= 65.0 && FanController != null)
                {
                    FanController.Off();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

        }
    }
}
