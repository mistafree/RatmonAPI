public class DeviceConfiguration
{
    public string Name { get; set; } = string.Empty;

    public float? AlarmThreshold { get; set; }         
    public float? CableLength { get; set; }             
    public float? TempAlarmThreshold { get; set; }      
    public float? HumidityAlarmThreshold { get; set; }  
}