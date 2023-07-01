using Microsoft.EntityFrameworkCore.Migrations;

namespace Quqaz.Web.Migrations
{
    public partial class AddOldCostToReceiptOfTheOrderStatusDetali : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "OldCost",
                table: "ReceiptOfTheOrderStatusDetalis",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OldCost",
                table: "ReceiptOfTheOrderStatusDetalis");
        }
    }
}
