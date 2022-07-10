using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class GroupBranch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Group",
                type: "int",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.CreateIndex(
                name: "IX_Group_BranchId",
                table: "Group",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Group_Branches_BranchId",
                table: "Group",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Group_Branches_BranchId",
                table: "Group");

            migrationBuilder.DropIndex(
                name: "IX_Group_BranchId",
                table: "Group");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Group");
        }
    }
}
