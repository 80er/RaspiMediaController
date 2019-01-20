using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Gpio;

namespace MediaControllerBackendServices
{
    public class FanController
    {
        private GpioPin OutputPin;

        public FanController()
        {
            OutputPin = Pi.Gpio[0];
            OutputPin.PinMode = GpioPinDriveMode.Output;
        }
        public void On()
        {
            if (!OutputPin.Read())
            {
                OutputPin.Write(true);
            }
        }

        public void Off()
        {
            if (OutputPin.Read())
            {
                OutputPin.Write(false);
            }
        }
    }
}
