using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class receipt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Receipt",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Receipt_BranchId",
                table: "Receipt",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Receipt_Branches_BranchId",
                table: "Receipt",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receipt_Branches_BranchId",
                table: "Receipt");

            migrationBuilder.DropIndex(
                name: "IX_Receipt_BranchId",
                table: "Receipt");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Receipt");
        }
    }
}
