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

              protected override void OnModelCreating(ModelBuilder builder)
              {
                     base.OnModelCreating(builder);

                     // ◼︎ Enum-naar-string opslag + defaults
                     builder.Entity<Employee>()
                            .Property(e => e.VerificationState)
                            .HasConversion<string>()
                            .HasDefaultValue(VerificationState.Unverified);

                     builder.Entity<Employee>()
                            .Property(e => e.EmployeeState)
                            .HasConversion<string>()
                            .HasDefaultValue(EmployeeState.Inactive);

                     // ◼︎ Koppeltabel composite PK
                     builder.Entity<EmployeeRole>()
                            .HasKey(er => new { er.EmployeeId, er.RoleId });

                     builder.Entity<EmployeeRole>()
                            .HasOne(er => er.Employee)
                            .WithMany(e => e.EmployeeRoles)
                            .HasForeignKey(er => er.EmployeeId);

                     builder.Entity<EmployeeRole>()
                            .HasOne(er => er.Role)
                            .WithMany(r => r.EmployeeRoles)
                            .HasForeignKey(er => er.RoleId);

                     // ◼︎ Apply EntityTypeConfigurations
                     builder.ApplyConfiguration(new EmployeeConfiguration());
                     // of gebruik: builder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
              }
       }
}
