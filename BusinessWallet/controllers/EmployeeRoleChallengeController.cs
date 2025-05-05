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
    public class EmployeeRoleChallengeController : ControllerBase
    {
        private readonly IEmployeeRoleChallengeService _challengeService;
        private readonly ILogger<EmployeeRoleChallengeController> _logger;

        public EmployeeRoleChallengeController(
            IEmployeeRoleChallengeService challengeService,
            ILogger<EmployeeRoleChallengeController> logger)
        {
            _challengeService = challengeService;
            _logger = logger;
        }

        /// <summary>
        /// Genereer een nieuwe challenge voor een medewerker + rol.
        /// </summary>
        [HttpPost("challenge")]
        public async Task<IActionResult> CreateChallenge([FromBody] ChallengeRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _challengeService.CreateChallengeAsync(dto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Challenge aanvraag mislukt.");
                return NotFound(new { message = ex.Message });
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
        public async Task<IActionResult> ValidateChallenge([FromBody] ChallengeValidationRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _challengeService.ValidateChallengeAsync(dto);
                if (result.IsValid)
                {
                    return Ok(result);
                }
                else
                {
                    return Unauthorized(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij validatie van de challenge.");
                return StatusCode(500, new { message = "Interne serverfout." });
            }
        }
    }
}
