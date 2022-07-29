using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class AddBranchAgentPrint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "AgentPrint",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_AgentPrint_BranchId",
                table: "AgentPrint",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_AgentPrint_Branches_BranchId",
                table: "AgentPrint",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgentPrint_Branches_BranchId",
                table: "AgentPrint");

            migrationBuilder.DropIndex(
                name: "IX_AgentPrint_BranchId",
                table: "AgentPrint");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "AgentPrint");
        }
    }
}
