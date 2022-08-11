using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class PorintsBranch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "PointsSetting",
                type: "int",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.CreateIndex(
                name: "IX_PointsSetting_BranchId",
                table: "PointsSetting",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_PointsSetting_Branches_BranchId",
                table: "PointsSetting",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointsSetting_Branches_BranchId",
                table: "PointsSetting");

            migrationBuilder.DropIndex(
                name: "IX_PointsSetting_BranchId",
                table: "PointsSetting");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "PointsSetting");
        }
    }
}
