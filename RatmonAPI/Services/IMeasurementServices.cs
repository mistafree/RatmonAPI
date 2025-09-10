using RatmonAPI.Domain;
using System.Diagnostics.Metrics;
using System.Text.Json;

namespace RatmonAPI.Services
{
    public interface IMeasurementServices
    {
        Task<Measurement> AddMeasurementAsync(Guid deviceId, JsonElement measurementJson);
        Task<IEnumerable<Measurement>> GetMeasurementsAsync(Guid deviceId, DateTime from, DateTime to);
    }
}
