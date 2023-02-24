using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class Branches : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApproveAgentEditOrderRequest");

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Receipt",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "PointsSetting",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "PaymentWay",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "PaymentRequest",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "OutCome",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "OrderType",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CurrentBranchId",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "InWayToBranch",
                table: "Order",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReturnedByBranch",
                table: "Order",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "NewCost",
                table: "Order",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewOrderPlacedId",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NextBranchId",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetBranchId",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Income",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Group",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "EditRequest",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "DisAcceptOrder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "ClientPayment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "AgentPrint",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branches_Country_Id",
                        column: x => x.Id,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "TransferToOtherBranches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceBranchId = table.Column<int>(type: "int", nullable: false),
                    DestinationBranchId = table.Column<int>(type: "int", nullable: false),
                    DriverName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrinterName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferToOtherBranches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferToOtherBranches_Branches_DestinationBranchId",
                        column: x => x.DestinationBranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferToOtherBranches_Branches_SourceBranchId",
                        column: x => x.SourceBranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserBranch",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBranch", x => new { x.UserId, x.BranchId });
                    table.ForeignKey(
                        name: "FK_UserBranch_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBranch_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransferToOtherBranchDetials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransferToOtherBranchId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferToOtherBranchDetials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferToOtherBranchDetials_TransferToOtherBranches_TransferToOtherBranchId",
                        column: x => x.TransferToOtherBranchId,
                        principalTable: "TransferToOtherBranches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "الفرع الرئيسي" },
                    { 2, "الفرع الثاني" },
                    { 3, "الفرع الثالث الوسيط" },
                    { 4, "فرع 4" }
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

            migrationBuilder.UpdateData(
                table: "Group",
                keyColumn: "Id",
                keyValue: 1,
                column: "BranchId",
                value: 1);

            migrationBuilder.InsertData(
                table: "Group",
                columns: new[] { "Id", "BranchId", "Name" },
                values: new object[,]
                {
                    { 2, 2, "مجموعة المدراء" },
                    { 3, 3, "مجموعة المدراء" },
                    { 4, 4, "مجموعة المدراء" }
                });

            migrationBuilder.InsertData(
                table: "UserBranch",
                columns: new[] { "BranchId", "UserId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 4, 1 }
                });

            migrationBuilder.InsertData(
                table: "GroupPrivilege",
                columns: new[] { "GroupId", "PrivilegId" },
                values: new object[,]
                {
                    { 2, 1 },
                    { 3, 58 },
                    { 3, 59 },
                    { 3, 60 },
                    { 3, 61 },
                    { 3, 62 },
                    { 3, 63 },
                    { 4, 1 },
                    { 4, 2 },
                    { 4, 3 },
                    { 3, 57 },
                    { 4, 4 },
                    { 4, 6 },
                    { 4, 7 },
                    { 4, 8 },
                    { 4, 9 },
                    { 4, 10 },
                    { 4, 11 },
                    { 4, 12 },
                    { 4, 13 },
                    { 4, 14 },
                    { 4, 5 },
                    { 4, 15 },
                    { 3, 56 },
                    { 3, 54 },
                    { 3, 34 },
                    { 3, 35 },
                    { 3, 36 },
                    { 3, 37 },
                    { 3, 38 },
                    { 3, 39 },
                    { 3, 40 },
                    { 3, 41 },
                    { 3, 42 },
                    { 3, 55 },
                    { 3, 43 },
                    { 3, 45 },
                    { 3, 46 },
                    { 3, 47 },
                    { 3, 48 },
                    { 3, 49 },
                    { 3, 50 }
                });

            migrationBuilder.InsertData(
                table: "GroupPrivilege",
                columns: new[] { "GroupId", "PrivilegId" },
                values: new object[,]
                {
                    { 3, 51 },
                    { 3, 52 },
                    { 3, 53 },
                    { 3, 44 },
                    { 3, 33 },
                    { 4, 16 },
                    { 4, 18 },
                    { 4, 43 },
                    { 4, 44 },
                    { 4, 45 },
                    { 4, 46 },
                    { 4, 47 },
                    { 4, 48 },
                    { 4, 49 },
                    { 4, 50 },
                    { 4, 51 },
                    { 4, 42 },
                    { 4, 52 },
                    { 4, 54 },
                    { 4, 55 },
                    { 4, 56 },
                    { 4, 57 },
                    { 4, 58 },
                    { 4, 59 },
                    { 4, 60 },
                    { 4, 61 },
                    { 4, 62 },
                    { 4, 53 },
                    { 4, 17 },
                    { 4, 41 },
                    { 4, 39 },
                    { 4, 19 },
                    { 4, 20 },
                    { 4, 21 },
                    { 4, 22 },
                    { 4, 23 },
                    { 4, 24 },
                    { 4, 25 },
                    { 4, 26 },
                    { 4, 27 },
                    { 4, 40 },
                    { 4, 28 }
                });

            migrationBuilder.InsertData(
                table: "GroupPrivilege",
                columns: new[] { "GroupId", "PrivilegId" },
                values: new object[,]
                {
                    { 4, 30 },
                    { 4, 31 },
                    { 4, 32 },
                    { 4, 33 },
                    { 4, 34 },
                    { 4, 35 },
                    { 4, 36 },
                    { 4, 37 },
                    { 4, 38 },
                    { 4, 29 },
                    { 4, 63 },
                    { 3, 32 },
                    { 3, 30 },
                    { 2, 26 },
                    { 2, 27 },
                    { 2, 28 },
                    { 2, 29 },
                    { 2, 30 },
                    { 2, 31 },
                    { 2, 32 },
                    { 2, 33 },
                    { 2, 34 },
                    { 2, 25 },
                    { 2, 35 },
                    { 2, 37 },
                    { 2, 38 },
                    { 2, 39 },
                    { 2, 40 },
                    { 2, 41 },
                    { 2, 42 },
                    { 2, 43 },
                    { 2, 44 },
                    { 2, 45 },
                    { 2, 36 },
                    { 2, 46 },
                    { 2, 24 },
                    { 2, 22 },
                    { 2, 2 },
                    { 2, 3 },
                    { 2, 4 },
                    { 2, 5 },
                    { 2, 6 }
                });

            migrationBuilder.InsertData(
                table: "GroupPrivilege",
                columns: new[] { "GroupId", "PrivilegId" },
                values: new object[,]
                {
                    { 2, 7 },
                    { 2, 8 },
                    { 2, 9 },
                    { 2, 10 },
                    { 2, 23 },
                    { 2, 11 },
                    { 2, 13 },
                    { 2, 14 },
                    { 2, 15 },
                    { 2, 16 },
                    { 2, 17 },
                    { 2, 18 },
                    { 2, 19 },
                    { 2, 20 },
                    { 2, 21 },
                    { 2, 12 },
                    { 2, 47 },
                    { 2, 48 },
                    { 2, 49 },
                    { 3, 10 },
                    { 3, 11 },
                    { 3, 12 },
                    { 3, 13 },
                    { 3, 14 },
                    { 3, 15 },
                    { 3, 16 },
                    { 3, 17 },
                    { 3, 18 },
                    { 3, 9 },
                    { 3, 19 },
                    { 3, 21 },
                    { 3, 22 },
                    { 3, 23 },
                    { 3, 24 },
                    { 3, 25 },
                    { 3, 26 },
                    { 3, 27 },
                    { 3, 28 },
                    { 3, 29 },
                    { 3, 20 },
                    { 3, 8 },
                    { 3, 7 }
                });

            migrationBuilder.InsertData(
                table: "GroupPrivilege",
                columns: new[] { "GroupId", "PrivilegId" },
                values: new object[,]
                {
                    { 3, 6 },
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
                    { 2, 63 },
                    { 3, 1 },
                    { 3, 2 },
                    { 3, 3 },
                    { 3, 4 },
                    { 3, 5 },
                    { 3, 31 }
                });

            migrationBuilder.InsertData(
                table: "UserGroup",
                columns: new[] { "GroupId", "UserId" },
                values: new object[,]
                {
                    { 3, 1 },
                    { 2, 1 },
                    { 4, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_BranchId",
                table: "Users",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Receipt_BranchId",
                table: "Receipt",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_PointsSetting_BranchId",
                table: "PointsSetting",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentWay_BranchId",
                table: "PaymentWay",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRequest_BranchId",
                table: "PaymentRequest",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_OutCome_BranchId",
                table: "OutCome",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderType_BranchId",
                table: "OrderType",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_BranchId",
                table: "Order",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CurrentBranchId",
                table: "Order",
                column: "CurrentBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_NewOrderPlacedId",
                table: "Order",
                column: "NewOrderPlacedId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_NextBranchId",
                table: "Order",
                column: "NextBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_TargetBranchId",
                table: "Order",
                column: "TargetBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Income_BranchId",
                table: "Income",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Group_BranchId",
                table: "Group",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_EditRequest_BranchId",
                table: "EditRequest",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_DisAcceptOrder_BranchId",
                table: "DisAcceptOrder",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_BranchId",
                table: "Clients",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientPayment_BranchId",
                table: "ClientPayment",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_AgentPrint_BranchId",
                table: "AgentPrint",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_MediatorCountry_MediatorCountryId",
                table: "MediatorCountry",
                column: "MediatorCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_MediatorCountry_ToCountryId",
                table: "MediatorCountry",
                column: "ToCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferToOtherBranchDetials_TransferToOtherBranchId",
                table: "TransferToOtherBranchDetials",
                column: "TransferToOtherBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferToOtherBranches_DestinationBranchId",
                table: "TransferToOtherBranches",
                column: "DestinationBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferToOtherBranches_SourceBranchId",
                table: "TransferToOtherBranches",
                column: "SourceBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBranch_BranchId",
                table: "UserBranch",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_AgentPrint_Branches_BranchId",
                table: "AgentPrint",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientPayment_Branches_BranchId",
                table: "ClientPayment",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Branches_BranchId",
                table: "Clients",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DisAcceptOrder_Branches_BranchId",
                table: "DisAcceptOrder",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EditRequest_Branches_BranchId",
                table: "EditRequest",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Group_Branches_BranchId",
                table: "Group",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Income_Branches_BranchId",
                table: "Income",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Branches_BranchId",
                table: "Order",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Branches_CurrentBranchId",
                table: "Order",
                column: "CurrentBranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Branches_NextBranchId",
                table: "Order",
                column: "NextBranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Branches_TargetBranchId",
                table: "Order",
                column: "TargetBranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_OrderPlaced_NewOrderPlacedId",
                table: "Order",
                column: "NewOrderPlacedId",
                principalTable: "OrderPlaced",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderType_Branches_BranchId",
                table: "OrderType",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OutCome_Branches_BranchId",
                table: "OutCome",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentRequest_Branches_BranchId",
                table: "PaymentRequest",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentWay_Branches_BranchId",
                table: "PaymentWay",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PointsSetting_Branches_BranchId",
                table: "PointsSetting",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Receipt_Branches_BranchId",
                table: "Receipt",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Branches_BranchId",
                table: "Users",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgentPrint_Branches_BranchId",
                table: "AgentPrint");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientPayment_Branches_BranchId",
                table: "ClientPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Branches_BranchId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_DisAcceptOrder_Branches_BranchId",
                table: "DisAcceptOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_EditRequest_Branches_BranchId",
                table: "EditRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_Group_Branches_BranchId",
                table: "Group");

            migrationBuilder.DropForeignKey(
                name: "FK_Income_Branches_BranchId",
                table: "Income");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Branches_BranchId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Branches_CurrentBranchId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Branches_NextBranchId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Branches_TargetBranchId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_OrderPlaced_NewOrderPlacedId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderType_Branches_BranchId",
                table: "OrderType");

            migrationBuilder.DropForeignKey(
                name: "FK_OutCome_Branches_BranchId",
                table: "OutCome");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentRequest_Branches_BranchId",
                table: "PaymentRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentWay_Branches_BranchId",
                table: "PaymentWay");

            migrationBuilder.DropForeignKey(
                name: "FK_PointsSetting_Branches_BranchId",
                table: "PointsSetting");

            migrationBuilder.DropForeignKey(
                name: "FK_Receipt_Branches_BranchId",
                table: "Receipt");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Branches_BranchId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "MediatorCountry");

            migrationBuilder.DropTable(
                name: "TransferToOtherBranchDetials");

            migrationBuilder.DropTable(
                name: "UserBranch");

            migrationBuilder.DropTable(
                name: "TransferToOtherBranches");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Users_BranchId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Receipt_BranchId",
                table: "Receipt");

            migrationBuilder.DropIndex(
                name: "IX_PointsSetting_BranchId",
                table: "PointsSetting");

            migrationBuilder.DropIndex(
                name: "IX_PaymentWay_BranchId",
                table: "PaymentWay");

            migrationBuilder.DropIndex(
                name: "IX_PaymentRequest_BranchId",
                table: "PaymentRequest");

            migrationBuilder.DropIndex(
                name: "IX_OutCome_BranchId",
                table: "OutCome");

            migrationBuilder.DropIndex(
                name: "IX_OrderType_BranchId",
                table: "OrderType");

            migrationBuilder.DropIndex(
                name: "IX_Order_BranchId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_CurrentBranchId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_NewOrderPlacedId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_NextBranchId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_TargetBranchId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Income_BranchId",
                table: "Income");

            migrationBuilder.DropIndex(
                name: "IX_Group_BranchId",
                table: "Group");

            migrationBuilder.DropIndex(
                name: "IX_EditRequest_BranchId",
                table: "EditRequest");

            migrationBuilder.DropIndex(
                name: "IX_DisAcceptOrder_BranchId",
                table: "DisAcceptOrder");

            migrationBuilder.DropIndex(
                name: "IX_Clients_BranchId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_ClientPayment_BranchId",
                table: "ClientPayment");

            migrationBuilder.DropIndex(
                name: "IX_AgentPrint_BranchId",
                table: "AgentPrint");

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
                table: "UserGroup",
                keyColumns: new[] { "GroupId", "UserId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "UserGroup",
                keyColumns: new[] { "GroupId", "UserId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "UserGroup",
                keyColumns: new[] { "GroupId", "UserId" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "Group",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Group",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Group",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Receipt");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "PointsSetting");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "PaymentWay");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "PaymentRequest");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "OutCome");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "OrderType");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "CurrentBranchId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "InWayToBranch",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "IsReturnedByBranch",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "NewCost",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "NewOrderPlacedId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "NextBranchId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "TargetBranchId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Income");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Group");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "EditRequest");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "DisAcceptOrder");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "ClientPayment");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "AgentPrint");

            migrationBuilder.CreateTable(
                name: "ApproveAgentEditOrderRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentId = table.Column<int>(type: "int", nullable: false),
                    IsApprove = table.Column<bool>(type: "bit", nullable: true),
                    NewAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    OrderPlacedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApproveAgentEditOrderRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK__ApproveAg__Agent__0B91BA14",
                        column: x => x.AgentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__ApproveAg__Order__0C85DE4D",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__ApproveAg__Order__0D7A0286",
                        column: x => x.OrderPlacedId,
                        principalTable: "OrderPlaced",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApproveAgentEditOrderRequest_AgentId",
                table: "ApproveAgentEditOrderRequest",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_ApproveAgentEditOrderRequest_OrderId",
                table: "ApproveAgentEditOrderRequest",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ApproveAgentEditOrderRequest_OrderPlacedId",
                table: "ApproveAgentEditOrderRequest",
                column: "OrderPlacedId");
        }
    }
}
