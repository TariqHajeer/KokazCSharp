using Microsoft.EntityFrameworkCore.Migrations;

namespace Quqaz.Web.Migrations
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

            migrationBuilder.DropForeignKey(
                name: "FK_Order_OrderPlaced_NewOrderPlacedId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_NewOrderPlacedId",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "OrderplacedId",
                table: "Order",
                newName: "OrderPlace");

            migrationBuilder.RenameColumn(
                name: "NewOrderPlacedId",
                table: "Order",
                newName: "NewOrderPlace");

            migrationBuilder.RenameColumn(
                name: "MoenyPlacedId",
                table: "Order",
                newName: "MoneyPlace");

            migrationBuilder.RenameIndex(
                name: "IX_Order_OrderplacedId",
                table: "Order",
                newName: "IX_Order_OrderPlace");

            migrationBuilder.RenameIndex(
                name: "IX_Order_MoenyPlacedId",
                table: "Order",
                newName: "IX_Order_MoneyPlace");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderPlace",
                table: "Order",
                newName: "OrderplacedId");

            migrationBuilder.RenameColumn(
                name: "NewOrderPlace",
                table: "Order",
                newName: "NewOrderPlacedId");

            migrationBuilder.RenameColumn(
                name: "MoneyPlace",
                table: "Order",
                newName: "MoenyPlacedId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_OrderPlace",
                table: "Order",
                newName: "IX_Order_OrderplacedId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_MoneyPlace",
                table: "Order",
                newName: "IX_Order_MoenyPlacedId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_NewOrderPlacedId",
                table: "Order",
                column: "NewOrderPlacedId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Order_OrderPlaced_NewOrderPlacedId",
                table: "Order",
                column: "NewOrderPlacedId",
                principalTable: "OrderPlaced",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
