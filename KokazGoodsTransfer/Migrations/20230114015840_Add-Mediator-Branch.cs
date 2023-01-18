using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class AddMediatorBranch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MediatorBranch",
                columns: table => new
                {
                    FromBranchId = table.Column<int>(type: "int", nullable: false),
                    ToBranchId = table.Column<int>(type: "int", nullable: false),
                    MediatorBranchId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediatorBranch", x => new { x.FromBranchId, x.ToBranchId });
                    table.ForeignKey(
                        name: "FK_MediatorBranch_Branches_FromBranchId",
                        column: x => x.FromBranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MediatorBranch_Branches_MediatorBranchId",
                        column: x => x.MediatorBranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MediatorBranch_Branches_ToBranchId",
                        column: x => x.ToBranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "Id", "DeliveryCost", "IsMain", "mediatorId", "Name", "Points" },
                values: new object[] { 3, 20000m, false, null, "مدينة 3", 20 });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "Id", "DeliveryCost", "IsMain", "mediatorId", "Name", "Points" },
                values: new object[] { 4, 20000m, false, null, "مدينة 4", 20 });

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "Id", "CountryId", "Name" },
                values: new object[] { 3, 3, "الفرع الثالث (وسيط)" });

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "Id", "CountryId", "Name" },
                values: new object[] { 4, 4, "الفرع الرابع" });

            migrationBuilder.InsertData(
                table: "MediatorBranch",
                columns: new[] { "FromBranchId", "ToBranchId", "MediatorBranchId" },
                values: new object[] { 1, 4, 3 });

            migrationBuilder.InsertData(
                table: "MediatorBranch",
                columns: new[] { "FromBranchId", "ToBranchId", "MediatorBranchId" },
                values: new object[] { 2, 4, 3 });

            migrationBuilder.CreateIndex(
                name: "IX_MediatorBranch_MediatorBranchId",
                table: "MediatorBranch",
                column: "MediatorBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_MediatorBranch_ToBranchId",
                table: "MediatorBranch",
                column: "ToBranchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediatorBranch");

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
