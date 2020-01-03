using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Timers;
using log4net;
using Timer = System.Timers.Timer;
using MediaControllerBackendServices.Messaging;
using Newtonsoft.Json;

namespace MediaControllerBackendServices.Broker
{
    internal class TimeBroker
    {
        private IMessageBus MessageBus { get; }
        private ILog Log { get; set; }
        private Timer Timer { get; set; }
        private static string myTopic = "time_data";
        private static int myLastMinute;
        public TimeBroker(IMessageBus messageBus, ILog log)
        {
            Log = log;
            MessageBus = messageBus;
            MessageBus.MessageReceived += MessageBusOnMessageReceived;
            myLastMinute = -1;
            TimerElapsed(null, null);
            StartTimer();
        }

        private void MessageBusOnMessageReceived(object sender, MessageReceivedArgs e)
        {
            if (e.Message.Topic != myTopic)
            {
                return;
            }

            if (e.Message.Payload == "resend_all")
            {
                TimerElapsed(null, null, true);
            }
        }

        public void StartTimer()
        {
            Timer = new Timer
            {
                Interval = 500
            };
            Timer.Elapsed += TimerElapsed;
            Timer.Start();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            TimerElapsed(sender, e, false);
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e, bool forceSend)
        {
            try
            {
                var time = new Time();
                if (time.Minute != myLastMinute || forceSend)
                {
                    myLastMinute = time.Minute;
                    var payload = JsonConvert.SerializeObject(time);
                    var message = new Message(myTopic, payload);
                    MessageBus.SendMessage(message);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

        }

        private class Time
        {
            public int Hour { get; }
            public int Minute { get; }
            public int Day { get; }
            public int Month { get; }
            public int Year { get; }
            public string Dayname { get; }

            public Time()
            {
                var now = DateTime.Now.ToLocalTime();
                Hour = now.Hour;
                Minute = now.Minute;
                Day = now.Day;
                Month = now.Month;
                Year = now.Year;
                Dayname = ResolveDayname(now.DayOfWeek);
            }

            public string ResolveDayname(DayOfWeek day)
            {
                switch (day)
                {
                    case DayOfWeek.Friday:
                        return "Freitag";
                    case DayOfWeek.Monday:
                        return "Montag";
                    case DayOfWeek.Saturday:
                        return "Samstag";
                    case DayOfWeek.Sunday:
                        return "Sonntag";
                    case DayOfWeek.Thursday:
                        return "Donnerstag";
                    case DayOfWeek.Tuesday:
                        return "Dienstag";
                    case DayOfWeek.Wednesday:
                        return "Mittwoch";
                    default:
                        return "Don't Panik! 42!";
                }
            }

        }
    }

}
