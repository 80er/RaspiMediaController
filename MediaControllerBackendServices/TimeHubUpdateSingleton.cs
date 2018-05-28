using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Microsoft.AspNetCore.SignalR;

namespace MediaControllerBackendServices
{
    class TimeHubUpdateSingleton
    {
        System.Timers.Timer myTimer;
        public TimeHubUpdateSingleton(IHubContext<TimeHub> hubContext)
        {
            _hubContext = hubContext;
            StartTimer();
        }

        private IHubContext<TimeHub> _hubContext;

        public void StartTimer()
        {
            myTimer = new Timer();
            myTimer.Interval = 1000;
            myTimer.Elapsed += TimerElapsed;
            myTimer.Start();
        }

        private async void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                var time = DateTime.Now;
                await _hubContext.Clients.All.SendAsync("UpdateTime", time.Hour, time.Minute, time.Second);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

        }
    }
}
