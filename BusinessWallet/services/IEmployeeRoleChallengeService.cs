using System.Threading.Tasks;
using BusinessWallet.DTOs;

namespace BusinessWallet.services
{
    public interface IEmployeeRoleChallengeService
    {
        Task<ChallengeResponseDto> CreateChallengeAsync(ChallengeRequestDto dto);
        Task<ChallengeValidationResponseDto> ValidateChallengeAsync(ChallengeValidationRequestDto dto);
    }
}
