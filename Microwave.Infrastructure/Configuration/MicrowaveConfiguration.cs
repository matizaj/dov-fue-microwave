namespace Microwave.Infrastructure.Configuration
{
    public class MicrowaveConfiguration
    {
        public int DefaultHeatingTime { get; set; } = 60;
        public int MaxHeatingTime { get; set; } = 3600;
    }
}
