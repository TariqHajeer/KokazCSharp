using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class OrderPlacedMoneyPlacedOrderStateSeeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MoenyPlaced",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "خارج الشركة" },
                    { 2, "مندوب" },
                    { 3, "داخل الشركة" },
                    { 4, "تم تسليمها" }
                });

            migrationBuilder.InsertData(
                table: "OrderPlaced",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "عند العميل" },
                    { 2, "في المخزن" },
                    { 3, "في الطريق" },
                    { 4, "تم التسليم" },
                    { 5, "مرتجع كلي" },
                    { 6, "مرتجع جزئي" },
                    { 7, "مرفوض" },
                    { 8, "مؤجل" }
                });

            migrationBuilder.InsertData(
                table: "OrderState",
                columns: new[] { "Id", "State" },
                values: new object[,]
                {
                    { 1, "قيد المعالجة" },
                    { 2, "يحب اخذ النقود من العميل" },
                    { 3, "منتهية" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MoenyPlaced",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MoenyPlaced",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MoenyPlaced",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MoenyPlaced",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "OrderPlaced",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OrderPlaced",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OrderPlaced",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OrderPlaced",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "OrderPlaced",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "OrderPlaced",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "OrderPlaced",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "OrderPlaced",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "OrderState",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OrderState",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OrderState",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
