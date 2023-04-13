using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quqaz.Web.Migrations
{
    public partial class AddSendOrdersReturnedToMainBranchReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SendOrdersReturnedToMainBranchReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    DriverId = table.Column<int>(type: "int", nullable: false),
                    MainBranchId = table.Column<int>(type: "int", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrinterName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SendOrdersReturnedToMainBranchReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SendOrdersReturnedToMainBranchReports_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SendOrdersReturnedToMainBranchReports_Branches_MainBranchId",
                        column: x => x.MainBranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SendOrdersReturnedToMainBranchReports_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SendOrdersReturnedToMainBranchDetali",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SendOrdersReturnedToMainBranchReportId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SendOrdersReturnedToMainBranchDetali", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SendOrdersReturnedToMainBranchDetali_SendOrdersReturnedToMainBranchReports_SendOrdersReturnedToMainBranchReportId",
                        column: x => x.SendOrdersReturnedToMainBranchReportId,
                        principalTable: "SendOrdersReturnedToMainBranchReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SendOrdersReturnedToMainBranchDetali_SendOrdersReturnedToMainBranchReportId",
                table: "SendOrdersReturnedToMainBranchDetali",
                column: "SendOrdersReturnedToMainBranchReportId");

            migrationBuilder.CreateIndex(
                name: "IX_SendOrdersReturnedToMainBranchReports_BranchId",
                table: "SendOrdersReturnedToMainBranchReports",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_SendOrdersReturnedToMainBranchReports_DriverId",
                table: "SendOrdersReturnedToMainBranchReports",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_SendOrdersReturnedToMainBranchReports_MainBranchId",
                table: "SendOrdersReturnedToMainBranchReports",
                column: "MainBranchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SendOrdersReturnedToMainBranchDetali");

            migrationBuilder.DropTable(
                name: "SendOrdersReturnedToMainBranchReports");
        }
    }
}
