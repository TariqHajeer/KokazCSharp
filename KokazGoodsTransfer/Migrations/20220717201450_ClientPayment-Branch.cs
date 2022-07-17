using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class ClientPaymentBranch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "ClientPayment",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_ClientPayment_BranchId",
                table: "ClientPayment",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientPayment_Branches_BranchId",
                table: "ClientPayment",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientPayment_Branches_BranchId",
                table: "ClientPayment");

            migrationBuilder.DropIndex(
                name: "IX_ClientPayment_BranchId",
                table: "ClientPayment");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "ClientPayment");
        }
    }
}
