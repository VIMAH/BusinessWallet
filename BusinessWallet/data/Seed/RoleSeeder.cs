using BusinessWallet.models;
using BusinessWallet.models.Enums;
using Microsoft.EntityFrameworkCore;

namespace BusinessWallet.data.Seed
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAsync(DataContext context)
        {
            if (!await context.Roles.AnyAsync())
            {
                // 1️⃣ Maak Roles aan (zonder CanX)
                var adminRole = new Role
                {
                    Name = "Admin",
                    Description = "Administrator role",
                    IsSystemRole = true
                };

                var hrRole = new Role
                {
                    Name = "HR",
                    Description = "HR Employee role",
                    IsSystemRole = false
                };

                var developerRole = new Role
                {
                    Name = "Developer",
                    Description = "Developer Employee role",
                    IsSystemRole = false
                };

                var financeRole = new Role
                {
                    Name = "Finance",
                    Description = "Finance Employee role",
                    IsSystemRole = false
                };

                var roles = new List<Role> { adminRole, hrRole, developerRole, financeRole };
                context.Roles.AddRange(roles);

                await context.SaveChangesAsync();

                // 2️⃣ Maak PolicyRules aan per rol (vervangt CanX)
                var policies = new List<PolicyRule>
                {
                    // Admin policies: alles mag
                    new PolicyRule { Action = ActionTypeEnum.View, TargetType = "Credential", TargetValue = "*", ConditionJson = "{ \"Role\": \"Admin\" }", IsAllowed = true },
                    new PolicyRule { Action = ActionTypeEnum.Issue, TargetType = "Credential", TargetValue = "*", ConditionJson = "{ \"Role\": \"Admin\" }", IsAllowed = true },
                    new PolicyRule { Action = ActionTypeEnum.Verify, TargetType = "Credential", TargetValue = "*", ConditionJson = "{ \"Role\": \"Admin\" }", IsAllowed = true },
                    new PolicyRule { Action = ActionTypeEnum.Revoke, TargetType = "Credential", TargetValue = "*", ConditionJson = "{ \"Role\": \"Admin\" }", IsAllowed = true },

                    // HR policies
                    new PolicyRule { Action = ActionTypeEnum.View, TargetType = "Credential", TargetValue = "*", ConditionJson = "{ \"Role\": \"HR\" }", IsAllowed = true },

                    // Developer policies
                    new PolicyRule { Action = ActionTypeEnum.View, TargetType = "Credential", TargetValue = "*", ConditionJson = "{ \"Role\": \"Developer\" }", IsAllowed = true },

                    // Finance policies
                    new PolicyRule { Action = ActionTypeEnum.View, TargetType = "Credential", TargetValue = "*", ConditionJson = "{ \"Role\": \"Finance\" }", IsAllowed = true },
                    new PolicyRule { Action = ActionTypeEnum.Verify, TargetType = "Credential", TargetValue = "*", ConditionJson = "{ \"Role\": \"Finance\" }", IsAllowed = true }
                };

                context.PolicyRules.AddRange(policies);
                await context.SaveChangesAsync();
            }
        }
    }
}
