using Microsoft.EntityFrameworkCore.Migrations;

namespace Quqaz.Web.Migrations
{
    public partial class DropMediatorIdFromCountry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Country__mediato__74794A92",
                table: "Country");

            migrationBuilder.DropIndex(
                name: "IX_Country_mediatorId",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "mediatorId",
                table: "Country");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "mediatorId",
                table: "Country",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Country_mediatorId",
                table: "Country",
                column: "mediatorId");

            migrationBuilder.AddForeignKey(
                name: "FK__Country__mediato__74794A92",
                table: "Country",
                column: "mediatorId",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
