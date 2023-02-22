using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class MidCountry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NextBranchId",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MediatorCountry",
                columns: table => new
                {
                    FromCountryId = table.Column<int>(type: "int", nullable: false),
                    ToCountryId = table.Column<int>(type: "int", nullable: false),
                    MediatorCountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediatorCountry", x => new { x.FromCountryId, x.ToCountryId });
                    table.ForeignKey(
                        name: "FK_MediatorCountry_Country_FromCountryId",
                        column: x => x.FromCountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MediatorCountry_Country_MediatorCountryId",
                        column: x => x.MediatorCountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MediatorCountry_Country_ToCountryId",
                        column: x => x.ToCountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "Id", "DeliveryCost", "IsMain", "mediatorId", "Name", "Points" },
                values: new object[,]
                {
                    { 3, 20000m, false, null, "مدينة3 (وسيطة) ", 20 },
                    { 4, 20000m, false, null, "مدينة 4", 20 },
                    { 5, 20000m, false, null, "مدينة 5", 20 },
                    { 6, 20000m, false, null, "مدينة 6", 20 }
                });

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "Id", "CountryId", "Name" },
                values: new object[,]
                {
                    { 3, 3, "الفرع الثالث الوسيط" },
                    { 4, 4, "فرع 4" }
                });

            migrationBuilder.InsertData(
                table: "MediatorCountry",
                columns: new[] { "FromCountryId", "ToCountryId", "MediatorCountryId" },
                values: new object[,]
                {
                    { 1, 2, 3 },
                    { 2, 1, 3 },
                    { 1, 4, 3 },
                    { 4, 1, 3 },
                    { 1, 5, 3 },
                    { 2, 5, 3 }
                });

            migrationBuilder.InsertData(
                table: "Group",
                columns: new[] { "Id", "BranchId", "Name" },
                values: new object[,]
                {
                    { 3, 3, "مجموعة المدراء" },
                    { 4, 4, "مجموعة المدراء" }
                });

            migrationBuilder.InsertData(
                table: "UserBranch",
                columns: new[] { "BranchId", "UserId" },
                values: new object[,]
                {
                    { 3, 1 },
                    { 4, 1 }
                });

            migrationBuilder.InsertData(
                table: "GroupPrivilege",
                columns: new[] { "GroupId", "PrivilegId" },
                values: new object[,]
                {
                    { 3, 1 },
                    { 4, 28 },
                    { 4, 27 },
                    { 4, 26 },
                    { 4, 25 },
                    { 4, 24 },
                    { 4, 23 },
                    { 4, 22 },
                    { 4, 21 },
                    { 4, 20 },
                    { 4, 19 },
                    { 4, 18 },
                    { 4, 17 },
                    { 4, 16 },
                    { 4, 15 },
                    { 4, 14 },
                    { 4, 13 },
                    { 4, 12 },
                    { 4, 11 },
                    { 4, 10 },
                    { 4, 9 },
                    { 4, 8 },
                    { 4, 7 },
                    { 4, 6 },
                    { 4, 5 },
                    { 4, 4 },
                    { 4, 3 },
                    { 4, 2 },
                    { 4, 29 },
                    { 4, 30 },
                    { 4, 31 },
                    { 4, 32 },
                    { 4, 60 },
                    { 4, 59 },
                    { 4, 58 },
                    { 4, 57 },
                    { 4, 56 },
                    { 4, 55 },
                    { 4, 54 },
                    { 4, 53 },
                    { 4, 52 },
                    { 4, 51 }
                });

            migrationBuilder.InsertData(
                table: "GroupPrivilege",
                columns: new[] { "GroupId", "PrivilegId" },
                values: new object[,]
                {
                    { 4, 50 },
                    { 4, 49 },
                    { 4, 48 },
                    { 4, 1 },
                    { 4, 47 },
                    { 4, 45 },
                    { 4, 44 },
                    { 4, 43 },
                    { 4, 42 },
                    { 4, 41 },
                    { 4, 40 },
                    { 4, 39 },
                    { 4, 38 },
                    { 4, 37 },
                    { 4, 36 },
                    { 4, 35 },
                    { 4, 34 },
                    { 4, 33 },
                    { 4, 46 },
                    { 4, 63 },
                    { 3, 63 },
                    { 3, 62 },
                    { 3, 29 },
                    { 3, 28 },
                    { 3, 27 },
                    { 3, 26 },
                    { 3, 25 },
                    { 3, 24 },
                    { 3, 23 },
                    { 3, 22 },
                    { 3, 21 },
                    { 3, 20 },
                    { 3, 19 },
                    { 3, 18 },
                    { 3, 17 },
                    { 3, 30 },
                    { 3, 16 },
                    { 3, 14 },
                    { 3, 13 },
                    { 3, 12 },
                    { 3, 11 },
                    { 3, 10 }
                });

            migrationBuilder.InsertData(
                table: "GroupPrivilege",
                columns: new[] { "GroupId", "PrivilegId" },
                values: new object[,]
                {
                    { 3, 9 },
                    { 3, 8 },
                    { 3, 7 },
                    { 3, 6 },
                    { 3, 5 },
                    { 3, 4 },
                    { 3, 3 },
                    { 3, 2 },
                    { 3, 15 },
                    { 4, 61 },
                    { 3, 31 },
                    { 3, 33 },
                    { 3, 61 },
                    { 3, 60 },
                    { 3, 59 },
                    { 3, 58 },
                    { 3, 57 },
                    { 3, 56 },
                    { 3, 55 },
                    { 3, 54 },
                    { 3, 53 },
                    { 3, 52 },
                    { 3, 51 },
                    { 3, 50 },
                    { 3, 49 },
                    { 3, 32 },
                    { 3, 48 },
                    { 3, 46 },
                    { 3, 45 },
                    { 3, 44 },
                    { 3, 43 },
                    { 3, 42 },
                    { 3, 41 },
                    { 3, 40 },
                    { 3, 39 },
                    { 3, 38 },
                    { 3, 37 },
                    { 3, 36 },
                    { 3, 35 },
                    { 3, 34 },
                    { 3, 47 },
                    { 4, 62 }
                });

            migrationBuilder.InsertData(
                table: "UserGroup",
                columns: new[] { "GroupId", "UserId" },
                values: new object[] { 3, 1 });

            migrationBuilder.InsertData(
                table: "UserGroup",
                columns: new[] { "GroupId", "UserId" },
                values: new object[] { 4, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Order_NextBranchId",
                table: "Order",
                column: "NextBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_MediatorCountry_MediatorCountryId",
                table: "MediatorCountry",
                column: "MediatorCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_MediatorCountry_ToCountryId",
                table: "MediatorCountry",
                column: "ToCountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Branches_NextBranchId",
                table: "Order",
                column: "NextBranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Branches_NextBranchId",
                table: "Order");

            migrationBuilder.DropTable(
                name: "MediatorCountry");

            migrationBuilder.DropIndex(
                name: "IX_Order_NextBranchId",
                table: "Order");

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 2 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 4 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 5 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 6 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 7 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 8 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 9 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 10 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 11 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 12 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 13 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 14 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 15 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 16 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 17 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 18 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 19 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 20 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 21 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 22 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 23 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 24 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 25 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 26 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 27 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 28 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 29 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 30 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 31 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 32 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 33 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 34 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 35 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 36 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 37 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 38 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 39 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 40 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 41 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 42 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 43 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 44 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 45 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 46 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 47 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 48 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 49 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 50 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 51 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 52 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 53 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 54 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 55 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 56 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 57 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 58 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 59 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 60 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 61 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 62 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 3, 63 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 2 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 3 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 4 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 5 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 6 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 7 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 8 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 9 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 10 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 11 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 12 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 13 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 14 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 15 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 16 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 17 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 18 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 19 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 20 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 21 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 22 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 23 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 24 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 25 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 26 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 27 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 28 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 29 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 30 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 31 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 32 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 33 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 34 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 35 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 36 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 37 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 38 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 39 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 40 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 41 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 42 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 43 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 44 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 45 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 46 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 47 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 48 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 49 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 50 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 51 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 52 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 53 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 54 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 55 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 56 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 57 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 58 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 59 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 60 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 61 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 62 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 4, 63 });

            migrationBuilder.DeleteData(
                table: "UserBranch",
                keyColumns: new[] { "BranchId", "UserId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "UserBranch",
                keyColumns: new[] { "BranchId", "UserId" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "UserGroup",
                keyColumns: new[] { "GroupId", "UserId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "UserGroup",
                keyColumns: new[] { "GroupId", "UserId" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Group",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Group",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "NextBranchId",
                table: "Order");
        }
    }
}
