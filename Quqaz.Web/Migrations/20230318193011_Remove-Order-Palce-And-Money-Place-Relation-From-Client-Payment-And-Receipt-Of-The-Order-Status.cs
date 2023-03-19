using Microsoft.EntityFrameworkCore.Migrations;

namespace Quqaz.Web.Migrations
{
    public partial class RemoveOrderPalceAndMoneyPlaceRelationFromClientPaymentAndReceiptOfTheOrderStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__ClientPay__Money__5224328E",
                table: "ClientPaymentDetails");

            migrationBuilder.DropForeignKey(
                name: "FK__ClientPay__Order__531856C7",
                table: "ClientPaymentDetails");

            migrationBuilder.DropForeignKey(
                name: "FK__ReceiptOf__Money__25DB9BFC",
                table: "ReceiptOfTheOrderStatusDetalis");

            migrationBuilder.DropForeignKey(
                name: "FK__ReceiptOf__Order__5E1FF51F",
                table: "ReceiptOfTheOrderStatusDetalis");

            migrationBuilder.RenameColumn(
                name: "OrderPlacedId",
                table: "ReceiptOfTheOrderStatusDetalis",
                newName: "OrderPlace");

            migrationBuilder.RenameColumn(
                name: "MoneyPlacedId",
                table: "ReceiptOfTheOrderStatusDetalis",
                newName: "MoneyPalce");

            migrationBuilder.RenameIndex(
                name: "IX_ReceiptOfTheOrderStatusDetalis_OrderPlacedId",
                table: "ReceiptOfTheOrderStatusDetalis",
                newName: "IX_ReceiptOfTheOrderStatusDetalis_OrderPlace");

            migrationBuilder.RenameIndex(
                name: "IX_ReceiptOfTheOrderStatusDetalis_MoneyPlacedId",
                table: "ReceiptOfTheOrderStatusDetalis",
                newName: "IX_ReceiptOfTheOrderStatusDetalis_MoneyPalce");

            migrationBuilder.RenameColumn(
                name: "OrderPlacedId",
                table: "ClientPaymentDetails",
                newName: "OrderPlace");

            migrationBuilder.RenameColumn(
                name: "MoneyPlacedId",
                table: "ClientPaymentDetails",
                newName: "MoneyPlace");

            migrationBuilder.RenameIndex(
                name: "IX_ClientPaymentDetails_OrderPlacedId",
                table: "ClientPaymentDetails",
                newName: "IX_ClientPaymentDetails_OrderPlace");

            migrationBuilder.RenameIndex(
                name: "IX_ClientPaymentDetails_MoneyPlacedId",
                table: "ClientPaymentDetails",
                newName: "IX_ClientPaymentDetails_MoneyPlace");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderPlace",
                table: "ReceiptOfTheOrderStatusDetalis",
                newName: "OrderPlacedId");

            migrationBuilder.RenameColumn(
                name: "MoneyPalce",
                table: "ReceiptOfTheOrderStatusDetalis",
                newName: "MoneyPlacedId");

            migrationBuilder.RenameIndex(
                name: "IX_ReceiptOfTheOrderStatusDetalis_OrderPlace",
                table: "ReceiptOfTheOrderStatusDetalis",
                newName: "IX_ReceiptOfTheOrderStatusDetalis_OrderPlacedId");

            migrationBuilder.RenameIndex(
                name: "IX_ReceiptOfTheOrderStatusDetalis_MoneyPalce",
                table: "ReceiptOfTheOrderStatusDetalis",
                newName: "IX_ReceiptOfTheOrderStatusDetalis_MoneyPlacedId");

            migrationBuilder.RenameColumn(
                name: "OrderPlace",
                table: "ClientPaymentDetails",
                newName: "OrderPlacedId");

            migrationBuilder.RenameColumn(
                name: "MoneyPlace",
                table: "ClientPaymentDetails",
                newName: "MoneyPlacedId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientPaymentDetails_OrderPlace",
                table: "ClientPaymentDetails",
                newName: "IX_ClientPaymentDetails_OrderPlacedId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientPaymentDetails_MoneyPlace",
                table: "ClientPaymentDetails",
                newName: "IX_ClientPaymentDetails_MoneyPlacedId");

            migrationBuilder.AddForeignKey(
                name: "FK__ClientPay__Money__5224328E",
                table: "ClientPaymentDetails",
                column: "MoneyPlacedId",
                principalTable: "MoenyPlaced",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__ClientPay__Order__531856C7",
                table: "ClientPaymentDetails",
                column: "OrderPlacedId",
                principalTable: "OrderPlaced",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__ReceiptOf__Money__25DB9BFC",
                table: "ReceiptOfTheOrderStatusDetalis",
                column: "MoneyPlacedId",
                principalTable: "MoenyPlaced",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__ReceiptOf__Order__5E1FF51F",
                table: "ReceiptOfTheOrderStatusDetalis",
                column: "OrderPlacedId",
                principalTable: "OrderPlaced",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
