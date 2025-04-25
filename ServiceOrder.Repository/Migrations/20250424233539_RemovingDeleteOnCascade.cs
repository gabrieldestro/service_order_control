using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceOrder.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RemovingDeleteOnCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Clients_ClientId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Clients_FinalClientId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ElectricCompanies_ElectricCompanyId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ReceivedDays",
                table: "OrderDeadlines");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Orders",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 24, 20, 35, 39, 511, DateTimeKind.Local).AddTicks(668),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 12, 11, 21, 55, 280, DateTimeKind.Local).AddTicks(598));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "OrderDeadlines",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 24, 20, 35, 39, 511, DateTimeKind.Local).AddTicks(3459),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 12, 11, 21, 55, 280, DateTimeKind.Local).AddTicks(2103));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ElectricCompanies",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 24, 20, 35, 39, 511, DateTimeKind.Local).AddTicks(3043),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 12, 11, 21, 55, 280, DateTimeKind.Local).AddTicks(1736));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Clients",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 24, 20, 35, 39, 511, DateTimeKind.Local).AddTicks(2698),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 12, 11, 21, 55, 280, DateTimeKind.Local).AddTicks(1343));

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Clients_ClientId",
                table: "Orders",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Clients_FinalClientId",
                table: "Orders",
                column: "FinalClientId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ElectricCompanies_ElectricCompanyId",
                table: "Orders",
                column: "ElectricCompanyId",
                principalTable: "ElectricCompanies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Clients_ClientId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Clients_FinalClientId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ElectricCompanies_ElectricCompanyId",
                table: "Orders");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Orders",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 12, 11, 21, 55, 280, DateTimeKind.Local).AddTicks(598),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 24, 20, 35, 39, 511, DateTimeKind.Local).AddTicks(668));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "OrderDeadlines",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 12, 11, 21, 55, 280, DateTimeKind.Local).AddTicks(2103),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 24, 20, 35, 39, 511, DateTimeKind.Local).AddTicks(3459));

            migrationBuilder.AddColumn<int>(
                name: "ReceivedDays",
                table: "OrderDeadlines",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ElectricCompanies",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 12, 11, 21, 55, 280, DateTimeKind.Local).AddTicks(1736),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 24, 20, 35, 39, 511, DateTimeKind.Local).AddTicks(3043));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Clients",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 12, 11, 21, 55, 280, DateTimeKind.Local).AddTicks(1343),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 24, 20, 35, 39, 511, DateTimeKind.Local).AddTicks(2698));

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Clients_ClientId",
                table: "Orders",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Clients_FinalClientId",
                table: "Orders",
                column: "FinalClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ElectricCompanies_ElectricCompanyId",
                table: "Orders",
                column: "ElectricCompanyId",
                principalTable: "ElectricCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
