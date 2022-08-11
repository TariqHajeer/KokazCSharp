using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class DeleteAgentRequestTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApproveAgentEditOrderRequest");

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

            migrationBuilder.CreateIndex(
                name: "IX_Order_NewOrderPlacedId",
                table: "Order",
                column: "NewOrderPlacedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_OrderPlaced_NewOrderPlacedId",
                table: "Order",
                column: "NewOrderPlacedId",
                principalTable: "OrderPlaced",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_OrderPlaced_NewOrderPlacedId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_NewOrderPlacedId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "NewCost",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "NewOrderPlacedId",
                table: "Order");

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
