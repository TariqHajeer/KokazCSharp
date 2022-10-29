using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class countrygroupUserGroupUserSeeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "Id", "DeliveryCost", "IsMain", "mediatorId", "Name", "Points" },
                values: new object[,]
                {
                    { 1, 10000m, true, null, "مدينة1", 15 },
                    { 2, 20000m, false, null, "مدينة2", 20 }
                });

            migrationBuilder.InsertData(
                table: "Group",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "مجموعة المدراء" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Adress", "BranchId", "CanWorkAsAgent", "Experince", "HireDate", "IsActive", "Name", "Note", "Password", "Salary", "UserName" },
                values: new object[] { 1, null, null, false, null, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "admin", null, "21232f297a57a5a743894a0e4a801fc3", null, "admin" });

            migrationBuilder.InsertData(
                table: "UserGroup",
                columns: new[] { "GroupId", "UserId" },
                values: new object[] { 1, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UserGroup",
                keyColumns: new[] { "GroupId", "UserId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "Group",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
