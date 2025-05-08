// File: services/IAuthService.cs
using System.Security.Claims;
using System.Threading.Tasks;
using BusinessWallet.DTOs;

namespace BusinessWallet.services
{
    public interface IAuthService
    {
        Task<AuthResponseChallengeDto> CreateChallengeAsync(AuthRequestChallengeDto dto);
        Task<AuthResponseTokenDto> GenerateTokenAsync(AuthRequestTokenDto dto);
        Task<AuthResponseCredentialsDto> GetCredentialsAsync(ClaimsPrincipal user);
    }
}
