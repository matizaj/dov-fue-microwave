namespace DoverFueling.Core.Models
{
    public record MicrowaveStatusInfo(
        string Status,
        bool DoorOpen,
        bool LightOn,
        bool HeaterOn,
        int RemainingSeconds
    );
}
