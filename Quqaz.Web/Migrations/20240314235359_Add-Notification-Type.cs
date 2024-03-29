using Microsoft.EntityFrameworkCore.Migrations;

namespace Quqaz.Web.Migrations
{
    public partial class AddNotificationType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from Notfication");
            migrationBuilder.AddColumn<string>(
                name: "NotificationExtraData",
                table: "Notfication",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NotificationType",
                table: "Notfication",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationExtraData",
                table: "Notfication");

            migrationBuilder.DropColumn(
                name: "NotificationType",
                table: "Notfication");
        }
    }
}
