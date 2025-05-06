using BusinessWallet.models;
using BusinessWallet.models.Enums;
using Microsoft.EntityFrameworkCore;
using BusinessWallet.configurations;

namespace BusinessWallet.data
{
       public class DataContext : DbContext
       {
              public DataContext(DbContextOptions<DataContext> options)
                  : base(options) { }

              // ───── DbSets ─────
              public DbSet<Employee> Employees => Set<Employee>();
              public DbSet<Role> Roles => Set<Role>();
              public DbSet<EmployeeRole> EmployeeRoles => Set<EmployeeRole>();
              public DbSet<EmployeeRoleChallenge> EmployeeRoleChallenges => Set<EmployeeRoleChallenge>();
              public DbSet<PolicyRule> PolicyRules => Set<PolicyRule>();
              public DbSet<AuthorizationLog> AuthorizationLogs => Set<AuthorizationLog>();

              protected override void OnModelCreating(ModelBuilder builder)
              {
                     base.OnModelCreating(builder);

                     // ◼︎ Enum-naar-string opslag + defaults
                     builder.Entity<Employee>()
                            .Property(e => e.VerificationState)
                            .HasConversion<string>()
                            .HasDefaultValue(models.Enums.VerificationState.Unverified);

                     builder.Entity<Employee>()
                            .Property(e => e.EmployeeState)
                            .HasConversion<string>()
                            .HasDefaultValue(models.Enums.EmployeeState.Inactive);

                     // ◼︎ PolicyRule: ActionTypeEnum opslaan als string
                     builder.Entity<PolicyRule>()
                            .Property(p => p.Action)
                            .HasConversion<string>();  // Zorgt dat de Enum als string in DB wordt opgeslagen

                     // ◼︎ Koppeltabel composite PK
                     builder.Entity<EmployeeRole>()
                            .HasKey(er => new { er.EmployeeId, er.RoleId });

                     builder.Entity<EmployeeRole>()
                            .HasOne(er => er.Employee);

                     builder.Entity<EmployeeRole>()
                            .HasOne(er => er.Role)
                            .WithMany(r => r.EmployeeRoles)
                            .HasForeignKey(er => er.RoleId);

                     builder.Entity<EmployeeRoleChallenge>()
                            .HasOne(c => c.Employee)
                            .WithMany()
                            .HasForeignKey(c => c.EmployeeId)
                            .OnDelete(DeleteBehavior.Cascade);

                     builder.Entity<EmployeeRoleChallenge>()
                            .HasOne(c => c.Role)
                            .WithMany()
                            .HasForeignKey(c => c.RoleId)
                            .OnDelete(DeleteBehavior.Cascade);

                     // ◼︎ Apply EntityTypeConfigurations (indien van toepassing)
                     builder.ApplyConfiguration(new EmployeeConfiguration());
                     // builder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
              }
       }
}
