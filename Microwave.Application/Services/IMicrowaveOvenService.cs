namespace Microwave.Application.Services
{
    public interface IMicrowaveOvenService : IDisposable
    {
        void OpenDoor();
        void CloseDoor();
        void PressStartButton();
        bool DoorOpen { get; }
        bool LightOn { get; }
        bool IsHeaterOn { get; }
        int RemainingSeconds { get; }
        string GetStatus();

        event Action MaxHeatingTimeReached;
    }
}
