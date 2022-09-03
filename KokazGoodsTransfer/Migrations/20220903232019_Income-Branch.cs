using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class IncomeBranch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var branchId = 2;
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Income",
                type: "int",
                nullable: false,
                defaultValue: branchId);

            migrationBuilder.CreateIndex(
                name: "IX_Income_BranchId",
                table: "Income",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Income_Branches_BranchId",
                table: "Income",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Income_Branches_BranchId",
                table: "Income");

            migrationBuilder.DropIndex(
                name: "IX_Income_BranchId",
                table: "Income");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Income");
        }
    }
}
