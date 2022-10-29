using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class BranchSeeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "Id", "CountryId", "Name" },
                values: new object[] { 1, 1, "الفرع الرئيسي" });

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "Id", "CountryId", "Name" },
                values: new object[] { 2, 2, "الفرع الثاني" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
