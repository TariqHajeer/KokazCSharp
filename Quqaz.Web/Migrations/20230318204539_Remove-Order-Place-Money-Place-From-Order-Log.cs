using Microsoft.EntityFrameworkCore.Migrations;

namespace Quqaz.Web.Migrations
{
    public partial class RemoveOrderPlaceMoneyPlaceFromOrderLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderLog_MoenyPlaced",
                table: "OrderLog");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderLog_OrderPlaced",
                table: "OrderLog");

            migrationBuilder.DropIndex(
                name: "IX_OrderLog_MoenyPlacedId",
                table: "OrderLog");

            migrationBuilder.DropIndex(
                name: "IX_OrderLog_OrderplacedId",
                table: "OrderLog");

            migrationBuilder.RenameColumn(
                name: "OrderplacedId",
                table: "OrderLog",
                newName: "OrderPlace");

            migrationBuilder.RenameColumn(
                name: "MoenyPlacedId",
                table: "OrderLog",
                newName: "MoneyPalce");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderPlace",
                table: "OrderLog",
                newName: "OrderplacedId");

            migrationBuilder.RenameColumn(
                name: "MoneyPalce",
                table: "OrderLog",
                newName: "MoenyPlacedId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLog_MoenyPlacedId",
                table: "OrderLog",
                column: "MoenyPlacedId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLog_OrderplacedId",
                table: "OrderLog",
                column: "OrderplacedId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLog_MoenyPlaced",
                table: "OrderLog",
                column: "MoenyPlacedId",
                principalTable: "MoenyPlaced",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLog_OrderPlaced",
                table: "OrderLog",
                column: "OrderplacedId",
                principalTable: "OrderPlaced",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
