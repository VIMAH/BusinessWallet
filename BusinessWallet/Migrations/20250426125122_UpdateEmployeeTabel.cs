using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWallet.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmployeeTabel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BirthCountry",
                table: "Employees",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Employees",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LaatsteAanmelding",
                table: "Employees",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Voorletters",
                table: "Employees",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Voorvoegsel",
                table: "Employees",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthCountry",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "LaatsteAanmelding",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Voorletters",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Voorvoegsel",
                table: "Employees");
        }
    }
}
