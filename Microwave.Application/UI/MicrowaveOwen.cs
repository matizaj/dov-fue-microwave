using DoverFueling.Core.Models;
using DoverFueling.UI.Console;
using DoverFueling.UI.Strategies;
using Microwave.Application.Services;
using Microwave.Infrastructure.Validation;
using Serilog;

namespace DoverFueling.UI
{
    public class MicrowaveOwen : IDisposable
    {
        private readonly ILogger _log;
        private readonly IMicrowaveOvenService _controller;
        private readonly IUserNotification _console;
        private readonly UserInputStrategy _inputStrategy;
        private readonly IInputValidator _validator;
        private bool _isRunning;

        public MicrowaveOwen(ILogger log, IMicrowaveOvenService controller, IUserNotification console, IInputValidator validator)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _inputStrategy = new UserInputStrategy();
            _isRunning = false;

            _controller.MaxHeatingTimeReached += OnMaxHeatingTimeReached;

            InitializeCommandStrategies();
        }

        public void Start()
        {
            _log.Information($"{nameof(MicrowaveOwen)}: Start");
            _console.DisplayWelcome();
            _console.ShowInstructions();
            ShowStatus();

            _isRunning = true;

            while (_isRunning)
            {
                ProcessUserInput();
            }

            _console.DisplayGoodbye();
        }

        public void Stop()
        {
            _isRunning = false;
        }

        private void InitializeCommandStrategies()
        {
            _inputStrategy.RegisterCommands(new[] { "open", "o" }, HandleOpenDoor);
            _inputStrategy.RegisterCommands(new[] { "close", "c" }, HandleCloseDoor);
            _inputStrategy.RegisterCommands(new[] { "start", "s" }, HandleStartButton);
            _inputStrategy.RegisterCommands(new[] { "status", "st" }, () => { });
            _inputStrategy.RegisterCommands(new[] { "help", "h", "?" }, () => _console.ShowInstructions());
            _inputStrategy.RegisterCommands(new[] { "quit", "exit", "q" }, HandleQuit);
        }

        private void ProcessUserInput()
        {
            string? input = _console.GetUserInput();

            var validationResult = _validator.ValidateCommand(input);

            if (!validationResult.IsValid)
            {
                _log.Warning($"{nameof(MicrowaveOwen)}: Invalid input detected: {input}");
                _console.ShowValidationError(validationResult.ErrorMessage!);
                ShowStatusIfRunning();
                return;
            }

            var action = _inputStrategy.GetCommandAction(validationResult.SanitizedInput);

            if (action != null)
            {
                action.Invoke();
            }
            else
            {
                _console.ShowUnknownCommandError(validationResult.SanitizedInput);
            }

            ShowStatusIfRunning();
        }

        private void ShowStatusIfRunning()
        {
            if (_isRunning)
            {
                ShowStatus();
            }
        }

        private void HandleOpenDoor()
        {
            _log.Information($"{nameof(MicrowaveOwen)}: Door open.");
            _console.ShowOpeningDoorMessage();
            _controller.OpenDoor();
        }

        private void HandleCloseDoor()
        {
            _log.Information($"{nameof(MicrowaveOwen)}: Door closed.");
            _console.ShowClosingDoorMessage();
            _controller.CloseDoor();
        }

        private void HandleStartButton()
        {
            _log.Information($"{nameof(MicrowaveOwen)}: Start button pressed.");
            _console.ShowStartButtonMessage();
            _controller.PressStartButton();
        }

        private void HandleQuit()
        {
            _log.Information($"{nameof(MicrowaveOwen)}: Quit application.");
            _console.ShowShutdownMessage();
            _isRunning = false;
        }

        private void OnMaxHeatingTimeReached()
        {
            _log.Information($"{nameof(MicrowaveOwen)}: Maximum heating time reached.");
            _console.ShowMaxHeatingTimeWarning();
        }

        private void ShowStatus()
        {
            var statusInfo = new MicrowaveStatusInfo(
                _controller.GetStatus(),
                _controller.DoorOpen,
                _controller.LightOn,
                _controller.IsHeaterOn,
                _controller.RemainingSeconds
            );

            _console.ShowStatus(statusInfo);
        }

        public void Dispose()
        {
            if (_controller != null)
            {
                _controller.MaxHeatingTimeReached -= OnMaxHeatingTimeReached;
                _controller.Dispose();
            }
        }
    }
}
