using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CpuTempService
{
    class WindowsCpuTemperatureReader : ICpuTemperatureReader
    {
        public CpuTemperature GetCurrentTemperature()
        {
            return new CpuTemperature(-1.0);
        }
    }
}
