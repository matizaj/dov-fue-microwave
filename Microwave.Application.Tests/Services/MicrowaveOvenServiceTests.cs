using Microwave.Application.Services;
using DoverFueling.Core.Entity;
using Microwave.Infrastructure.Configuration;
using Moq;
using Xunit;

namespace Microwave.Application.Tests.Services
{
    public class MicrowaveOvenServiceTests
    {
        private readonly Mock<IMicrowaveOvenHWExtended> _mockHardware;
        private readonly MicrowaveConfiguration _config;
        private readonly MicrowaveOvenService _service;

        public MicrowaveOvenServiceTests()
        {
            _mockHardware = new Mock<IMicrowaveOvenHWExtended>();
            _config = new MicrowaveConfiguration
            {
                DefaultHeatingTime = 30,
                MaxHeatingTime = 300
            };
            _service = new MicrowaveOvenService(_mockHardware.Object, _config);
        }

        [Fact]
        public void OpenDoorShouldCallHardwareOpenDoor()
        {
            _service.OpenDoor();
            _mockHardware.Verify(h => h.SimulateDoorOpen(), Times.Once);
        }

        [Fact]
        public void CloseDoorShouldCallHardwareCloseDoor()
        {
            _service.CloseDoor();
            _mockHardware.Verify(h => h.SimulateDoorClose(), Times.Once);
        }

        [Fact]
        public void PressStartButtonShouldCallHardwareStartButton()
        {
            _service.PressStartButton();
            _mockHardware.Verify(h => h.SimulateStartButtonPress(), Times.Once);
        }

        [Fact]
        public void DoorOpenShouldReturnHardwareDoorStatus()
        {
            _mockHardware.Setup(h => h.DoorOpen).Returns(true);
            var result = _service.DoorOpen;
            Assert.True(result);
        }

        [Fact]
        public void LightOnShouldReturnHardwareLightStatus()
        {
            _mockHardware.Setup(h => h.IsLightOn).Returns(true);
            var result = _service.LightOn;
            Assert.True(result);
        }

        [Fact]
        public void IsHeaterOnShouldReturnHardwareHeaterStatus()
        {
            _mockHardware.Setup(h => h.IsHeaterOn).Returns(true);
            var result = _service.IsHeaterOn;
            Assert.True(result);
        }
    }
}
