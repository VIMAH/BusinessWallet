using System.Threading.Tasks;
using BusinessWallet.DTOs;

namespace BusinessWallet.services
{
    /// <summary>
    /// Biedt de core-operaties voor de Identificatie-Authenticatie-Autorisatie-flow.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Genereert een challenge voor de opgegeven URL.
        /// </summary>
        Task<AuthChallengeResponseDto> CreateChallengeAsync(AuthChallengeRequestDto request);

        /// <summary>
        /// Valideert een handtekening en voert autorisatiecontrole uit.
        /// </summary>
        Task<AuthValidateResponseDto> ValidateAsync(AuthValidateRequestDto request);
    }
}
