using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class AddMediatorBranch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NextBranchId",
                table: "Order",
                type: "int",
                nullable: true);

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
                values: new object[,]
                {
                    { 1, 4, 3 },
                    { 2, 4, 3 }
                });

            migrationBuilder.InsertData(
                table: "UserBranch",
                columns: new[] { "BranchId", "UserId" },
                values: new object[,]
                {
                    { 3, 1 },
                    { 4, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_NextBranchId",
                table: "Order",
                column: "NextBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_MediatorBranch_MediatorBranchId",
                table: "MediatorBranch",
                column: "MediatorBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_MediatorBranch_ToBranchId",
                table: "MediatorBranch",
                column: "ToBranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Branches_NextBranchId",
                table: "Order",
                column: "NextBranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Branches_NextBranchId",
                table: "Order");

            migrationBuilder.DropTable(
                name: "MediatorBranch");

            migrationBuilder.DropIndex(
                name: "IX_Order_NextBranchId",
                table: "Order");

            migrationBuilder.DeleteData(
                table: "UserBranch",
                keyColumns: new[] { "BranchId", "UserId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "UserBranch",
                keyColumns: new[] { "BranchId", "UserId" },
                keyValues: new object[] { 4, 1 });

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

            migrationBuilder.DropColumn(
                name: "NextBranchId",
                table: "Order");
        }
    }
}
