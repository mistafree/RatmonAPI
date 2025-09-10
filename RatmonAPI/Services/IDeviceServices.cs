using RatmonAPI.Domain;

namespace RatmonAPI.Services
{
    public interface IDeviceServices
    {
        Task<Device?> GetDeviceAsync(Guid id);
        Task UpdateConfigurationAsync(Guid deviceId, DeviceConfiguration config);
        Task AddDeviceAsync(Device device);
    }
}
