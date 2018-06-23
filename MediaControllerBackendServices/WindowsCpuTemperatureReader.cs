using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MediaControllerBackendServices
{
    class WindowsCpuTemperatureReader : ICpuTemperatureReader
    {
        private static System.Timers.Timer myTimer;
        private static double CurrentTemperature = 35;
        static WindowsCpuTemperatureReader()
        {
            myTimer = new Timer
            {
                Interval = 2000
            };
            myTimer.Elapsed += MyTimerOnElapsed;
            myTimer.Start();
        }

        private static void MyTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            if (CurrentTemperature == 35.0) CurrentTemperature = 36.123;
            else CurrentTemperature = 35.0;

        }

        public CpuTemperature GetCurrentTemperature()
        {
            return new CpuTemperature(CurrentTemperature);
        }
    }
}
