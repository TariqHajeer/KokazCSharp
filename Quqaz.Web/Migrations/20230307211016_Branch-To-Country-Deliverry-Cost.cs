using Microsoft.EntityFrameworkCore.Migrations;

namespace Quqaz.Web.Migrations
{
    public partial class BranchToCountryDeliverryCost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BranchToCountryDeliverryCosts",
                columns: table => new
                {
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    DeliveryCost = table.Column<decimal>(type: "decimal(6,0)", nullable: false),
                    Points = table.Column<short>(type: "SMALLINT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchToCountryDeliverryCosts", x => new { x.BranchId, x.CountryId });
                    table.ForeignKey(
                        name: "FK_BranchToCountryDeliverryCosts_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BranchToCountryDeliverryCosts_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "BranchToCountryDeliverryCosts",
                columns: new[] { "BranchId", "CountryId", "DeliveryCost", "Points" },
                values: new object[,]
                {
                    { 3, 1, 5000m, (short)20 },
                    { 7, 9, 5000m, (short)20 },
                    { 7, 10, 5000m, (short)20 },
                    { 7, 11, 5000m, (short)20 },
                    { 7, 12, 5000m, (short)20 },
                    { 7, 13, 5000m, (short)20 },
                    { 7, 14, 5000m, (short)20 },
                    { 7, 15, 5000m, (short)20 },
                    { 7, 16, 5000m, (short)20 },
                    { 7, 17, 5000m, (short)20 },
                    { 7, 18, 5000m, (short)20 },
                    { 7, 19, 5000m, (short)20 },
                    { 7, 20, 5000m, (short)20 },
                    { 7, 8, 5000m, (short)20 },
                    { 7, 21, 5000m, (short)20 },
                    { 7, 23, 5000m, (short)20 },
                    { 7, 24, 5000m, (short)20 },
                    { 7, 25, 5000m, (short)20 },
                    { 7, 26, 5000m, (short)20 },
                    { 7, 27, 5000m, (short)20 },
                    { 7, 28, 5000m, (short)20 },
                    { 7, 29, 5000m, (short)20 },
                    { 7, 30, 5000m, (short)20 },
                    { 7, 31, 5000m, (short)20 },
                    { 7, 32, 5000m, (short)20 },
                    { 7, 33, 5000m, (short)20 },
                    { 7, 34, 5000m, (short)20 },
                    { 7, 22, 5000m, (short)20 },
                    { 7, 7, 5000m, (short)20 },
                    { 7, 6, 5000m, (short)20 },
                    { 7, 5, 5000m, (short)20 },
                    { 5, 27, 5000m, (short)20 },
                    { 5, 28, 5000m, (short)20 },
                    { 5, 29, 5000m, (short)20 },
                    { 5, 30, 5000m, (short)20 },
                    { 5, 31, 5000m, (short)20 },
                    { 5, 32, 5000m, (short)20 },
                    { 5, 33, 5000m, (short)20 },
                    { 5, 34, 5000m, (short)20 },
                    { 5, 35, 5000m, (short)20 },
                    { 5, 36, 5000m, (short)20 },
                    { 5, 37, 5000m, (short)20 }
                });

            migrationBuilder.InsertData(
                table: "BranchToCountryDeliverryCosts",
                columns: new[] { "BranchId", "CountryId", "DeliveryCost", "Points" },
                values: new object[,]
                {
                    { 5, 38, 5000m, (short)20 },
                    { 5, 39, 5000m, (short)20 },
                    { 5, 40, 5000m, (short)20 },
                    { 5, 41, 5000m, (short)20 },
                    { 5, 42, 5000m, (short)20 },
                    { 5, 43, 5000m, (short)20 },
                    { 5, 44, 5000m, (short)20 },
                    { 5, 45, 5000m, (short)20 },
                    { 5, 46, 5000m, (short)20 },
                    { 5, 47, 5000m, (short)20 },
                    { 5, 48, 5000m, (short)20 },
                    { 5, 49, 5000m, (short)20 },
                    { 7, 1, 5000m, (short)20 },
                    { 7, 2, 5000m, (short)20 },
                    { 7, 3, 5000m, (short)20 },
                    { 7, 4, 5000m, (short)20 },
                    { 7, 35, 5000m, (short)20 },
                    { 7, 36, 5000m, (short)20 },
                    { 7, 37, 5000m, (short)20 },
                    { 7, 38, 5000m, (short)20 },
                    { 8, 21, 5000m, (short)20 },
                    { 8, 22, 5000m, (short)20 },
                    { 8, 23, 5000m, (short)20 },
                    { 8, 24, 5000m, (short)20 },
                    { 8, 25, 5000m, (short)20 },
                    { 8, 26, 5000m, (short)20 },
                    { 8, 27, 5000m, (short)20 },
                    { 8, 28, 5000m, (short)20 },
                    { 8, 29, 5000m, (short)20 },
                    { 8, 30, 5000m, (short)20 },
                    { 8, 31, 5000m, (short)20 },
                    { 8, 32, 5000m, (short)20 },
                    { 8, 33, 5000m, (short)20 },
                    { 8, 34, 5000m, (short)20 },
                    { 8, 35, 5000m, (short)20 },
                    { 8, 36, 5000m, (short)20 },
                    { 8, 37, 5000m, (short)20 },
                    { 8, 38, 5000m, (short)20 },
                    { 8, 39, 5000m, (short)20 },
                    { 8, 40, 5000m, (short)20 },
                    { 8, 41, 5000m, (short)20 },
                    { 8, 42, 5000m, (short)20 }
                });

            migrationBuilder.InsertData(
                table: "BranchToCountryDeliverryCosts",
                columns: new[] { "BranchId", "CountryId", "DeliveryCost", "Points" },
                values: new object[,]
                {
                    { 8, 43, 5000m, (short)20 },
                    { 8, 44, 5000m, (short)20 },
                    { 8, 45, 5000m, (short)20 },
                    { 8, 46, 5000m, (short)20 },
                    { 8, 47, 5000m, (short)20 },
                    { 8, 20, 5000m, (short)20 },
                    { 5, 26, 5000m, (short)20 },
                    { 8, 19, 5000m, (short)20 },
                    { 8, 17, 5000m, (short)20 },
                    { 7, 39, 5000m, (short)20 },
                    { 7, 40, 5000m, (short)20 },
                    { 7, 41, 5000m, (short)20 },
                    { 7, 42, 5000m, (short)20 },
                    { 7, 43, 5000m, (short)20 },
                    { 7, 44, 5000m, (short)20 },
                    { 7, 45, 5000m, (short)20 },
                    { 7, 46, 5000m, (short)20 },
                    { 7, 47, 5000m, (short)20 },
                    { 7, 48, 5000m, (short)20 },
                    { 7, 49, 5000m, (short)20 },
                    { 8, 1, 5000m, (short)20 },
                    { 8, 2, 5000m, (short)20 },
                    { 8, 3, 5000m, (short)20 },
                    { 8, 4, 5000m, (short)20 },
                    { 8, 5, 5000m, (short)20 },
                    { 8, 6, 5000m, (short)20 },
                    { 8, 7, 5000m, (short)20 },
                    { 8, 8, 5000m, (short)20 },
                    { 8, 9, 5000m, (short)20 },
                    { 8, 10, 5000m, (short)20 },
                    { 8, 11, 5000m, (short)20 },
                    { 8, 12, 5000m, (short)20 },
                    { 8, 13, 5000m, (short)20 },
                    { 8, 14, 5000m, (short)20 },
                    { 8, 15, 5000m, (short)20 },
                    { 8, 16, 5000m, (short)20 },
                    { 8, 18, 5000m, (short)20 },
                    { 8, 48, 5000m, (short)20 },
                    { 5, 25, 5000m, (short)20 },
                    { 5, 23, 5000m, (short)20 },
                    { 3, 33, 5000m, (short)20 },
                    { 3, 34, 5000m, (short)20 }
                });

            migrationBuilder.InsertData(
                table: "BranchToCountryDeliverryCosts",
                columns: new[] { "BranchId", "CountryId", "DeliveryCost", "Points" },
                values: new object[,]
                {
                    { 3, 35, 5000m, (short)20 },
                    { 3, 36, 5000m, (short)20 },
                    { 3, 37, 5000m, (short)20 },
                    { 3, 38, 5000m, (short)20 },
                    { 3, 39, 5000m, (short)20 },
                    { 3, 40, 5000m, (short)20 },
                    { 3, 41, 5000m, (short)20 },
                    { 3, 42, 5000m, (short)20 },
                    { 3, 43, 5000m, (short)20 },
                    { 3, 44, 5000m, (short)20 },
                    { 3, 32, 5000m, (short)20 },
                    { 3, 45, 5000m, (short)20 },
                    { 3, 47, 5000m, (short)20 },
                    { 3, 48, 5000m, (short)20 },
                    { 3, 49, 5000m, (short)20 },
                    { 4, 1, 5000m, (short)20 },
                    { 4, 2, 5000m, (short)20 },
                    { 4, 3, 5000m, (short)20 },
                    { 4, 4, 5000m, (short)20 },
                    { 4, 5, 5000m, (short)20 },
                    { 4, 6, 5000m, (short)20 },
                    { 4, 7, 5000m, (short)20 },
                    { 4, 8, 5000m, (short)20 },
                    { 4, 9, 5000m, (short)20 },
                    { 3, 46, 5000m, (short)20 },
                    { 3, 31, 5000m, (short)20 },
                    { 3, 30, 5000m, (short)20 },
                    { 3, 29, 5000m, (short)20 },
                    { 3, 2, 5000m, (short)20 },
                    { 3, 3, 5000m, (short)20 },
                    { 3, 4, 5000m, (short)20 },
                    { 3, 5, 5000m, (short)20 },
                    { 3, 6, 5000m, (short)20 },
                    { 3, 7, 5000m, (short)20 },
                    { 3, 8, 5000m, (short)20 },
                    { 3, 9, 5000m, (short)20 },
                    { 3, 10, 5000m, (short)20 },
                    { 3, 11, 5000m, (short)20 },
                    { 3, 12, 5000m, (short)20 },
                    { 3, 13, 5000m, (short)20 },
                    { 3, 14, 5000m, (short)20 },
                    { 3, 15, 5000m, (short)20 }
                });

            migrationBuilder.InsertData(
                table: "BranchToCountryDeliverryCosts",
                columns: new[] { "BranchId", "CountryId", "DeliveryCost", "Points" },
                values: new object[,]
                {
                    { 3, 16, 5000m, (short)20 },
                    { 3, 17, 5000m, (short)20 },
                    { 3, 18, 5000m, (short)20 },
                    { 3, 19, 5000m, (short)20 },
                    { 3, 20, 5000m, (short)20 },
                    { 3, 21, 5000m, (short)20 },
                    { 3, 22, 5000m, (short)20 },
                    { 3, 23, 5000m, (short)20 },
                    { 3, 24, 5000m, (short)20 },
                    { 3, 25, 5000m, (short)20 },
                    { 3, 26, 5000m, (short)20 },
                    { 3, 27, 5000m, (short)20 },
                    { 3, 28, 5000m, (short)20 },
                    { 4, 10, 5000m, (short)20 },
                    { 4, 11, 5000m, (short)20 },
                    { 4, 12, 5000m, (short)20 },
                    { 4, 13, 5000m, (short)20 },
                    { 4, 45, 5000m, (short)20 },
                    { 4, 46, 5000m, (short)20 },
                    { 4, 47, 5000m, (short)20 },
                    { 4, 48, 5000m, (short)20 },
                    { 4, 49, 5000m, (short)20 },
                    { 5, 1, 5000m, (short)20 },
                    { 5, 2, 5000m, (short)20 },
                    { 5, 3, 5000m, (short)20 },
                    { 5, 4, 5000m, (short)20 },
                    { 5, 5, 5000m, (short)20 },
                    { 5, 6, 5000m, (short)20 },
                    { 5, 7, 5000m, (short)20 },
                    { 5, 8, 5000m, (short)20 },
                    { 5, 9, 5000m, (short)20 },
                    { 5, 10, 5000m, (short)20 },
                    { 5, 11, 5000m, (short)20 },
                    { 5, 12, 5000m, (short)20 },
                    { 5, 13, 5000m, (short)20 },
                    { 5, 14, 5000m, (short)20 },
                    { 5, 15, 5000m, (short)20 },
                    { 5, 16, 5000m, (short)20 },
                    { 5, 17, 5000m, (short)20 },
                    { 5, 18, 5000m, (short)20 },
                    { 5, 19, 5000m, (short)20 },
                    { 5, 20, 5000m, (short)20 }
                });

            migrationBuilder.InsertData(
                table: "BranchToCountryDeliverryCosts",
                columns: new[] { "BranchId", "CountryId", "DeliveryCost", "Points" },
                values: new object[,]
                {
                    { 5, 21, 5000m, (short)20 },
                    { 5, 22, 5000m, (short)20 },
                    { 4, 44, 5000m, (short)20 },
                    { 5, 24, 5000m, (short)20 },
                    { 4, 43, 5000m, (short)20 },
                    { 4, 41, 5000m, (short)20 },
                    { 4, 14, 5000m, (short)20 },
                    { 4, 15, 5000m, (short)20 },
                    { 4, 16, 5000m, (short)20 },
                    { 4, 17, 5000m, (short)20 },
                    { 4, 18, 5000m, (short)20 },
                    { 4, 19, 5000m, (short)20 },
                    { 4, 20, 5000m, (short)20 },
                    { 4, 21, 5000m, (short)20 },
                    { 4, 22, 5000m, (short)20 },
                    { 4, 23, 5000m, (short)20 },
                    { 4, 24, 5000m, (short)20 },
                    { 4, 25, 5000m, (short)20 },
                    { 4, 26, 5000m, (short)20 },
                    { 4, 27, 5000m, (short)20 },
                    { 4, 28, 5000m, (short)20 },
                    { 4, 29, 5000m, (short)20 },
                    { 4, 30, 5000m, (short)20 },
                    { 4, 31, 5000m, (short)20 },
                    { 4, 32, 5000m, (short)20 },
                    { 4, 33, 5000m, (short)20 },
                    { 4, 34, 5000m, (short)20 },
                    { 4, 35, 5000m, (short)20 },
                    { 4, 36, 5000m, (short)20 },
                    { 4, 37, 5000m, (short)20 },
                    { 4, 38, 5000m, (short)20 },
                    { 4, 39, 5000m, (short)20 },
                    { 4, 40, 5000m, (short)20 },
                    { 4, 42, 5000m, (short)20 },
                    { 8, 49, 5000m, (short)20 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BranchToCountryDeliverryCosts_BranchId",
                table: "BranchToCountryDeliverryCosts",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchToCountryDeliverryCosts_CountryId",
                table: "BranchToCountryDeliverryCosts",
                column: "CountryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BranchToCountryDeliverryCosts");
        }
    }
}
