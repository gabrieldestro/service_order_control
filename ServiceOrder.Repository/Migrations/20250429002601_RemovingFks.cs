using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceOrder.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RemovingFks : Migration
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

            migrationBuilder.DropIndex(
                name: "IX_Orders_ClientId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ElectricCompanyId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_FinalClientId",
                table: "Orders");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Orders",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 28, 21, 26, 1, 136, DateTimeKind.Local).AddTicks(3363),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 24, 21, 38, 52, 444, DateTimeKind.Local).AddTicks(253));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "OrderDeadlines",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 28, 21, 26, 1, 136, DateTimeKind.Local).AddTicks(9034),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 24, 21, 38, 52, 444, DateTimeKind.Local).AddTicks(3254));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ElectricCompanies",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 28, 21, 26, 1, 136, DateTimeKind.Local).AddTicks(8447),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 24, 21, 38, 52, 444, DateTimeKind.Local).AddTicks(2904));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Clients",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 28, 21, 26, 1, 136, DateTimeKind.Local).AddTicks(7854),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 24, 21, 38, 52, 444, DateTimeKind.Local).AddTicks(2562));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Orders",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 24, 21, 38, 52, 444, DateTimeKind.Local).AddTicks(253),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 28, 21, 26, 1, 136, DateTimeKind.Local).AddTicks(3363));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "OrderDeadlines",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 24, 21, 38, 52, 444, DateTimeKind.Local).AddTicks(3254),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 28, 21, 26, 1, 136, DateTimeKind.Local).AddTicks(9034));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ElectricCompanies",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 24, 21, 38, 52, 444, DateTimeKind.Local).AddTicks(2904),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 28, 21, 26, 1, 136, DateTimeKind.Local).AddTicks(8447));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Clients",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 24, 21, 38, 52, 444, DateTimeKind.Local).AddTicks(2562),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 28, 21, 26, 1, 136, DateTimeKind.Local).AddTicks(7854));

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ClientId",
                table: "Orders",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ElectricCompanyId",
                table: "Orders",
                column: "ElectricCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_FinalClientId",
                table: "Orders",
                column: "FinalClientId");

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
    }
}
