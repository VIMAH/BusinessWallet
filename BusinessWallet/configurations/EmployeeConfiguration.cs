using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusinessWallet.models;

namespace BusinessWallet.configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            // bestaande configuratie …

            // computed column (persisted) voor FullName
            builder.Property(e => e.FullName)
                   .HasComputedColumnSql(
                       "[FirstName] + CASE WHEN [Voorvoegsel] IS NULL OR [Voorvoegsel] = '' " +
                       "THEN ' ' ELSE ' ' + [Voorvoegsel] + ' ' END + [LastName]",
                       stored: true);

            builder.Property(e => e.Voorletters).HasMaxLength(20);
            builder.Property(e => e.Voorvoegsel).HasMaxLength(30);
            builder.Property(e => e.BirthCountry).HasMaxLength(60);
        }
    }
}
