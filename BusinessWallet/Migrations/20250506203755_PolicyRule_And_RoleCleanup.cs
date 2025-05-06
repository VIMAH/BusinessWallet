using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWallet.Migrations
{
    /// <inheritdoc />
    public partial class PolicyRule_And_RoleCleanup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanIssue",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanPresent",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanReceive",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanRevoke",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanStore",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanVerify",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CanView",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Employees");

            migrationBuilder.CreateTable(
                name: "AuthorizationLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RequestedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Action = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CredentialKey = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    AttributesJson = table.Column<string>(type: "TEXT", nullable: true),
                    Result = table.Column<bool>(type: "INTEGER", nullable: false),
                    Reason = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizationLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthorizationLogs_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorizationLogs_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PolicyRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Action = table.Column<string>(type: "TEXT", nullable: false),
                    TargetType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    TargetValue = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    ConditionJson = table.Column<string>(type: "TEXT", nullable: false),
                    IsAllowed = table.Column<bool>(type: "INTEGER", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolicyRules", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizationLogs_EmployeeId",
                table: "AuthorizationLogs",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizationLogs_RoleId",
                table: "AuthorizationLogs",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorizationLogs");

            migrationBuilder.DropTable(
                name: "PolicyRules");

            migrationBuilder.AddColumn<bool>(
                name: "CanIssue",
                table: "Roles",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanPresent",
                table: "Roles",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanReceive",
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
                name: "CanStore",
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

            migrationBuilder.AddColumn<bool>(
                name: "CanView",
                table: "Roles",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Employees",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
