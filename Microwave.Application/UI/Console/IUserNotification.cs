using DoverFueling.Core.Models;

namespace DoverFueling.UI.Console
{
    public interface IUserNotification
    {
        void DisplayWelcome();
        void DisplayGoodbye();
        void ShowInstructions();
        void ShowStatus(MicrowaveStatusInfo statusInfo);
        string? GetUserInput();
        void ShowUnknownCommandError(string command);
        void ShowValidationError(string errorMessage);
        void ShowConfigurationError(string errorMessage);
        void ShowOpeningDoorMessage();
        void ShowClosingDoorMessage();
        void ShowStartButtonMessage();
        void ShowShutdownMessage();
        void ShowMaxHeatingTimeWarning();
    }
}
