using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWallet.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsAdmin = table.Column<bool>(type: "INTEGER", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    Voorvoegsel = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                    Voorletters = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    FullName = table.Column<string>(type: "TEXT", nullable: false, computedColumnSql: "[FirstName] + CASE WHEN [Voorvoegsel] IS NULL OR [Voorvoegsel] = '' THEN ' ' ELSE ' ' + [Voorvoegsel] + ' ' END + [LastName]", stored: true),
                    BirthName = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    Gender = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    OlderThan18 = table.Column<bool>(type: "INTEGER", nullable: false),
                    BirthPlace = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    BirthCountry = table.Column<string>(type: "TEXT", maxLength: 60, nullable: true),
                    Married = table.Column<bool>(type: "INTEGER", nullable: false),
                    LegalName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Category = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Kvk = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Position = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    VerificationState = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "Unverified"),
                    EmployeeState = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "Inactive"),
                    PublicKey = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LaatsteAanmelding = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    CanIssue = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanReceive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanStore = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanView = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanPresent = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanVerify = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanRevoke = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSystemRole = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeRoleChallenges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Challenge = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsUsed = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeRoleChallenges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeRoleChallenges_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeRoleChallenges_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeRoles",
                columns: table => new
                {
                    EmployeeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeRoles", x => new { x.EmployeeId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_EmployeeRoles_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRoleChallenges_EmployeeId",
                table: "EmployeeRoleChallenges",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRoleChallenges_RoleId",
                table: "EmployeeRoleChallenges",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRoles_RoleId",
                table: "EmployeeRoles",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeRoleChallenges");

            migrationBuilder.DropTable(
                name: "EmployeeRoles");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
