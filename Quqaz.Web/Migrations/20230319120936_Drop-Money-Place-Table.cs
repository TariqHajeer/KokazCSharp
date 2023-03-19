using Microsoft.EntityFrameworkCore.Migrations;

namespace Quqaz.Web.Migrations
{
    public partial class DropMoneyPlaceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoenyPlaced");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MoenyPlaced",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoenyPlaced", x => x.Id);
                });

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
        }
    }
}
