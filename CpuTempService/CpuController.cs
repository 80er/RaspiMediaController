using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace CpuTempService
{
    [Route(("cpu"))]
    public class CpuController : Controller
    {
        private bool OnLinux { get; }
        private ICpuTemperatureReader TemperatureReader { get; }

        public CpuController()
        {
            // TODO: Find better way to do so ;)
            if (Directory.Exists("/etc") && Directory.Exists("/usr"))
            {
                OnLinux = true;
            }
            if(OnLinux) TemperatureReader = new LinuxCpuTemperatureReader();
            else TemperatureReader = new WindowsCpuTemperatureReader();
        }
        [HttpGet("temperature")]
        public IActionResult Get()
        {
            return Ok(TemperatureReader.GetCurrentTemperature());
        }
    }
}
