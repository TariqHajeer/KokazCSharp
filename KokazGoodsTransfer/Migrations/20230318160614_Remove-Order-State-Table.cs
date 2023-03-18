using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class RemoveOrderStateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_OrderState",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK__ReceiptOf__Order__24E777C3",
                table: "ReceiptOfTheOrderStatusDetalis");

            migrationBuilder.DropTable(
                name: "OrderState");

            migrationBuilder.RenameColumn(
                name: "OrderStateId",
                table: "ReceiptOfTheOrderStatusDetalis",
                newName: "OrderState");

            migrationBuilder.RenameIndex(
                name: "IX_ReceiptOfTheOrderStatusDetalis_OrderStateId",
                table: "ReceiptOfTheOrderStatusDetalis",
                newName: "IX_ReceiptOfTheOrderStatusDetalis_OrderState");

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
                table: "ReceiptOfTheOrderStatusDetalis",
                newName: "OrderStateId");

            migrationBuilder.RenameIndex(
                name: "IX_ReceiptOfTheOrderStatusDetalis_OrderState",
                table: "ReceiptOfTheOrderStatusDetalis",
                newName: "IX_ReceiptOfTheOrderStatusDetalis_OrderStateId");

            migrationBuilder.RenameColumn(
                name: "OrderState",
                table: "Order",
                newName: "OrderStateId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_OrderState",
                table: "Order",
                newName: "IX_Order_OrderStateId");

            migrationBuilder.CreateTable(
                name: "OrderState",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderState", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "OrderState",
                columns: new[] { "Id", "State" },
                values: new object[] { 1, "قيد المعالجة" });

            migrationBuilder.InsertData(
                table: "OrderState",
                columns: new[] { "Id", "State" },
                values: new object[] { 2, "يحب اخذ النقود من العميل" });

            migrationBuilder.InsertData(
                table: "OrderState",
                columns: new[] { "Id", "State" },
                values: new object[] { 3, "منتهية" });

            migrationBuilder.AddForeignKey(
                name: "FK_Order_OrderState",
                table: "Order",
                column: "OrderStateId",
                principalTable: "OrderState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__ReceiptOf__Order__24E777C3",
                table: "ReceiptOfTheOrderStatusDetalis",
                column: "OrderStateId",
                principalTable: "OrderState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
