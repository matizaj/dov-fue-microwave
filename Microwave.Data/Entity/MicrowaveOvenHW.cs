namespace DoverFueling.Core.Entity
{
    public class MicrowaveOvenHW : IMicrowaveOvenHWExtended
    {
        private bool _doorOpen = false;
        private bool _heaterOn = false;
        private bool _lightOn = false;

        public MicrowaveOvenHW()
        {
            DoorOpenChanged = delegate { };
            StartButtonPressed = delegate { };
        }

        public bool DoorOpen
        {
            get => _doorOpen;
            private set
            {
                if (_doorOpen != value)
                {
                    _doorOpen = value;
                    DoorOpenChanged?.Invoke(_doorOpen);
                }
            }
        }

        public bool IsHeaterOn => _heaterOn;
        public bool IsLightOn => _lightOn;

        public event Action<bool> DoorOpenChanged;
        public event EventHandler StartButtonPressed;
        public void TurnOnHeater()
        {
            _heaterOn = true;
            Console.WriteLine("Device: Microwave heater turned ON");
        }

        public void TurnOffHeater()
        {
            _heaterOn = false;
            Console.WriteLine("Device: Microwave heater turned OFF");
        }

        public void TurnOnLight()
        {
            _lightOn = true;
            Console.WriteLine("Device: Light turned ON");
        }

        public void TurnOffLight()
        {
            _lightOn = false;
            Console.WriteLine("Device: Light turned OFF");
        }

        public void SimulateDoorOpen()
        {
            DoorOpen = true;
            Console.WriteLine("Device: Door opened");
        }

        public void SimulateDoorClose()
        {
            DoorOpen = false;
            Console.WriteLine("Device: Door closed");
        }

        public void SimulateStartButtonPress()
        {
            StartButtonPressed?.Invoke(this, EventArgs.Empty);
            Console.WriteLine("Device: Start button pressed");
        }
    }
}
