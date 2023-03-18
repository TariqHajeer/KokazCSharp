using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class RemoveOrderPlaceFromOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_OrderState",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "OrderStateId",
                table: "Order",
                newName: "OrderState");

            migrationBuilder.RenameIndex(
                name: "IX_Order_OrderStateId",
                table: "Order",
                newName: "IX_Order_OrderState");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderState",
                table: "Order",
                newName: "OrderStateId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_OrderState",
                table: "Order",
                newName: "IX_Order_OrderStateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_OrderState",
                table: "Order",
                column: "OrderStateId",
                principalTable: "OrderState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
