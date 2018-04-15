using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MediaControllerBackendServices
{
    class WindowsCpuTemperatureReader : ICpuTemperatureReader
    {
        public CpuTemperature GetCurrentTemperature()
        {
            return new CpuTemperature(-1.0);
        }
    }
}
