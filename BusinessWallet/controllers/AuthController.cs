using System;
using System.Threading.Tasks;
using BusinessWallet.DTOs;
using BusinessWallet.services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BusinessWallet.controllers
{
    /// <summary>
    /// REST-API voor de volledige Identificatie-Authenticatie-Autorisatie-flow.
    /// </summary>
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService,
                              ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        // ─────────────────────────────────────────────────────────────────────
        // POST /auth/challenge
        // ─────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Genereert een challenge voor de opgegeven URL.
        /// </summary>
        /// <response code="200">Challenge succesvol aangemaakt.</response>
        [HttpPost("challenge")]
        [ProducesResponseType(typeof(AuthChallengeResponseDto), 200)]
        public async Task<ActionResult<AuthChallengeResponseDto>> CreateChallenge(
            [FromBody] AuthChallengeRequestDto request)
        {
            var response = await _authService.CreateChallengeAsync(request);
            return Ok(response);
        }

        // ─────────────────────────────────────────────────────────────────────
        // POST /auth/validate
        // ─────────────────────────────────────────────────────────────────────
        /// <summary>
        /// Valideert handtekening & policies en geeft autorisatie-uitkomst.
        /// </summary>
        /// <response code="200">Validatie uitgevoerd.</response>
        [HttpPost("validate")]
        [ProducesResponseType(typeof(AuthValidateResponseDto), 200)]
        public async Task<ActionResult<AuthValidateResponseDto>> Validate(
            [FromBody] AuthValidateRequestDto request)
        {
            var response = await _authService.ValidateAsync(request);
            return Ok(response);
        }
    }
}
