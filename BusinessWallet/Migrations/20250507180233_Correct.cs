using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWallet.Migrations
{
    /// <inheritdoc />
    public partial class Correct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AuthorizationLogs_EmployeeId",
                table: "AuthorizationLogs");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizationLogs_EmployeeId_RoleId_RequestedBy_CreatedAt",
                table: "AuthorizationLogs",
                columns: new[] { "EmployeeId", "RoleId", "RequestedBy", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AuthorizationLogs_EmployeeId_RoleId_RequestedBy_CreatedAt",
                table: "AuthorizationLogs");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizationLogs_EmployeeId",
                table: "AuthorizationLogs",
                column: "EmployeeId");
        }
    }
}
