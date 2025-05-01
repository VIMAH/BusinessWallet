using BusinessWallet.models;
using Microsoft.EntityFrameworkCore;

namespace BusinessWallet.data.Seed
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAsync(DataContext context)
        {
            if (!await context.Roles.AnyAsync())
            {
                var roles = new List<Role>
                {
                    new Role
                    {
                        Name = "Admin",
                        Description = "Administrator role",
                        CanStore = true,
                        CanView = true,
                        CanReceive = true,
                        CanPresent = true,
                        CanIssue = true,
                        CanVerify = true,
                        CanRevoke = true,
                        IsSystemRole = true
                    },
                    new Role
                    {
                        Name = "HR",
                        Description = "HR Employee role",
                        CanView = true,
                        CanReceive = true,
                        CanPresent = true
                    },
                    new Role
                    {
                        Name = "Developer",
                        Description = "Developer Employee role",
                        CanStore = true,
                        CanView = true,
                        CanPresent = true
                    },
                    new Role
                    {
                        Name = "Finance",
                        Description = "Finance Employee role",
                        CanView = true,
                        CanVerify = true
                    }
                };

                context.Roles.AddRange(roles);
                await context.SaveChangesAsync();
            }
        }
    }
}
