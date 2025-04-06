using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceOrder.Repository.Migrations
{
    /// <inheritdoc />
    public partial class MigrationName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                oldDefaultValue: new DateTime(2025, 4, 5, 16, 40, 1, 53, DateTimeKind.Local).AddTicks(7526));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ElectricCompanies",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 6, 9, 55, 27, 774, DateTimeKind.Local).AddTicks(919),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 5, 16, 40, 1, 53, DateTimeKind.Local).AddTicks(8431));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Clients",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 6, 9, 55, 27, 774, DateTimeKind.Local).AddTicks(581),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 5, 16, 40, 1, 53, DateTimeKind.Local).AddTicks(8063));

            migrationBuilder.CreateTable(
                name: "OrderDeadlines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ReceivedDays = table.Column<int>(type: "INTEGER", nullable: true),
                    DocumentSentDays = table.Column<int>(type: "INTEGER", nullable: true),
                    DocumentReceivedDays = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectRegistrationDays = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectSubmissionDays = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectApprovalDays = table.Column<int>(type: "INTEGER", nullable: true),
                    InspectionRequestDays = table.Column<int>(type: "INTEGER", nullable: true),
                    FinalizationDays = table.Column<int>(type: "INTEGER", nullable: true),
                    PaymentDays = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValue: new DateTime(2025, 4, 6, 9, 55, 27, 774, DateTimeKind.Local).AddTicks(1328)),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDeadlines", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDeadlines");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Orders",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 5, 16, 40, 1, 53, DateTimeKind.Local).AddTicks(7526),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 6, 9, 55, 27, 774, DateTimeKind.Local).AddTicks(89));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ElectricCompanies",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 5, 16, 40, 1, 53, DateTimeKind.Local).AddTicks(8431),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 6, 9, 55, 27, 774, DateTimeKind.Local).AddTicks(919));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Clients",
                type: "TEXT",
                nullable: true,
                defaultValue: new DateTime(2025, 4, 5, 16, 40, 1, 53, DateTimeKind.Local).AddTicks(8063),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 4, 6, 9, 55, 27, 774, DateTimeKind.Local).AddTicks(581));
        }
    }
}
