using Microsoft.EntityFrameworkCore.Migrations;

namespace Quqaz.Web.Migrations
{
    public partial class AddDriverForAgentPrint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DriverId",
                table: "AgentPrint",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AgentPrint_DriverId",
                table: "AgentPrint",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_AgentPrint_Drivers_DriverId",
                table: "AgentPrint",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgentPrint_Drivers_DriverId",
                table: "AgentPrint");

            migrationBuilder.DropIndex(
                name: "IX_AgentPrint_DriverId",
                table: "AgentPrint");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "AgentPrint");
        }
    }
}
