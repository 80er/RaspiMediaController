using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;

namespace MediaControllerBackendServices
{
    public class FanController
    {
        private readonly IGpioPin _outputPin;

        public FanController()
        {
            _outputPin = Pi.Gpio[0];
            _outputPin.PinMode = GpioPinDriveMode.Output;
        }
        public void On()
        {
            if (!_outputPin.Read())
            {
                _outputPin.Write(true);
            }
        }

        public void Off()
        {
            if (_outputPin.Read())
            {
                _outputPin.Write(false);
            }
        }
    }
}
