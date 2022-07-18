using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class addbranchdiscAcceptOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "DisAcceptOrder",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_DisAcceptOrder_BranchId",
                table: "DisAcceptOrder",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_DisAcceptOrder_Branches_BranchId",
                table: "DisAcceptOrder",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DisAcceptOrder_Branches_BranchId",
                table: "DisAcceptOrder");

            migrationBuilder.DropIndex(
                name: "IX_DisAcceptOrder_BranchId",
                table: "DisAcceptOrder");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "DisAcceptOrder");
        }
    }
}
