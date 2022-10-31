using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class GroupByBranch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Group",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Group",
                keyColumn: "Id",
                keyValue: 1,
                column: "BranchId",
                value: 1);

            migrationBuilder.InsertData(
                table: "Group",
                columns: new[] { "Id", "BranchId", "Name" },
                values: new object[] { 2, 2, "مجموعة المدراء" });


            migrationBuilder.InsertData(
                table: "GroupPrivilege",
                columns: new[] { "GroupId", "PrivilegId" },
                values: new object[,]
                {
                    { 2, 1 },
                    { 2, 35 },
                    { 2, 36 },
                    { 2, 37 },
                    { 2, 38 },
                    { 2, 39 },
                    { 2, 40 },
                    { 2, 41 },
                    { 2, 42 },
                    { 2, 43 },
                    { 2, 44 },
                    { 2, 45 },
                    { 2, 46 },
                    { 2, 47 },
                    { 2, 34 },
                    { 2, 48 },
                    { 2, 50 },
                    { 2, 51 },
                    { 2, 52 },
                    { 2, 53 },
                    { 2, 54 },
                    { 2, 55 },
                    { 2, 56 },
                    { 2, 57 },
                    { 2, 58 },
                    { 2, 59 },
                    { 2, 60 },
                    { 2, 61 },
                    { 2, 62 },
                    { 2, 49 },
                    { 2, 33 },
                    { 2, 32 },
                    { 2, 31 },
                    { 2, 2 },
                    { 2, 3 },
                    { 2, 4 },
                    { 2, 5 },
                    { 2, 6 },
                    { 2, 7 },
                    { 2, 8 },
                    { 2, 9 },
                    { 2, 10 }
                });

            migrationBuilder.InsertData(
                table: "GroupPrivilege",
                columns: new[] { "GroupId", "PrivilegId" },
                values: new object[,]
                {
                    { 2, 11 },
                    { 2, 12 },
                    { 2, 13 },
                    { 2, 14 },
                    { 2, 15 },
                    { 2, 16 },
                    { 2, 17 },
                    { 2, 18 },
                    { 2, 19 },
                    { 2, 20 },
                    { 2, 21 },
                    { 2, 22 },
                    { 2, 23 },
                    { 2, 24 },
                    { 2, 25 },
                    { 2, 26 },
                    { 2, 27 },
                    { 2, 28 },
                    { 2, 29 },
                    { 2, 30 },
                    { 2, 63 }
                });

            migrationBuilder.InsertData(
                table: "UserGroup",
                columns: new[] { "GroupId", "UserId" },
                values: new object[] { 2, 1 });

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

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 4 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 5 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 6 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 7 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 8 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 9 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 10 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 11 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 12 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 13 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 14 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 15 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 16 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 17 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 18 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 19 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 20 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 21 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 22 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 23 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 24 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 25 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 26 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 27 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 28 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 29 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 30 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 31 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 32 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 33 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 34 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 35 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 36 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 37 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 38 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 39 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 40 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 41 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 42 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 43 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 44 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 45 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 46 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 47 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 48 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 49 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 50 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 51 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 52 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 53 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 54 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 55 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 56 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 57 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 58 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 59 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 60 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 61 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 62 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 2, 63 });

            migrationBuilder.DeleteData(
                table: "UserGroup",
                keyColumns: new[] { "GroupId", "UserId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "Group",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Group");
        }
    }
}
