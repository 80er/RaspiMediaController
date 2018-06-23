using System;
using System.Threading;
using System.Timers;
using Microsoft.AspNetCore.SignalR;
using Timer = System.Timers.Timer;

namespace MediaControllerBackendServices
{
    class TimeHubUpdateSingleton
    {
        Timer myTimer;
        private static int myLastMinute;
        public TimeHubUpdateSingleton(IHubContext<TimeHub> hubContext)
        {
            myLastMinute = DateTime.Now.Minute;
            _hubContext = hubContext;
            //TimerElapsed(null, null);
            StartTimerAfterFullMinute();
        }

        private IHubContext<TimeHub> _hubContext;

        public void StartTimerAfterFullMinute()
        {
            var sleepSeconds = 59 - DateTime.Now.Second;
            Thread.Sleep(sleepSeconds * 1000);
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
                var time = DateTime.Now;
                if (time.Minute != myLastMinute)
                {
                    myLastMinute = time.Minute;
                    await _hubContext.Clients.All.SendAsync("UpdateTime", time.Hour, time.Minute);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

        }
    }
}
