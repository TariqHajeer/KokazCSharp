using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class RemoveOrderPlacedMoneyPlacedRelationWithOrderAndRenameField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_MoenyPlaced",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_OrderPlaced",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "OrderplacedId",
                table: "Order",
                newName: "Orderplaced");

            migrationBuilder.RenameColumn(
                name: "MoenyPlacedId",
                table: "Order",
                newName: "MoneyPlaced");

            migrationBuilder.RenameIndex(
                name: "IX_Order_OrderplacedId",
                table: "Order",
                newName: "IX_Order_Orderplaced");

            migrationBuilder.RenameIndex(
                name: "IX_Order_MoenyPlacedId",
                table: "Order",
                newName: "IX_Order_MoneyPlaced");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Orderplaced",
                table: "Order",
                newName: "OrderplacedId");

            migrationBuilder.RenameColumn(
                name: "MoneyPlaced",
                table: "Order",
                newName: "MoenyPlacedId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_Orderplaced",
                table: "Order",
                newName: "IX_Order_OrderplacedId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_MoneyPlaced",
                table: "Order",
                newName: "IX_Order_MoenyPlacedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_MoenyPlaced",
                table: "Order",
                column: "MoenyPlacedId",
                principalTable: "MoenyPlaced",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_OrderPlaced",
                table: "Order",
                column: "OrderplacedId",
                principalTable: "OrderPlaced",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
