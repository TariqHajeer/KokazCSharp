using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class DropOrderPlace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderPlaced");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderPlaced",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPlaced", x => x.Id);
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
        }
    }
}
