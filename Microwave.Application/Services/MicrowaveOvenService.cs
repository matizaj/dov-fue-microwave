using DoverFueling.Core.Entity;
using Microwave.Infrastructure.Configuration;

namespace Microwave.Application.Services
{
    public class MicrowaveOvenService : IMicrowaveOvenService
    {
        private readonly IMicrowaveOvenHWExtended _device;
        private readonly MicrowaveConfiguration _config;
        private int _remainingSeconds = 0;
        private CancellationTokenSource? _heaterToken;
        private Task? _heaterTask;



        public event Action? MaxHeatingTimeReached;
        public bool DoorOpen => _device.DoorOpen;
        public bool LightOn => _device.IsLightOn;
        public bool IsHeaterOn => _device.IsHeaterOn;
        public int RemainingSeconds => _remainingSeconds;

        public MicrowaveOvenService(IMicrowaveOvenHWExtended hardware, MicrowaveConfiguration config)
        {
            _device = hardware ?? throw new ArgumentNullException(nameof(hardware));
            _config = config ?? throw new ArgumentNullException(nameof(config));

            _device.DoorOpenChanged += OnDoorOpenChanged;
            _device.StartButtonPressed += OnStartButtonPressed;
        }

        public void OpenDoor()
        {
            _device.SimulateDoorOpen();
        }

        public void CloseDoor()
        {
            _device.SimulateDoorClose();
        }

        public void PressStartButton()
        {
            _device.SimulateStartButtonPress();
        }

        private void OnDoorOpenChanged(bool isOpen)
        {
            if (isOpen)
            {
                // Door opened - turn on light and stop heater
                _device.TurnOnLight();
                if (_device.IsHeaterOn)
                {
                    StopHeating();
                }
            }
            else
            {
                // Door closed - turn off light
                _device.TurnOffLight();
            }
        }

        private async void OnStartButtonPressed(object? sender, EventArgs e)
        {
            if (_device.DoorOpen)
            {
                Console.WriteLine("Controller: Start button pressed but door is open - nothing happens");
                return;
            }

            if (_device.IsHeaterOn)
            {
                var newTime = _remainingSeconds + _config.DefaultHeatingTime;
                if (newTime > _config.MaxHeatingTime)
                {
                    MaxHeatingTimeReached?.Invoke();
                    return;
                }

                _remainingSeconds = newTime;
                Console.WriteLine($"Controller: Start button pressed - increased remaining time to {_remainingSeconds} seconds");
            }
            else
            {
                await StartHeating(_config.DefaultHeatingTime);
            }
        }

        private async Task StartHeating(int seconds)
        {
            if (_device.IsHeaterOn)
            {
                Console.WriteLine("Controller: Heater is already running");
                return;
            }

            _remainingSeconds = seconds;
            _heaterToken = new CancellationTokenSource();

            Console.WriteLine($"Controller: Starting heater for {seconds} seconds");
            _device.TurnOnHeater();

            _heaterTask = RunHeatingTimer();
            await _heaterTask;
        }

        private async Task RunHeatingTimer()
        {
            try
            {
                while (_remainingSeconds > 0 && !_heaterToken!.Token.IsCancellationRequested)
                {
                    await Task.Delay(1000, _heaterToken.Token); // Wait 1 second
                    _remainingSeconds--;
                    Console.WriteLine($"Controller: Heating... {_remainingSeconds} seconds remaining");// Log every second
                }

                if (_remainingSeconds <= 0)
                {
                    Console.WriteLine("Controller: Timer finished - turning off heater");
                    StopHeating();
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Controller: Heating operation was cancelled");
            }
        }



        private void StopHeating()
        {
            _device.TurnOffHeater();
            _heaterToken?.Cancel();
        }

        public string GetStatus()
        {
            return $"Door: {(_device.DoorOpen ? "Open" : "Closed")}, " +
                   $"Light: {(_device.IsLightOn ? "On" : "Off")}, " +
                   $"Heater: {(_device.IsHeaterOn ? "On" : "Off")}, " +
                   $"Remaining Time: {_remainingSeconds} seconds";
        }

        public void Dispose()
        {
            _heaterToken?.Cancel();
            _heaterToken?.Dispose();

            if (_device != null)
            {
                _device.DoorOpenChanged -= OnDoorOpenChanged;
                _device.StartButtonPressed -= OnStartButtonPressed;
            }
        }
    }
}
