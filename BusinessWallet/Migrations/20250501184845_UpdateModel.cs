using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWallet.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanIssue",
                table: "Roles",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanRevoke",
                table: "Roles",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanVerify",
                table: "Roles",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanIssue",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanRevoke",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanVerify",
                table: "Roles");
        }
    }
}
