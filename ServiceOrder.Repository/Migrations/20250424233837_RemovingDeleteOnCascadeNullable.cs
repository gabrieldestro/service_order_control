using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceOrder.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RemovingDeleteOnCascadeNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "FinalClientId",
                table: "Orders",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "ElectricCompanyId",
                table: "Orders",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Orders",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 24, 20, 38, 36, 890, DateTimeKind.Local).AddTicks(9507),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 24, 20, 35, 39, 511, DateTimeKind.Local).AddTicks(668));

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Orders",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "OrderDeadlines",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 24, 20, 38, 36, 891, DateTimeKind.Local).AddTicks(2227),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 24, 20, 35, 39, 511, DateTimeKind.Local).AddTicks(3459));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ElectricCompanies",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 24, 20, 38, 36, 891, DateTimeKind.Local).AddTicks(1868),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 24, 20, 35, 39, 511, DateTimeKind.Local).AddTicks(3043));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Clients",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 24, 20, 38, 36, 891, DateTimeKind.Local).AddTicks(1484),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 24, 20, 35, 39, 511, DateTimeKind.Local).AddTicks(2698));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "FinalClientId",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ElectricCompanyId",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Orders",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 24, 20, 35, 39, 511, DateTimeKind.Local).AddTicks(668),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 24, 20, 38, 36, 890, DateTimeKind.Local).AddTicks(9507));

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "OrderDeadlines",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 24, 20, 35, 39, 511, DateTimeKind.Local).AddTicks(3459),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 24, 20, 38, 36, 891, DateTimeKind.Local).AddTicks(2227));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ElectricCompanies",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 24, 20, 35, 39, 511, DateTimeKind.Local).AddTicks(3043),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 24, 20, 38, 36, 891, DateTimeKind.Local).AddTicks(1868));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Clients",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 24, 20, 35, 39, 511, DateTimeKind.Local).AddTicks(2698),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 24, 20, 38, 36, 891, DateTimeKind.Local).AddTicks(1484));
        }
    }
}
