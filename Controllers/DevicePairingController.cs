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

        [HttpPost("verify")]
        public async Task<IActionResult> Verify()
        {
            if (!Request.Headers.TryGetValue("Authorization", out var authHeader))
                return Unauthorized(new { error = "Missing Authorization header." });

            var token = authHeader.ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase).Trim();

            if (string.IsNullOrWhiteSpace(token))
                return Unauthorized(new { error = "Invalid Authorization header." });

            var result = await _pairingService.VerifyTokenAsync(token);

            if (!result.IsSuccess)
                return Unauthorized(new { error = result.ErrorMessage });

            return Ok(new { userId = result.UserId });
        }
    }
}
