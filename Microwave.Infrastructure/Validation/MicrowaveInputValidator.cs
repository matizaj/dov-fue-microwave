using Microwave.Infrastructure.Configuration;

namespace Microwave.Infrastructure.Validation
{
    public class MicrowaveInputValidator : IInputValidator
    {
        private readonly HashSet<string> _validCommands;

        public MicrowaveInputValidator()
        {
            _validCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "open", "o",
                "close", "c",
                "start", "s",
                "status", "st",
                "help", "h", "?",
                "quit", "exit", "q"
            };
        }

        public ValidationResult ValidateCommand(string? input)
        {
            var sanitizedInput = input?.Trim();

            return input switch
            {
                null => ValidationResult.Failure("Input cant be null."),
                _ when string.IsNullOrWhiteSpace(input) => ValidationResult.Failure("Input cannot be empty."),
                _ when !_validCommands.Contains(sanitizedInput!) => ValidationResult.Failure($"Unknown command: '{sanitizedInput}'. Type 'help' for available commands."),
                _ => ValidationResult.Success(sanitizedInput)
            };
        }

        public ValidationResult ValidateConfiguration(MicrowaveConfiguration config)
        {
            return config switch
            {
                null => ValidationResult.Failure("Configuration cannot be null."),
                _ when config.DefaultHeatingTime <= 0 => ValidationResult.Failure("DefaultHeatingTime must be greater than 0."),
                _ when config.MaxHeatingTime <= 0 => ValidationResult.Failure("MaxHeatingTime must be greater than 0."),
                _ when config.DefaultHeatingTime > config.MaxHeatingTime => ValidationResult.Failure("DefaultHeatingTime cannot be greater than MaxHeatingTime."),
                _ => ValidationResult.Success()
            };
        }

        public IEnumerable<string> GetValidCommands()
        {
            return _validCommands.ToList();
        }
    }
}
