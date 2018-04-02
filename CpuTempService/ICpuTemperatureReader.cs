namespace CpuTempService
{
    internal interface ICpuTemperatureReader
    {
        CpuTemperature GetCurrentTemperature();
    }
}