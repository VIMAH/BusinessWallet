// File: repository/PolicyRulesRepository.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessWallet.data;
using BusinessWallet.models;
using BusinessWallet.models.Enums;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BusinessWallet.repository
{
    public class PolicyRulesRepository : IPolicyRulesRepository
    {
        private readonly DataContext _context;

        public PolicyRulesRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PolicyRule>> GetAllPolicyRulesAsync()
        {
            return await _context.PolicyRules.ToListAsync();
        }

        public async Task<IEnumerable<PolicyRule>> GetPolicyRulesByActionTargetAsync(ActionTypeEnum action, string targetType, string targetValue)
        {
            return await _context.PolicyRules
                .Where(r => r.Action == action && r.TargetType == targetType && r.TargetValue == targetValue)
                .ToListAsync();
        }

        public async Task<IEnumerable<PolicyRule>> GetAllowedCredentialsAsync(Guid employeeId, Guid roleId)
        {
            var allRules = await _context.PolicyRules.ToListAsync();
            var matchingRules = new List<PolicyRule>();

            foreach (var rule in allRules)
            {
                try
                {
                    var condition = JsonSerializer.Deserialize<Dictionary<string, string>>(rule.ConditionJson);
                    if (condition != null && condition.TryGetValue("Role", out var requiredRole))
                    {
                        if (requiredRole == roleId.ToString())
                        {
                            matchingRules.Add(rule);
                        }
                    }
                }
                catch
                {
                    // Invalid JSON → skip rule
                }
            }

            return matchingRules;
        }
    }
}
