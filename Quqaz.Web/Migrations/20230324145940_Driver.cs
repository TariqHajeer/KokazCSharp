using Microsoft.EntityFrameworkCore.Migrations;

namespace Quqaz.Web.Migrations
{
    public partial class Driver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverName",
                table: "TransferToOtherBranches");

            migrationBuilder.AddColumn<int>(
                name: "DriverId",
                table: "TransferToOtherBranches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransferToOtherBranches_DriverId",
                table: "TransferToOtherBranches",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransferToOtherBranches_Drivers_DriverId",
                table: "TransferToOtherBranches",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransferToOtherBranches_Drivers_DriverId",
                table: "TransferToOtherBranches");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_TransferToOtherBranches_DriverId",
                table: "TransferToOtherBranches");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "TransferToOtherBranches");

            migrationBuilder.AddColumn<string>(
                name: "DriverName",
                table: "TransferToOtherBranches",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
