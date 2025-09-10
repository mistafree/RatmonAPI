using RatmonAPI.Data;
using RatmonAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace RatmonAPI.Services
{
    public class DeviceService : IDeviceServices
    {
        private readonly AppDbContext _db;

        public DeviceService(AppDbContext db) => _db = db;

        public async Task<Device?> GetDeviceAsync(Guid id) =>
            await _db.Devices.Include(d => d.Configuration).FirstOrDefaultAsync(d => d.Id == id);

        public async Task UpdateConfigurationAsync(Guid deviceId, DeviceConfiguration config)
        {
            var device = await _db.Devices.Include(d => d.Configuration)
                                          .FirstOrDefaultAsync(d => d.Id == deviceId);
            if (device == null) throw new KeyNotFoundException();

            device.Configuration = config;
            await _db.SaveChangesAsync();
        }
        public async Task AddDeviceAsync(Device device)
        {
            _db.Devices.Add(device);
            await _db.SaveChangesAsync();
        }
    }
}
