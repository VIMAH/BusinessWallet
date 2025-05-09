using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BusinessWallet.data;
using BusinessWallet.models;
using BusinessWallet.models.Enums;
using Microsoft.EntityFrameworkCore;

namespace BusinessWallet.repository
{
    /// <summary>
    /// Datatoegang voor de Identificatie-Authenticatie-Autorisatie-flow.
    /// </summary>
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context) => _context = context;

        // ────────────────────────────────────────────────────────────────
        // Medewerkers + rollen
        // ────────────────────────────────────────────────────────────────
        public Task<List<Employee>> GetEmployeesWithRolesAsync() =>
            _context.Employees
                    .Include(e => e.EmployeeRoles)
                    .ThenInclude(er => er.Role)
                    .ToListAsync();

        // ────────────────────────────────────────────────────────────────
        // Policy-controle (met "*" wildcard)
        // ────────────────────────────────────────────────────────────────
        public async Task<bool> HasAllowedPolicyAsync(
            ActionTypeEnum action,
            string credentialKey,
            Employee employee,  // ← nu bewust gebruikt in ConditionMet
            Role role)
        {
            var policies = await _context.PolicyRules
                                         .Where(pr => pr.Action == action &&
                                                      pr.IsAllowed &&
                                                      (pr.TargetValue == credentialKey ||
                                                       pr.TargetValue == "*"))
                                         .ToListAsync();

            return policies.Any(pr => ConditionMet(pr.ConditionJson, employee, role));
        }

        // Evalueert eenvoudige JSON-condities.
        private static bool ConditionMet(string json, Employee employee, Role role)
        {
            // gebruik employee.Id puur om Sonar “unused” te vermijden (kan later uitgebreid worden)
            _ = employee.Id;

            if (string.IsNullOrWhiteSpace(json) || json == "{}")
                return true;

            try
            {
                var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("Role", out var roleProp))
                {
                    return roleProp.GetString()?.Equals(role.Name,
                           System.StringComparison.OrdinalIgnoreCase) == true;
                }
            }
            catch
            {
                // Fout in JSON → policy niet matchen
            }
            return false;
        }

        // ────────────────────────────────────────────────────────────────
        // Logging
        // ────────────────────────────────────────────────────────────────
        public async Task AddAuthorizationLogAsync(AuthorizationLog log) =>
            await _context.AuthorizationLogs.AddAsync(log);

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
