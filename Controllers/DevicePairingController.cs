using Microsoft.AspNetCore.Mvc;
using ActivityManagementApp.Models.Api;
using ActivityManagementApp.Services;

namespace ActivityManagementApp.Controllers
{
    [ApiController]
    [Route("api/device/pair")]
    public class DevicePairingController : ControllerBase
    {
        private readonly DevicePairingService _pairingService;

        public DevicePairingController(DevicePairingService devicePairingService)
        {
            _pairingService = devicePairingService;
        }

        [HttpPost("activate")]
        public async Task<IActionResult> Activate([FromBody] ActivateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Code))
                return BadRequest(new { error = "Code is required." });

            var result = await _pairingService.ActivateAsync(request);

            if (!result.IsSuccess)
                return BadRequest(new { erro = result.ErrorMessage });

            return Ok(new { Token = result.Token });
        }
    }
}
