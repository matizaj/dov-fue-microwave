

namespace DoverFueling.Core.Entity
{
    public interface IMicrowaveOvenHWExtended : IMicrowaveOvenHW
    {
        void TurnOnLight();
        void TurnOffLight();
        bool IsLightOn { get; }
        bool IsHeaterOn { get; }
        void SimulateDoorOpen();
        void SimulateDoorClose();
        void SimulateStartButtonPress();
    }
}
