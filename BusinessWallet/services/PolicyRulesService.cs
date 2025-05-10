using BusinessWallet.DTOs;
using BusinessWallet.models;
using BusinessWallet.repository;

namespace BusinessWallet.services
{
    public class PolicyRulesService : IPolicyRulesService
    {
        private readonly IPolicyRulesRepository _repository;

        public PolicyRulesService(IPolicyRulesRepository repository)
        {
            _repository = repository;
        }

        public async Task<PolicyRuleResponseDto?> GetByIdAsync(Guid id)
        {
            var policyRule = await _repository.GetByIdAsync(id);
            if (policyRule == null) return null;

            return MapToResponseDto(policyRule);
        }

        public async Task<PolicyRuleResponseDto> CreateAsync(CreatePolicyRuleDto dto)
        {
            var policyRule = new PolicyRule
            {
                Action = dto.Action,
                TargetType = dto.TargetType,
                TargetValue = dto.TargetValue,
                ConditionJson = dto.ConditionJson,
                IsAllowed = dto.IsAllowed,
                Message = dto.Message,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = dto.UpdatedAt ?? DateTime.UtcNow
            };

            await _repository.AddAsync(policyRule);  // ✅ Async gebruiken
            await _repository.SaveChangesAsync();

            return MapToResponseDto(policyRule);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var policyRule = await _repository.GetByIdAsync(id);
            if (policyRule == null) return false;

            await _repository.DeleteAsync(policyRule);  // ✅ Async gebruiken
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<PolicyRuleResponseDto?> UpdateAsync(Guid id, UpdatePolicyRuleDto dto)
        {
            var policyRule = await _repository.GetByIdAsync(id);
            if (policyRule == null) return null;

            if (dto.Action.HasValue) policyRule.Action = dto.Action.Value;
            if (!string.IsNullOrEmpty(dto.TargetType)) policyRule.TargetType = dto.TargetType;
            if (!string.IsNullOrEmpty(dto.TargetValue)) policyRule.TargetValue = dto.TargetValue;
            if (!string.IsNullOrEmpty(dto.ConditionJson)) policyRule.ConditionJson = dto.ConditionJson;
            if (dto.IsAllowed.HasValue) policyRule.IsAllowed = dto.IsAllowed.Value;
            if (!string.IsNullOrEmpty(dto.Message)) policyRule.Message = dto.Message;
            policyRule.UpdatedAt = dto.UpdatedAt ?? DateTime.UtcNow;

            await _repository.UpdateAsync(policyRule);  // ✅ Async gebruiken
            await _repository.SaveChangesAsync();

            return MapToResponseDto(policyRule);
        }

        private PolicyRuleResponseDto MapToResponseDto(PolicyRule policyRule)
        {
            return new PolicyRuleResponseDto
            {
                Id = policyRule.Id,
                Action = policyRule.Action,
                TargetType = policyRule.TargetType,
                TargetValue = policyRule.TargetValue,
                ConditionJson = policyRule.ConditionJson,
                IsAllowed = policyRule.IsAllowed,
                Message = policyRule.Message,
                CreatedAt = policyRule.CreatedAt,
                UpdatedAt = policyRule.UpdatedAt
            };
        }
    }
}
