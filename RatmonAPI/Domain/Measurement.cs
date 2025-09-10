using System.Text.Json.Serialization;

namespace RatmonAPI.Domain
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(Mouse2Measurement), "Mouse2")]
    [JsonDerivedType(typeof(Mouse2BMeasurement), "Mouse2B")]
    [JsonDerivedType(typeof(MouseComboMeasurement), "MouseCombo")]
    [JsonDerivedType(typeof(Mas2Measurement), "Mas2")]
    public abstract class Measurement
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid DeviceId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class Mouse2Measurement : Measurement
    {
        public float Voltage { get; set; }
        public float Resistance { get; set; }
    }

    public class Mouse2BMeasurement : Mouse2Measurement
    {
        public float LeakLocation { get; set; }
    }

    public class MouseComboMeasurement : Mouse2Measurement
    {
        public List<Reflectogram> Reflectograms { get; set; } = new();
    }

    public class Mas2Measurement : Measurement
    {
        public float Temperature { get; set; }
        public float Humidity { get; set; }
    }
    public class Reflectogram
    {
        public int SerialNumber { get; set; }
        public byte[] Data { get; set; } = Array.Empty<byte>();
    }
}
