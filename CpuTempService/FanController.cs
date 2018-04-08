using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Gpio;

namespace CpuTempService
{
    [Route(("fan"))]
    public class FanController : Controller
    {
        private GpioPin OutputPin;

        public FanController()
        {
            OutputPin = Pi.Gpio[0];
            OutputPin.PinMode = GpioPinDriveMode.Output;
        }
        [HttpGet("on")]
        public IActionResult On()
        {
            if (!OutputPin.Read())
            {
                OutputPin.Write(true);
            }
            return Ok();
        }

        [HttpGet("off")]
        public IActionResult Off()
        {
            if (OutputPin.Read())
            {
                OutputPin.Write(false);
            }
            return Ok();
        }
    }
}
