using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class UserBranchSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UserBranch",
                columns: new[] { "BranchId", "UserId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "UserBranch",
                columns: new[] { "BranchId", "UserId" },
                values: new object[] { 2, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserBranch",
                keyColumns: new[] { "BranchId", "UserId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "UserBranch",
                keyColumns: new[] { "BranchId", "UserId" },
                keyValues: new object[] { 2, 1 });
        }
    }
}
