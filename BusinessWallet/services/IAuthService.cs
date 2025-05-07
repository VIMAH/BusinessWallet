using System.Threading.Tasks;
using BusinessWallet.DTOs;

namespace BusinessWallet.services
{
    public interface IAuthService
    {
        Task<AuthResponseChallengeDto> CreateChallengeAsync(AuthRequestChallengeDto dto);
        Task<AuthResponseValidateDto> ValidateChallengeAsync(AuthRequestValidateDto dto);
    }
}
