using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class DropCurrentCountry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Order__CurrentCo__078C1F06",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_CurrentCountry",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "CurrentCountry",
                table: "Order");

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "CurrentCountry",
                table: "Order",
                type:"int",
                nullable:true);
            migrationBuilder.CreateIndex(
                name: "IX_Order_CurrentCountry",
                table: "Order",
                column: "CurrentCountry"
                );
            migrationBuilder.AddForeignKey(
                name: "FK__Order__CurrentCo__078C1F06",
                table: "Order",
                column: "CurrentCountry",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
