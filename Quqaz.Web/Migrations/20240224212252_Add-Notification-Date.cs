using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quqaz.Web.Migrations
{
    public partial class AddNotificationDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "Notfication",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Notfication",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Notfication",
                type: "nvarchar(max)",
                nullable: true);
            migrationBuilder.Sql("Delete from Notfication");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Body",
                table: "Notfication");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Notfication");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Notfication");
        }
    }
}
