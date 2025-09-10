using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RatmonAPI.Domain;
using RatmonAPI.Services;
using System.Diagnostics.Metrics;
using System.Text.Json;

namespace RatmonAPI.Controllers;

[ApiController]
[Route("api/devices/{deviceId}/measurements")]
public class MeasurementsController : ControllerBase
{
    private readonly IMeasurementServices _measurementService;
    private readonly IDeviceServices _deviceService;

    public MeasurementsController(IMeasurementServices measurementService, IDeviceServices deviceService)
    {
        _measurementService = measurementService;
        _deviceService = deviceService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddMeasurement(Guid deviceId, [FromBody] JsonElement measurementJson)
    {
        var device = await _deviceService.GetDeviceAsync(deviceId);
        if (device == null) return NotFound("Device not found");

        Measurement measurement = device.Type switch        // deserialize for inherince depends of device type
        {
            DeviceType.Mouse2 => JsonSerializer.Deserialize<Mouse2Measurement>(measurementJson.GetRawText())!,
            DeviceType.Mouse2B => JsonSerializer.Deserialize<Mouse2BMeasurement>(measurementJson.GetRawText())!,
            DeviceType.MouseCombo => JsonSerializer.Deserialize<MouseComboMeasurement>(measurementJson.GetRawText())!,
            DeviceType.Mas2 => JsonSerializer.Deserialize<Mas2Measurement>(measurementJson.GetRawText())!,
            _ => throw new ArgumentException("Unknown device type")
        };

        measurement.DeviceId = deviceId;

        await _measurementService.AddMeasurementAsync(deviceId, measurementJson);

        return CreatedAtAction(nameof(GetMeasurements), new { deviceId }, measurement);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetMeasurements(Guid deviceId, DateTime from, DateTime to)
    {
        var device = await _deviceService.GetDeviceAsync(deviceId);
        if (device == null)
            return NotFound("Device not found");

       
        var deviceType = device.Type;

        var measurements = await _measurementService.GetMeasurementsAsync(deviceId, from, to);

        return Ok(measurements);
    }
}