using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceOrder.Repository.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FinalCustomerName = table.Column<string>(type: "TEXT", nullable: true),
                    ResellerName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ElectricCompanies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectricCompanies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReceivedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DocumentSentDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DocumentReceivedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ProjectRegistrationDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ProjectSubmissionDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ProjectApprovalDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    InspectionRequestDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    FinalizationDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ProjectValue = table.Column<decimal>(type: "TEXT", nullable: true),
                    ClientId = table.Column<int>(type: "INTEGER", nullable: false),
                    ElectricCompanyId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_ElectricCompanies_ElectricCompanyId",
                        column: x => x.ElectricCompanyId,
                        principalTable: "ElectricCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ClientId",
                table: "Orders",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ElectricCompanyId",
                table: "Orders",
                column: "ElectricCompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "ElectricCompanies");
        }
    }
}
