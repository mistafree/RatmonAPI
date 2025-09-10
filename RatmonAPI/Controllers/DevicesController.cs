using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RatmonAPI.Domain;
using RatmonAPI.Services;

namespace RatmonAPI.Controllers;

[ApiController]
[Route("api/devices")]
public class DevicesController : ControllerBase
{
    private readonly IDeviceServices _service;

    public DevicesController(IDeviceServices service) => _service = service;

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDevice(Guid id)
    {
        var device = await _service.GetDeviceAsync(id);
        return device is null ? NotFound() : Ok(device);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}/configuration")]
    public async Task<IActionResult> UpdateConfig(Guid id, [FromBody] DeviceConfiguration config)
    {
        await _service.UpdateConfigurationAsync(id, config);
        return NoContent();
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateDevice([FromBody] Device device)
    {
        await _service.AddDeviceAsync(device);
        return CreatedAtAction(nameof(GetDevice), new { id = device.Id }, device);
    }

}
