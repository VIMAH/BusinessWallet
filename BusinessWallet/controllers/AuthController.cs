using System;
using System.Threading.Tasks;
using BusinessWallet.DTOs;
using BusinessWallet.services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BusinessWallet.controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Genereer een nieuwe challenge voor authenticatie.
        /// </summary>
        [HttpPost("challenge")]
        public async Task<IActionResult> CreateChallenge([FromBody] AuthRequestChallengeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _authService.CreateChallengeAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij het aanmaken van de challenge.");
                return StatusCode(500, new { message = "Interne serverfout." });
            }
        }

        /// <summary>
        /// Valideer een gesigneerde challenge.
        /// </summary>
        [HttpPost("validate")]
        public async Task<IActionResult> ValidateChallenge([FromBody] AuthRequestValidateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _authService.ValidateChallengeAsync(dto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Challenge validatie mislukt: {Message}", ex.Message);
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij validatie van de challenge.");
                return StatusCode(500, new { message = "Interne serverfout." });
            }
        }
    }
}
