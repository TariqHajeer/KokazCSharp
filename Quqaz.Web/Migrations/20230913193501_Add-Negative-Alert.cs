using Microsoft.EntityFrameworkCore.Migrations;

namespace Quqaz.Web.Migrations
{
    public partial class AddNegativeAlert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NegativeAlert",
                table: "Order",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NegativeAlert",
                table: "Order");
        }
    }
}
