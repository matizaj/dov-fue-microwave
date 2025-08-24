using Microwave.Infrastructure.Configuration;

namespace Microwave.Infrastructure.Validation
{
    public interface IInputValidator
    {
        ValidationResult ValidateCommand(string? input);
        ValidationResult ValidateConfiguration(MicrowaveConfiguration config);
        IEnumerable<string> GetValidCommands();
    }
}
