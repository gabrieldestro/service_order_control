using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceOrder.Repository.Migrations
{
    /// <inheritdoc />
    public partial class FieldSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Orders",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 8, 18, 54, 55, 668, DateTimeKind.Local).AddTicks(2992),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 6, 9, 55, 27, 774, DateTimeKind.Local).AddTicks(89));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "OrderDeadlines",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 8, 18, 54, 55, 668, DateTimeKind.Local).AddTicks(4221),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 6, 9, 55, 27, 774, DateTimeKind.Local).AddTicks(1328));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ElectricCompanies",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 8, 18, 54, 55, 668, DateTimeKind.Local).AddTicks(3853),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 6, 9, 55, 27, 774, DateTimeKind.Local).AddTicks(919));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Clients",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 8, 18, 54, 55, 668, DateTimeKind.Local).AddTicks(3452),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 6, 9, 55, 27, 774, DateTimeKind.Local).AddTicks(581));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Orders",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 6, 9, 55, 27, 774, DateTimeKind.Local).AddTicks(89),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 8, 18, 54, 55, 668, DateTimeKind.Local).AddTicks(2992));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "OrderDeadlines",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 6, 9, 55, 27, 774, DateTimeKind.Local).AddTicks(1328),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 8, 18, 54, 55, 668, DateTimeKind.Local).AddTicks(4221));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ElectricCompanies",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 6, 9, 55, 27, 774, DateTimeKind.Local).AddTicks(919),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 8, 18, 54, 55, 668, DateTimeKind.Local).AddTicks(3853));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Clients",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 6, 9, 55, 27, 774, DateTimeKind.Local).AddTicks(581),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 8, 18, 54, 55, 668, DateTimeKind.Local).AddTicks(3452));
        }
    }
}
