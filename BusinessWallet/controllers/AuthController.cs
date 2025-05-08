// File: controllers/AuthController.cs
using System.Security.Claims;
using System.Threading.Tasks;
using BusinessWallet.DTOs;
using BusinessWallet.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessWallet.controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Endpoint om een challenge aan te maken.
        /// POST /auth/challenge
        /// </summary>
        [HttpPost("challenge")]
        public async Task<ActionResult<AuthResponseChallengeDto>> CreateChallenge([FromBody] AuthRequestChallengeDto dto)
        {
            var response = await _authService.CreateChallengeAsync(dto);
            return Ok(response);
        }

        /// <summary>
        /// Endpoint om een access token op te halen door een gesigneerde challenge te valideren.
        /// POST /auth/token
        /// </summary>
        [HttpPost("token")]
        public async Task<ActionResult<AuthResponseTokenDto>> GenerateToken([FromBody] AuthRequestTokenDto dto)
        {
            var response = await _authService.GenerateTokenAsync(dto);
            return Ok(response);
        }

        /// <summary>
        /// Endpoint om credentials (claims) op te halen.
        /// GET /auth/credentials
        /// Authorization: Bearer {access_token}
        /// </summary>
        [Authorize]
        [HttpGet("credentials")]
        public async Task<ActionResult<AuthResponseCredentialsDto>> GetCredentials()
        {
            var response = await _authService.GetCredentialsAsync(User);
            return Ok(response);
        }
    }
}
