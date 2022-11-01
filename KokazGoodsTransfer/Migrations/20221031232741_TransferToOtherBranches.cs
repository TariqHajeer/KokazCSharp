using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class TransferToOtherBranches : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransferToOtherBranches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceBranchId = table.Column<int>(type: "int", nullable: false),
                    DestinationBranchId = table.Column<int>(type: "int", nullable: false),
                    DriverName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrinterName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferToOtherBranches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferToOtherBranches_Branches_DestinationBranchId",
                        column: x => x.DestinationBranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferToOtherBranches_Branches_SourceBranchId",
                        column: x => x.SourceBranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransferToOtherBranchDetials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransferToOtherBranchId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferToOtherBranchDetials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferToOtherBranchDetials_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransferToOtherBranchDetials_TransferToOtherBranches_TransferToOtherBranchId",
                        column: x => x.TransferToOtherBranchId,
                        principalTable: "TransferToOtherBranches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransferToOtherBranchDetials_CountryId",
                table: "TransferToOtherBranchDetials",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferToOtherBranchDetials_TransferToOtherBranchId",
                table: "TransferToOtherBranchDetials",
                column: "TransferToOtherBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferToOtherBranches_DestinationBranchId",
                table: "TransferToOtherBranches",
                column: "DestinationBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferToOtherBranches_SourceBranchId",
                table: "TransferToOtherBranches",
                column: "SourceBranchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransferToOtherBranchDetials");

            migrationBuilder.DropTable(
                name: "TransferToOtherBranches");
        }
    }
}
