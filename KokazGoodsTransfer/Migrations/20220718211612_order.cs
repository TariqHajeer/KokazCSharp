using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class order : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "SecondBranchId",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "DisAcceptOrder",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "ClientPayment",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Order_BranchId",
                table: "Order",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_SecondBranchId",
                table: "Order",
                column: "SecondBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_DisAcceptOrder_BranchId",
                table: "DisAcceptOrder",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientPayment_BranchId",
                table: "ClientPayment",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientPayment_Branches_BranchId",
                table: "ClientPayment",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DisAcceptOrder_Branches_BranchId",
                table: "DisAcceptOrder",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Branches_BranchId",
                table: "Order",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Branches_SecondBranchId",
                table: "Order",
                column: "SecondBranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientPayment_Branches_BranchId",
                table: "ClientPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_DisAcceptOrder_Branches_BranchId",
                table: "DisAcceptOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Branches_BranchId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Branches_SecondBranchId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_BranchId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_SecondBranchId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_DisAcceptOrder_BranchId",
                table: "DisAcceptOrder");

            migrationBuilder.DropIndex(
                name: "IX_ClientPayment_BranchId",
                table: "ClientPayment");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "SecondBranchId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "DisAcceptOrder");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "ClientPayment");
        }
    }
}
