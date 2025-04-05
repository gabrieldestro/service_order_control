using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceOrder.Repository.Migrations
{
    /// <inheritdoc />
    public partial class FixingFields_20250405 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResellerName",
                table: "Clients",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "FinalCustomerName",
                table: "Clients",
                newName: "LastUpdated");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Orders",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 5, 14, 11, 29, 40, DateTimeKind.Local).AddTicks(2282));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Orders",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "Cnpj",
                table: "ElectricCompanies",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ElectricCompanies",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 5, 14, 11, 29, 40, DateTimeKind.Local).AddTicks(3047));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ElectricCompanies",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "ElectricCompanies",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "ElectricCompanies",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cnpj",
                table: "Clients",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Clients",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 5, 14, 11, 29, 40, DateTimeKind.Local).AddTicks(2706));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Clients",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "Clients",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Cnpj",
                table: "ElectricCompanies");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ElectricCompanies");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ElectricCompanies");

            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "ElectricCompanies");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "ElectricCompanies");

            migrationBuilder.DropColumn(
                name: "Cnpj",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Clients",
                newName: "ResellerName");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "Clients",
                newName: "FinalCustomerName");
        }
    }
}
