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
        public TemperatureUpdateSingleton(IHubContext<TimeHub> hubContext)
        {
            if (Directory.Exists("/etc") && Directory.Exists("/usr"))
            {
                OnLinux = true;
            }
            if (OnLinux) TemperatureReader = new LinuxCpuTemperatureReader();
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
                 await _hubContext.Clients.All.SendAsync("UpdateTemperature", TemperatureReader.GetCurrentTemperature());
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

        }
    }
}
