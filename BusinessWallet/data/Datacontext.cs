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
        public DbSet<PolicyRule> PolicyRules => Set<PolicyRule>();
        public DbSet<AuthorizationLog> AuthorizationLogs => Set<AuthorizationLog>();

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

            // ◼︎ PolicyRule: ActionTypeEnum opslaan als string
            builder.Entity<PolicyRule>()
                .Property(p => p.Action)
                .HasConversion<string>();  // Enum als string opslaan

            // ◼︎ EmployeeRole: composite PK + relaties
            builder.Entity<EmployeeRole>()
                .HasKey(er => new { er.EmployeeId, er.RoleId });

            builder.Entity<EmployeeRole>()
                .HasOne(er => er.Employee)
                .WithMany(e => e.EmployeeRoles)
                .HasForeignKey(er => er.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<EmployeeRole>()
                .HasOne(er => er.Role)
                .WithMany(r => r.EmployeeRoles)
                .HasForeignKey(er => er.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // ◼︎ AuthorizationLog: index voor zoekopties (optioneel)
            builder.Entity<AuthorizationLog>()
                .HasIndex(l => new { l.EmployeeId, l.RoleId, l.RequestedBy, l.CreatedAt });

            // ◼︎ Apply EntityTypeConfigurations (indien van toepassing)
            builder.ApplyConfiguration(new EmployeeConfiguration());
            // builder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        }
    }
}
