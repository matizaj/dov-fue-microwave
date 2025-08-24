using DoverFueling.Core.Models;
using Microwave.Infrastructure.Configuration;

namespace DoverFueling.UI.Console
{
    public class UserNotification : IUserNotification
    {
        private readonly MicrowaveConfiguration _config;

        public UserNotification(MicrowaveConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }
        public void DisplayWelcome()
        {
            System.Console.WriteLine("Welcome to the Microwave Simulator!\n");
        }

        public void DisplayGoodbye()
        {
            System.Console.WriteLine("Thank you for using the Microwave Simulator!");
        }

        public void ShowInstructions()
        {
            System.Console.WriteLine("Available Commands:");
            System.Console.WriteLine("\topen  (o)  - Open the microwave door");
            System.Console.WriteLine("\tclose (c)  - Close the microwave door");
            System.Console.WriteLine("\tstart (s)  - Press the START button");
            System.Console.WriteLine("\tstatus(st) - Show current status");
            System.Console.WriteLine("\thelp  (h)  - Show this help message");
            System.Console.WriteLine("\tquit  (q)  - Exit the application");
            System.Console.WriteLine();
        }

        public void ShowStatus(MicrowaveStatusInfo statusInfo)
        {
            System.Console.WriteLine("\nCurrent Status:");
            System.Console.WriteLine($"\t{statusInfo.Status}");

            if (statusInfo.RemainingSeconds > 0)
            {
                int minutes = statusInfo.RemainingSeconds / 60;
                int seconds = statusInfo.RemainingSeconds % 60;
            }
        }

        public string? GetUserInput()
        {
            System.Console.Write("\nEnter command: ");
            return System.Console.ReadLine()?.Trim();
        }

        public void ShowUnknownCommandError(string command)
        {
            System.Console.WriteLine($"\nUnknown command: '{command}'");
            System.Console.WriteLine("Type 'help' to see available commands.");
        }

        public void ShowValidationError(string errorMessage)
        {
            System.Console.WriteLine($"\nValidation Error: {errorMessage}");
            System.Console.WriteLine("Please try again with valid input.");
        }

        public void ShowConfigurationError(string errorMessage)
        {
            System.Console.WriteLine($"\nConfiguration Error: {errorMessage}");
            System.Console.WriteLine("Please check your appsettings.json file.");
        }

        public void ShowOpeningDoorMessage()
        {
            System.Console.WriteLine("\nOpening microwave door...");
        }

        public void ShowClosingDoorMessage()
        {
            System.Console.WriteLine("\nClosing microwave door...");
        }

        public void ShowStartButtonMessage()
        {
            System.Console.WriteLine("\nPressing START button...");
        }

        public void ShowShutdownMessage()
        {
            System.Console.WriteLine("\nShutting down microwave...");
        }

        public void ShowMaxHeatingTimeWarning()
        {
            int maxHeatingTime = _config.MaxHeatingTime;
            int minutes = maxHeatingTime / 60;
            int seconds = maxHeatingTime % 60;
            System.Console.WriteLine($"\nWARNING: Maximum heating time limit reached!");
            System.Console.WriteLine($"\tCannot exceed {minutes:D2}:{seconds:D2} ({maxHeatingTime} seconds) for safety reasons.");
            System.Console.WriteLine("\tPlease wait for current cycle to complete before adding more time.");
        }
    }
}
