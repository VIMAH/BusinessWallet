// File: repository/IPolicyRulesRepository.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessWallet.models;
using BusinessWallet.models.Enums;

namespace BusinessWallet.repository
{
    public interface IPolicyRulesRepository
    {
        Task<IEnumerable<PolicyRule>> GetAllPolicyRulesAsync();
        Task<IEnumerable<PolicyRule>> GetPolicyRulesByActionTargetAsync(ActionTypeEnum action, string targetType, string targetValue);
        Task<IEnumerable<PolicyRule>> GetAllowedCredentialsAsync(Guid employeeId, Guid roleId);  // ✅ toegevoegd
    }
}
