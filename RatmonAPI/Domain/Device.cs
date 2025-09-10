namespace RatmonAPI.Domain
{
    public class Device
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public DeviceType Type { get; set; } 
        public DeviceConfiguration Configuration { get; set; } = default!;
    }

    public enum DeviceType
    {
        Mouse2,
        Mouse2B,
        MouseCombo,
        Mas2
    }
}
