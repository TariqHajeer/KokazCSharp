using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class ClientBranches : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_BranchId",
                table: "Clients",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Branches_BranchId",
                table: "Clients",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Branches_BranchId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_BranchId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Clients");
        }
    }
}
