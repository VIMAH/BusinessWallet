using BusinessWallet.DTOs;

namespace BusinessWallet.services
{
    public interface IPolicyRulesService
    {
        Task<PolicyRuleResponseDto?> GetByIdAsync(Guid id);
        Task<PolicyRuleResponseDto> CreateAsync(CreatePolicyRuleDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<PolicyRuleResponseDto?> UpdateAsync(Guid id, UpdatePolicyRuleDto dto);
    }
}
