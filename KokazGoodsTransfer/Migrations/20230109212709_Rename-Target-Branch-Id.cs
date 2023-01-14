using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class RenameTargetBranchId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Branches_SecondBranchId",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "SecondBranchId",
                table: "Order",
                newName: "TargetBranchId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_SecondBranchId",
                table: "Order",
                newName: "IX_Order_TargetBranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Branches_TargetBranchId",
                table: "Order",
                column: "TargetBranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Branches_TargetBranchId",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "TargetBranchId",
                table: "Order",
                newName: "SecondBranchId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_TargetBranchId",
                table: "Order",
                newName: "IX_Order_SecondBranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Branches_SecondBranchId",
                table: "Order",
                column: "SecondBranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
