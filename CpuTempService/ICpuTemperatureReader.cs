namespace MediaControllerBackendServices
{
    internal interface ICpuTemperatureReader
    {
        CpuTemperature GetCurrentTemperature();
    }
}