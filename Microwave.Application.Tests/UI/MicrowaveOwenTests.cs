using Microwave.Application.Services;
using Microwave.Infrastructure.Validation;
using DoverFueling.UI;
using DoverFueling.UI.Console;
using Moq;
using Serilog;
using Xunit;
using DoverFueling.Core.Models;

namespace Microwave.Application.Tests.UI
{
    public class MicrowaveOwenTests
    {
        private readonly Mock<ILogger> _mockLogger;
        private readonly Mock<IMicrowaveOvenService> _mockService;
        private readonly Mock<IUserNotification> _mockConsole;
        private readonly Mock<IInputValidator> _mockValidator;
        private readonly MicrowaveOwen _microwave;

        public MicrowaveOwenTests()
        {
            _mockLogger = new Mock<ILogger>();
            _mockService = new Mock<IMicrowaveOvenService>();
            _mockConsole = new Mock<IUserNotification>();
            _mockValidator = new Mock<IInputValidator>();

            _microwave = new MicrowaveOwen(_mockLogger.Object, _mockService.Object, _mockConsole.Object, _mockValidator.Object);
        }

        [Fact]
        public void ConstructorShouldSubscribeToServiceEvents()
        {
            _mockService.VerifyAdd(s => s.MaxHeatingTimeReached += It.IsAny<Action>(), Times.Once);
        }

        [Fact]
        public void DisposeShouldUnsubscribeFromServiceEvents()
        {
            _microwave.Dispose();
            _mockService.VerifyRemove(s => s.MaxHeatingTimeReached -= It.IsAny<Action>(), Times.Once);
        }

        [Fact]
        public void StartShouldDisplayWelcomeInstructionsAndExitOnQuit()
        {
            _mockConsole.Setup(c => c.GetUserInput()).Returns("quit");
            _mockValidator.Setup(v => v.ValidateCommand("quit"))
                .Returns(new ValidationResult { IsValid = true, SanitizedInput = "quit" });

            _microwave.Start();

            _mockConsole.Verify(c => c.DisplayWelcome(), Times.Once);
            _mockConsole.Verify(c => c.ShowInstructions(), Times.Once);
            _mockService.Verify(s => s.GetStatus(), Times.AtLeastOnce);
            _mockConsole.Verify(c => c.DisplayGoodbye(), Times.Once);
        }
    }
}
