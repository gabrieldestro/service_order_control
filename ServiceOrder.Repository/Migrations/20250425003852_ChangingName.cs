using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceOrder.Repository.Migrations
{
    /// <inheritdoc />
    public partial class ChangingName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "OrderDeadlines",
                newName: "OrderIdentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Orders",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 24, 21, 38, 52, 444, DateTimeKind.Local).AddTicks(253),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 24, 20, 38, 36, 890, DateTimeKind.Local).AddTicks(9507));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "OrderDeadlines",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 24, 21, 38, 52, 444, DateTimeKind.Local).AddTicks(3254),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 24, 20, 38, 36, 891, DateTimeKind.Local).AddTicks(2227));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ElectricCompanies",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 24, 21, 38, 52, 444, DateTimeKind.Local).AddTicks(2904),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 24, 20, 38, 36, 891, DateTimeKind.Local).AddTicks(1868));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Clients",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 24, 21, 38, 52, 444, DateTimeKind.Local).AddTicks(2562),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 24, 20, 38, 36, 891, DateTimeKind.Local).AddTicks(1484));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderIdentifier",
                table: "OrderDeadlines",
                newName: "OrderId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Orders",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 24, 20, 38, 36, 890, DateTimeKind.Local).AddTicks(9507),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 24, 21, 38, 52, 444, DateTimeKind.Local).AddTicks(253));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "OrderDeadlines",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 24, 20, 38, 36, 891, DateTimeKind.Local).AddTicks(2227),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 24, 21, 38, 52, 444, DateTimeKind.Local).AddTicks(3254));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ElectricCompanies",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 24, 20, 38, 36, 891, DateTimeKind.Local).AddTicks(1868),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 24, 21, 38, 52, 444, DateTimeKind.Local).AddTicks(2904));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Clients",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 24, 20, 38, 36, 891, DateTimeKind.Local).AddTicks(1484),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 24, 21, 38, 52, 444, DateTimeKind.Local).AddTicks(2562));
        }
    }
}
