using Microsoft.EntityFrameworkCore;
using RatmonAPI.Data;
using RatmonAPI.Domain;
using System.Text.Json;

namespace RatmonAPI.Services
{
    public class MeasurementService : IMeasurementServices
    {
        private readonly AppDbContext _db;
        private readonly IDeviceServices _deviceService;

        public MeasurementService(AppDbContext db, IDeviceServices deviceService)
        {
            _db = db;
            _deviceService = deviceService;
        }

        public async Task<Measurement> AddMeasurementAsync(Guid deviceId, JsonElement measurementJson)
        {
      
            var device = await _deviceService.GetDeviceAsync(deviceId);
            if (device == null)
                throw new ArgumentException("Device not found");

            //validation json for devicetype
            if (!IsValidJson(measurementJson, device.Type))
                throw new ArgumentException($"Invalid measurement JSON for device type {device.Type}");

            // deserial for device type
            Measurement measurement = device.Type switch
            {
                DeviceType.Mas2 => JsonSerializer.Deserialize<Mas2Measurement>(measurementJson.GetRawText())!,
                DeviceType.Mouse2 => JsonSerializer.Deserialize<Mouse2Measurement>(measurementJson.GetRawText())!,
                DeviceType.Mouse2B => JsonSerializer.Deserialize<Mouse2BMeasurement>(measurementJson.GetRawText())!,
                DeviceType.MouseCombo => JsonSerializer.Deserialize<MouseComboMeasurement>(measurementJson.GetRawText())!,
                _ => throw new ArgumentException("Unknown device type")
            };

            measurement.DeviceId = deviceId;

            // 4. Zapis do bazy
            _db.Measurements.Add(measurement);
            await _db.SaveChangesAsync();

            return measurement;
        }

        public async Task<IEnumerable<Measurement>> GetMeasurementsAsync(Guid deviceId, DateTime from, DateTime to)
        {

            return await _db.Measurements
                .Where(m => m.DeviceId == deviceId && m.Timestamp >= from && m.Timestamp <= to)
                .ToListAsync();
            //return await _db.Measurements.ToListAsync();
        }
        private bool IsValidJson(JsonElement json, DeviceType type) => type switch
        {
            DeviceType.Mas2 => json.TryGetProperty("temperature", out _) && json.TryGetProperty("humidity", out _),
            DeviceType.Mouse2 => json.TryGetProperty("voltage", out _) && json.TryGetProperty("resistance", out _),
            DeviceType.Mouse2B => json.TryGetProperty("voltage", out _) && json.TryGetProperty("resistance", out _) && json.TryGetProperty("leakLocation", out _),
            DeviceType.MouseCombo => json.TryGetProperty("voltage", out _) && json.TryGetProperty("resistance", out _) && json.TryGetProperty("reflectograms", out _),
            _ => false
        };
    }
}
