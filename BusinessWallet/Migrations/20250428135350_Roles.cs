using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWallet.Migrations
{
    /// <inheritdoc />
    public partial class Roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Employees",
                type: "TEXT",
                nullable: false,
                computedColumnSql: "[FirstName] + CASE WHEN [Voorvoegsel] IS NULL OR [Voorvoegsel] = '' THEN ' ' ELSE ' ' + [Voorvoegsel] + ' ' END + [LastName]",
                stored: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Employees",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldComputedColumnSql: "[FirstName] + CASE WHEN [Voorvoegsel] IS NULL OR [Voorvoegsel] = '' THEN ' ' ELSE ' ' + [Voorvoegsel] + ' ' END + [LastName]");
        }
    }
}
