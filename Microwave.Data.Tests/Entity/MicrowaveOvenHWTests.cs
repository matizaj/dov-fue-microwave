using Xunit;
using DoverFueling.Core.Entity;

namespace Microwave.Data.Tests.Entity
{
    public class MicrowaveOvenHWTests
    {
        private readonly MicrowaveOvenHW _microwave;

        public MicrowaveOvenHWTests()
        {
            _microwave = new MicrowaveOvenHW();
        }

        [Fact]
        public void TurnOnHeaterShouldSetHeaterToOn()
        {
            _microwave.TurnOnHeater();
            Assert.True(_microwave.IsHeaterOn);
        }

        [Fact]
        public void TurnOffHeaterShouldSetHeaterToOff()
        {
            _microwave.TurnOnHeater();

            _microwave.TurnOffHeater();

            Assert.False(_microwave.IsHeaterOn);
        }

        [Fact]
        public void TurnOnLightShouldSetLightToOn()
        {
            _microwave.TurnOnLight();

            Assert.True(_microwave.IsLightOn);
        }

        [Fact]
        public void TurnOffLightShouldSetLightToOff()
        {
            _microwave.TurnOnLight();

            _microwave.TurnOffLight();

            Assert.False(_microwave.IsLightOn);
        }

        [Fact]
        public void SimulateDoorOpenShouldSetDoorToOpen()
        {
            _microwave.SimulateDoorOpen();

            Assert.True(_microwave.DoorOpen);
        }

        [Fact]
        public void SimulateDoorCloseShouldSetDoorToClosed()
        {
            _microwave.SimulateDoorOpen();

            _microwave.SimulateDoorClose();

            Assert.False(_microwave.DoorOpen);
        }

        [Fact]
        public void SimulateDoorOpenShouldRaiseDoorOpenChangedEvent()
        {
            bool evt = false;
            bool door = false;
            _microwave.DoorOpenChanged += (isOpen) => 
            {
                evt = true;
                door = isOpen;
            };

            _microwave.SimulateDoorOpen();

            Assert.True(evt);
            Assert.True(door);
        }


        [Fact]
        public void SimulateStartButtonPressShouldRaiseStartButtonPressedEvent()
        {
            bool evt = false;
            object? sender = null;
            EventArgs? args = null;
            _microwave.StartButtonPressed += (s, e) => 
            {
                evt = true;
                sender = s;
                args = e;
            };

            _microwave.SimulateStartButtonPress();

            Assert.True(evt);
            Assert.Same(_microwave, sender);
            Assert.Equal(EventArgs.Empty, args);
        }

        [Fact]
        public void SimulateStartButtonPressShouldWorkMultipleTimes()
        {
            int eventCount = 0;
            _microwave.StartButtonPressed += (s, e) => eventCount++;

            _microwave.SimulateStartButtonPress();
            _microwave.SimulateStartButtonPress();
            _microwave.SimulateStartButtonPress();

            Assert.Equal(3, eventCount);
        }

    }
}
