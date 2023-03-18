using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class RemoveNotificationRelationWithOrderPlaceAndMoneyPLace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Notficati__Money__208CD6FA",
                table: "Notfication");

            migrationBuilder.DropForeignKey(
                name: "FK__Notficati__Order__2180FB33",
                table: "Notfication");

            migrationBuilder.DropIndex(
                name: "IX_Notfication_MoneyPlacedId",
                table: "Notfication");

            migrationBuilder.DropIndex(
                name: "IX_Notfication_OrderPlacedId",
                table: "Notfication");

            migrationBuilder.RenameColumn(
                name: "OrderPlacedId",
                table: "Notfication",
                newName: "OrderPlace");

            migrationBuilder.RenameColumn(
                name: "MoneyPlacedId",
                table: "Notfication",
                newName: "MoneyPlace");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderPlace",
                table: "Notfication",
                newName: "OrderPlacedId");

            migrationBuilder.RenameColumn(
                name: "MoneyPlace",
                table: "Notfication",
                newName: "MoneyPlacedId");

            migrationBuilder.CreateIndex(
                name: "IX_Notfication_MoneyPlacedId",
                table: "Notfication",
                column: "MoneyPlacedId");

            migrationBuilder.CreateIndex(
                name: "IX_Notfication_OrderPlacedId",
                table: "Notfication",
                column: "OrderPlacedId");

            migrationBuilder.AddForeignKey(
                name: "FK__Notficati__Money__208CD6FA",
                table: "Notfication",
                column: "MoneyPlacedId",
                principalTable: "MoenyPlaced",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__Notficati__Order__2180FB33",
                table: "Notfication",
                column: "OrderPlacedId",
                principalTable: "OrderPlaced",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
