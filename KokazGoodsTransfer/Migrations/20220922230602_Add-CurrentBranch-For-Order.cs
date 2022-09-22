using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class AddCurrentBranchForOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentBranchId",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Order_CurrentBranchId",
                table: "Order",
                column: "CurrentBranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Branches_CurrentBranchId",
                table: "Order",
                column: "CurrentBranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Branches_CurrentBranchId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_CurrentBranchId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "CurrentBranchId",
                table: "Order");
        }
    }
}
