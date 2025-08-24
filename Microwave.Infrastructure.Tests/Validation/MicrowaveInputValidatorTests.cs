using Xunit;
using Microwave.Infrastructure.Validation;
using Microwave.Infrastructure.Configuration;

namespace Microwave.Infrastructure.Tests.Validation
{
    public class MicrowaveInputValidatorTests
    {
        private readonly MicrowaveInputValidator _validator;

        public MicrowaveInputValidatorTests()
        {
            _validator = new MicrowaveInputValidator();
        }

        [Fact]
        public void ValidateCommandWithValidCommandsShouldReturnSuccess()
        {
            var validCommands = new[] { "open", "close", "start", "status", "help", "quit" };

            foreach (var command in validCommands)
            {
                var result = _validator.ValidateCommand(command);
                Assert.True(result.IsValid);
                Assert.Equal(command, result.SanitizedInput);
            }
        }

        [Fact]
        public void ValidateCommandWithValidShortCommandsShouldReturnSuccess()
        {
            var validCommands = new[] { "o", "c", "s", "st", "h", "q" };

            foreach (var command in validCommands)
            {
                var result = _validator.ValidateCommand(command);
                Assert.True(result.IsValid);
                Assert.Equal(command, result.SanitizedInput);
            }
        }

        [Fact]
        public void ValidateCommandWithNullInputShouldReturnFailure()
        {
            var result = _validator.ValidateCommand(null);

            Assert.False(result.IsValid);
            Assert.Equal("Input cant be null.", result.ErrorMessage);
        }

        [Fact]
        public void ValidateCommandWithEmptyInputShouldReturnFailure()
        {
            var result = _validator.ValidateCommand("");

            Assert.False(result.IsValid);
            Assert.Equal("Input cannot be empty.", result.ErrorMessage);
        }

        [Fact]
        public void ValidateCommandWithInvalidCommandShouldReturnFailure()
        {
            var result = _validator.ValidateCommand("invalid_cmd");

            Assert.False(result.IsValid);
            Assert.Contains("Unknown command", result.ErrorMessage);
        }

        [Fact]
        public void ValidateConfigurationWithValidConfigShouldReturnSuccess()
        {
            var config = new MicrowaveConfiguration
            {
                DefaultHeatingTime = 30,
                MaxHeatingTime = 60
            };

            var result = _validator.ValidateConfiguration(config);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void ValidateConfigurationWithNullConfigShouldReturnFailure()
        {
            var result = _validator.ValidateConfiguration(null);

            Assert.False(result.IsValid);
            Assert.Equal("Configuration cannot be null.", result.ErrorMessage);
        }

        [Fact]
        public void ValidateConfigurationWithInvalidDefaultTimeShouldReturnFailure()
        {
            var config = new MicrowaveConfiguration
            {
                DefaultHeatingTime = 0,
                MaxHeatingTime = 60
            };

            var result = _validator.ValidateConfiguration(config);

            Assert.False(result.IsValid);
            Assert.Contains("DefaultHeatingTime must be greater than 0", result.ErrorMessage);
        }

        [Fact]
        public void GetValidCommandsShouldReturnAllValidCommands()
        {
            var commands = _validator.GetValidCommands().ToList();

            Assert.Contains("open", commands);
            Assert.Contains("close", commands);
            Assert.Contains("start", commands);
            Assert.Contains("status", commands);
            Assert.Contains("help", commands);
            Assert.Contains("quit", commands);
        }
    }
}
