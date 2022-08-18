using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class incomeoutcomepaymenteditrequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            int branchId = 2;
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "PaymentWay",
                type: "int",
                nullable: false,
                defaultValue: branchId);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "PaymentRequest",
                type: "int",
                nullable: false,
                defaultValue: branchId);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "OutCome",
                type: "int",
                nullable: false,
                defaultValue: branchId);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "OrderType",
                type: "int",
                nullable: false,
                defaultValue: branchId);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "EditRequest",
                type: "int",
                nullable: false,
                defaultValue: branchId);

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
                name: "IX_EditRequest_BranchId",
                table: "EditRequest",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_EditRequest_Branches_BranchId",
                table: "EditRequest",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EditRequest_Branches_BranchId",
                table: "EditRequest");

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
                name: "IX_EditRequest_BranchId",
                table: "EditRequest");

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
                table: "EditRequest");
        }
    }
}
