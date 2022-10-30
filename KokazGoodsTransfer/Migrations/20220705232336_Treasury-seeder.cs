using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class Treasuryseeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Treasury",
                columns: new[] { "Id", "CreateOnUtc", "IsActive", "Total" },
                values: new object[] { 1, new DateTime(2019, 12, 31, 22, 0, 0, 0, DateTimeKind.Utc), true, 0m });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Treasury",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
