using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class InitalCatalog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AgentPrint",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrinterName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    DestinationName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DestinationPhone = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentPrint", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientPayment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrinterName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    DestinationName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DestinationPhone = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientPayment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DeliveryCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    mediatorId = table.Column<int>(type: "int", nullable: true),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Country__mediato__74794A92",
                        column: x => x.mediatorId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IncomeType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MoenyPlaced",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoenyPlaced", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderPlaced",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPlaced", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderState",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderState", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutComeType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutComeType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentWay",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentWay", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PointsSetting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Points = table.Column<int>(type: "int", nullable: false),
                    Money = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointsSetting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Privilege",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SysName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Privilege", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Experince = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HireDate = table.Column<DateTime>(type: "date", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CanWorkAsAgent = table.Column<bool>(type: "bit", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AgentPrintDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ClientName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AgentPrintId = table.Column<int>(type: "int", nullable: false),
                    ClientNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentPrintDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK__AgentPrin__Agent__09A971A2",
                        column: x => x.AgentPrintId,
                        principalTable: "AgentPrint",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Discount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Points = table.Column<int>(type: "int", nullable: false),
                    Money = table.Column<decimal>(type: "money", nullable: false),
                    ClientPaymentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discount", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Discount__Client__56E8E7AB",
                        column: x => x.ClientPaymentId,
                        principalTable: "ClientPayment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Region",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reginos_Country",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientPaymentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientPaymentId = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeliveryCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MoneyPlacedId = table.Column<int>(type: "int", nullable: true),
                    OrderPlacedId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "date", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayForClient = table.Column<decimal>(type: "money", nullable: true),
                    ClientNote = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientPaymentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK__ClientPay__Clien__51300E55",
                        column: x => x.ClientPaymentId,
                        principalTable: "ClientPayment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__ClientPay__Money__5224328E",
                        column: x => x.MoneyPlacedId,
                        principalTable: "MoenyPlaced",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__ClientPay__Order__531856C7",
                        column: x => x.OrderPlacedId,
                        principalTable: "OrderPlaced",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupPrivilege",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    PrivilegId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupPrivilege", x => new { x.GroupId, x.PrivilegId });
                    table.ForeignKey(
                        name: "FK_GroupPrivilege_Group",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupPrivilege_Privilege",
                        column: x => x.PrivilegId,
                        principalTable: "Privilege",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AgentCountry",
                columns: table => new
                {
                    AgentId = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentCountr", x => new { x.AgentId, x.CountryId });
                    table.ForeignKey(
                        name: "FK_AgentCountr_Country",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgentCountr_Users",
                        column: x => x.AgentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FirstDate = table.Column<DateTime>(type: "date", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Mail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Points = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_Country",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Clients_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Income",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Earining = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userId = table.Column<int>(type: "int", nullable: false),
                    IncomeTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Income", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Income_IncomeType",
                        column: x => x.IncomeTypeId,
                        principalTable: "IncomeType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Income_Users",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OutCome",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userId = table.Column<int>(type: "int", nullable: false),
                    OutComeTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutCome", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutCome_OutComeType",
                        column: x => x.OutComeTypeId,
                        principalTable: "OutComeType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutCome_Users",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptOfTheOrderStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "date", nullable: false),
                    RecvierId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptOfTheOrderStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK__ReceiptOf__Recvi__5B438874",
                        column: x => x.RecvierId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Treasury",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreateOnUtc = table.Column<DateTime>(type: "date", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treasury", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Treasury__Id__60FC61CA",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserGroup",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroup", x => new { x.UserId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_UserGroup_Group",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroup_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPhone",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPhone", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPhone_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "clientPhones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Phone = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clientPhones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_clientPhones_Clients",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DisAcceptOrder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    DeliveryCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RecipientName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RecipientPhones = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "date", nullable: true),
                    IsDollar = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisAcceptOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK__DisAccept__Clien__151B244E",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__DisAccept__Count__0D7A0286",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__DisAccept__Regio__17036CC0",
                        column: x => x.RegionId,
                        principalTable: "Region",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EditRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    OldName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NewName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OldUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NewUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Accept = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EditRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK__EditReque__Clien__18EBB532",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__EditReque__UserI__19DFD96B",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Market",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarketUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Market", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Market__ClientId__1EA48E88",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notfication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderCount = table.Column<int>(type: "int", nullable: true),
                    OrderPlacedId = table.Column<int>(type: "int", nullable: true),
                    MoneyPlacedId = table.Column<int>(type: "int", nullable: true),
                    IsSeen = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notfication", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Notficati__Clien__1F98B2C1",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Notficati__Money__208CD6FA",
                        column: x => x.MoneyPlacedId,
                        principalTable: "MoenyPlaced",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Notficati__Order__2180FB33",
                        column: x => x.OrderPlacedId,
                        principalTable: "OrderPlaced",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    DeliveryCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OldCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AgentCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RecipientName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RecipientPhones = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoenyPlacedId = table.Column<int>(type: "int", nullable: false),
                    OrderplacedId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    DiliveryDate = table.Column<DateTime>(type: "date", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgentId = table.Column<int>(type: "int", nullable: true),
                    seen = table.Column<bool>(type: "bit", nullable: true),
                    IsClientDiliverdMoney = table.Column<bool>(type: "bit", nullable: false),
                    IsSync = table.Column<bool>(type: "bit", nullable: false),
                    OrderStateId = table.Column<int>(type: "int", nullable: false),
                    IsDollar = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    SystemNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldDeliveryCost = table.Column<decimal>(type: "money", nullable: true),
                    IsSend = table.Column<bool>(type: "bit", nullable: true),
                    ClientPaied = table.Column<decimal>(type: "money", nullable: true),
                    CurrentCountry = table.Column<int>(type: "int", nullable: true),
                    PrintedTimes = table.Column<int>(type: "int", nullable: false),
                    AgentRequestStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Order__CurrentCo__078C1F06",
                        column: x => x.CurrentCountry,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_Clients",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_Country",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_MoenyPlaced",
                        column: x => x.MoenyPlacedId,
                        principalTable: "MoenyPlaced",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_OrderPlaced",
                        column: x => x.OrderplacedId,
                        principalTable: "OrderPlaced",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_OrderState",
                        column: x => x.OrderStateId,
                        principalTable: "OrderState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_Region",
                        column: x => x.RegionId,
                        principalTable: "Region",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_Users",
                        column: x => x.AgentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderFromExcel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RecipientName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderFromExcel", x => x.Id);
                    table.ForeignKey(
                        name: "FK__OrderFrom__Clien__2A164134",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    PaymentWayId = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Accept = table.Column<bool>(type: "bit", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK__PaymentRe__Clien__5AB9788F",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__PaymentRe__Payme__5BAD9CC8",
                        column: x => x.PaymentWayId,
                        principalTable: "PaymentWay",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Receipt",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    About = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Manager = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPay = table.Column<bool>(type: "bit", nullable: false),
                    ClientPaymentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receipt", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Receipt__ClientP__57DD0BE4",
                        column: x => x.ClientPaymentId,
                        principalTable: "ClientPayment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Receipt_Clients",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CashMovment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TreasuryId = table.Column<int>(type: "int", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "date", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashMovment", x => x.Id);
                    table.ForeignKey(
                        name: "FK__CashMovme__Treas__1975C517",
                        column: x => x.TreasuryId,
                        principalTable: "Treasury",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AgentOrderPrint",
                columns: table => new
                {
                    AgentPrintId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AgentOrd__CAFCEB4A3800EF29", x => new { x.OrderId, x.AgentPrintId });
                    table.ForeignKey(
                        name: "FK__AgentOrde__Agent__07C12930",
                        column: x => x.AgentPrintId,
                        principalTable: "AgentPrint",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__AgentOrde__Order__08B54D69",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApproveAgentEditOrderRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    OrderPlacedId = table.Column<int>(type: "int", nullable: false),
                    NewAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AgentId = table.Column<int>(type: "int", nullable: false),
                    IsApprove = table.Column<bool>(type: "bit", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "OrderClientPaymnet",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ClientPaymentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Pk_OrderClientPaymnet", x => new { x.OrderId, x.ClientPaymentId });
                    table.ForeignKey(
                        name: "FK__OrderClie__Clien__55F4C372",
                        column: x => x.ClientPaymentId,
                        principalTable: "ClientPayment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__OrderClie__Order__55009F39",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    OrderTpyeId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderOrderType", x => new { x.OrderId, x.OrderTpyeId });
                    table.ForeignKey(
                        name: "FK_OrderOrderType_Order",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderOrderType_OrderType",
                        column: x => x.OrderTpyeId,
                        principalTable: "OrderType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    DeliveryCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OldCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AgentCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RecipientName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RecipientPhones = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoenyPlacedId = table.Column<int>(type: "int", nullable: false),
                    OrderplacedId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: true),
                    DiliveryDate = table.Column<DateTime>(type: "date", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgentId = table.Column<int>(type: "int", nullable: true),
                    seen = table.Column<bool>(type: "bit", nullable: true),
                    IsClientDiliverdMoney = table.Column<bool>(type: "bit", nullable: false),
                    IsSync = table.Column<bool>(type: "bit", nullable: false),
                    OrderStateId = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    IsDollar = table.Column<bool>(type: "bit", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    SystemNote = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderLog_Clients",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderLog_Country",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderLog_MoenyPlaced",
                        column: x => x.MoenyPlacedId,
                        principalTable: "MoenyPlaced",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderLog_Order",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderLog_OrderPlaced",
                        column: x => x.OrderplacedId,
                        principalTable: "OrderPlaced",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderLog_Region",
                        column: x => x.RegionId,
                        principalTable: "Region",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderLog_Users",
                        column: x => x.AgentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptOfTheOrderStatusDetalis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AgentCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AgentId = table.Column<int>(type: "int", nullable: false),
                    OrderStateId = table.Column<int>(type: "int", nullable: false),
                    MoneyPlacedId = table.Column<int>(type: "int", nullable: false),
                    ReceiptOfTheOrderStatusId = table.Column<int>(type: "int", nullable: false),
                    OrderPlacedId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptOfTheOrderStatusDetalis", x => x.Id);
                    table.ForeignKey(
                        name: "FK__ReceiptOf__Agent__23F3538A",
                        column: x => x.AgentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__ReceiptOf__Clien__595B4002",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__ReceiptOf__Money__25DB9BFC",
                        column: x => x.MoneyPlacedId,
                        principalTable: "MoenyPlaced",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__ReceiptOf__Order__24E777C3",
                        column: x => x.OrderStateId,
                        principalTable: "OrderState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__ReceiptOf__Order__5E1FF51F",
                        column: x => x.OrderPlacedId,
                        principalTable: "OrderPlaced",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__ReceiptOf__Order__6B79F03D",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__ReceiptOf__Recei__26CFC035",
                        column: x => x.ReceiptOfTheOrderStatusId,
                        principalTable: "ReceiptOfTheOrderStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TreasuryHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TreasuryId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "date", nullable: false),
                    ClientPaymentId = table.Column<int>(type: "int", nullable: true),
                    CashMovmentId = table.Column<int>(type: "int", nullable: true),
                    ReceiptId = table.Column<int>(type: "int", nullable: true),
                    ReceiptOfTheOrderStatusId = table.Column<int>(type: "int", nullable: true),
                    IncomeId = table.Column<int>(type: "int", nullable: true),
                    OutcomeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreasuryHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK__TreasuryH__CashM__1E3A7A34",
                        column: x => x.CashMovmentId,
                        principalTable: "CashMovment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__TreasuryH__Clien__65C116E7",
                        column: x => x.ClientPaymentId,
                        principalTable: "ClientPayment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__TreasuryH__Incom__6991A7CB",
                        column: x => x.IncomeId,
                        principalTable: "Income",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__TreasuryH__Outco__6A85CC04",
                        column: x => x.OutcomeId,
                        principalTable: "OutCome",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__TreasuryH__Recei__1F2E9E6D",
                        column: x => x.ReceiptId,
                        principalTable: "Receipt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__TreasuryH__Recei__27C3E46E",
                        column: x => x.ReceiptOfTheOrderStatusId,
                        principalTable: "ReceiptOfTheOrderStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__TreasuryH__Treas__1C5231C2",
                        column: x => x.TreasuryId,
                        principalTable: "Treasury",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgentCountry_CountryId",
                table: "AgentCountry",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_AgentOrderPrint_AgentPrintId",
                table: "AgentOrderPrint",
                column: "AgentPrintId");

            migrationBuilder.CreateIndex(
                name: "IX_AgentPrintDetails_AgentPrintId",
                table: "AgentPrintDetails",
                column: "AgentPrintId");

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

            migrationBuilder.CreateIndex(
                name: "IX_CashMovment_TreasuryId",
                table: "CashMovment",
                column: "TreasuryId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientPaymentDetails_ClientPaymentId",
                table: "ClientPaymentDetails",
                column: "ClientPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientPaymentDetails_MoneyPlacedId",
                table: "ClientPaymentDetails",
                column: "MoneyPlacedId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientPaymentDetails_OrderPlacedId",
                table: "ClientPaymentDetails",
                column: "OrderPlacedId");

            migrationBuilder.CreateIndex(
                name: "IX_clientPhones_ClientId",
                table: "clientPhones",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CountryId",
                table: "Clients",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_UserId",
                table: "Clients",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Country_mediatorId",
                table: "Country",
                column: "mediatorId");

            migrationBuilder.CreateIndex(
                name: "IX_DisAcceptOrder_ClientId",
                table: "DisAcceptOrder",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_DisAcceptOrder_CountryId",
                table: "DisAcceptOrder",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_DisAcceptOrder_RegionId",
                table: "DisAcceptOrder",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Discount_ClientPaymentId",
                table: "Discount",
                column: "ClientPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_EditRequest_ClientId",
                table: "EditRequest",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_EditRequest_UserId",
                table: "EditRequest",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPrivilege_PrivilegId",
                table: "GroupPrivilege",
                column: "PrivilegId");

            migrationBuilder.CreateIndex(
                name: "IX_Income_IncomeTypeId",
                table: "Income",
                column: "IncomeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Income_userId",
                table: "Income",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Market_ClientId",
                table: "Market",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Notfication_ClientId",
                table: "Notfication",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Notfication_MoneyPlacedId",
                table: "Notfication",
                column: "MoneyPlacedId");

            migrationBuilder.CreateIndex(
                name: "IX_Notfication_OrderPlacedId",
                table: "Notfication",
                column: "OrderPlacedId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_AgentId",
                table: "Order",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ClientId",
                table: "Order",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CountryId",
                table: "Order",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CurrentCountry",
                table: "Order",
                column: "CurrentCountry");

            migrationBuilder.CreateIndex(
                name: "IX_Order_MoenyPlacedId",
                table: "Order",
                column: "MoenyPlacedId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_OrderplacedId",
                table: "Order",
                column: "OrderplacedId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_OrderStateId",
                table: "Order",
                column: "OrderStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_RegionId",
                table: "Order",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderClientPaymnet_ClientPaymentId",
                table: "OrderClientPaymnet",
                column: "ClientPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderFromExcel_ClientId",
                table: "OrderFromExcel",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderTpyeId",
                table: "OrderItem",
                column: "OrderTpyeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLog_AgentId",
                table: "OrderLog",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLog_ClientId",
                table: "OrderLog",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLog_CountryId",
                table: "OrderLog",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLog_MoenyPlacedId",
                table: "OrderLog",
                column: "MoenyPlacedId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLog_OrderId",
                table: "OrderLog",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLog_OrderplacedId",
                table: "OrderLog",
                column: "OrderplacedId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLog_RegionId",
                table: "OrderLog",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_OutCome_OutComeTypeId",
                table: "OutCome",
                column: "OutComeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OutCome_userId",
                table: "OutCome",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRequest_ClientId",
                table: "PaymentRequest",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRequest_PaymentWayId",
                table: "PaymentRequest",
                column: "PaymentWayId");

            migrationBuilder.CreateIndex(
                name: "UQ__PointsSe__DA826786C9B4659A",
                table: "PointsSetting",
                column: "Points",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__PointsSe__FA951B46C519FCD7",
                table: "PointsSetting",
                column: "Money",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Receipt_ClientId",
                table: "Receipt",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Receipt_ClientPaymentId",
                table: "Receipt",
                column: "ClientPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptOfTheOrderStatus_RecvierId",
                table: "ReceiptOfTheOrderStatus",
                column: "RecvierId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptOfTheOrderStatusDetalis_AgentId",
                table: "ReceiptOfTheOrderStatusDetalis",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptOfTheOrderStatusDetalis_ClientId",
                table: "ReceiptOfTheOrderStatusDetalis",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptOfTheOrderStatusDetalis_MoneyPlacedId",
                table: "ReceiptOfTheOrderStatusDetalis",
                column: "MoneyPlacedId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptOfTheOrderStatusDetalis_OrderId",
                table: "ReceiptOfTheOrderStatusDetalis",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptOfTheOrderStatusDetalis_OrderPlacedId",
                table: "ReceiptOfTheOrderStatusDetalis",
                column: "OrderPlacedId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptOfTheOrderStatusDetalis_OrderStateId",
                table: "ReceiptOfTheOrderStatusDetalis",
                column: "OrderStateId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptOfTheOrderStatusDetalis_ReceiptOfTheOrderStatusId",
                table: "ReceiptOfTheOrderStatusDetalis",
                column: "ReceiptOfTheOrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Region_CountryId",
                table: "Region",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_TreasuryHistory_CashMovmentId",
                table: "TreasuryHistory",
                column: "CashMovmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TreasuryHistory_ClientPaymentId",
                table: "TreasuryHistory",
                column: "ClientPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_TreasuryHistory_IncomeId",
                table: "TreasuryHistory",
                column: "IncomeId");

            migrationBuilder.CreateIndex(
                name: "IX_TreasuryHistory_OutcomeId",
                table: "TreasuryHistory",
                column: "OutcomeId");

            migrationBuilder.CreateIndex(
                name: "IX_TreasuryHistory_ReceiptId",
                table: "TreasuryHistory",
                column: "ReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_TreasuryHistory_ReceiptOfTheOrderStatusId",
                table: "TreasuryHistory",
                column: "ReceiptOfTheOrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TreasuryHistory_TreasuryId",
                table: "TreasuryHistory",
                column: "TreasuryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroup_GroupId",
                table: "UserGroup",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPhone_UserId",
                table: "UserPhone",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgentCountry");

            migrationBuilder.DropTable(
                name: "AgentOrderPrint");

            migrationBuilder.DropTable(
                name: "AgentPrintDetails");

            migrationBuilder.DropTable(
                name: "ApproveAgentEditOrderRequest");

            migrationBuilder.DropTable(
                name: "ClientPaymentDetails");

            migrationBuilder.DropTable(
                name: "clientPhones");

            migrationBuilder.DropTable(
                name: "DisAcceptOrder");

            migrationBuilder.DropTable(
                name: "Discount");

            migrationBuilder.DropTable(
                name: "EditRequest");

            migrationBuilder.DropTable(
                name: "GroupPrivilege");

            migrationBuilder.DropTable(
                name: "Market");

            migrationBuilder.DropTable(
                name: "Notfication");

            migrationBuilder.DropTable(
                name: "OrderClientPaymnet");

            migrationBuilder.DropTable(
                name: "OrderFromExcel");

            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.DropTable(
                name: "OrderLog");

            migrationBuilder.DropTable(
                name: "PaymentRequest");

            migrationBuilder.DropTable(
                name: "PointsSetting");

            migrationBuilder.DropTable(
                name: "ReceiptOfTheOrderStatusDetalis");

            migrationBuilder.DropTable(
                name: "TreasuryHistory");

            migrationBuilder.DropTable(
                name: "UserGroup");

            migrationBuilder.DropTable(
                name: "UserPhone");

            migrationBuilder.DropTable(
                name: "AgentPrint");

            migrationBuilder.DropTable(
                name: "Privilege");

            migrationBuilder.DropTable(
                name: "OrderType");

            migrationBuilder.DropTable(
                name: "PaymentWay");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "CashMovment");

            migrationBuilder.DropTable(
                name: "Income");

            migrationBuilder.DropTable(
                name: "OutCome");

            migrationBuilder.DropTable(
                name: "Receipt");

            migrationBuilder.DropTable(
                name: "ReceiptOfTheOrderStatus");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropTable(
                name: "MoenyPlaced");

            migrationBuilder.DropTable(
                name: "OrderPlaced");

            migrationBuilder.DropTable(
                name: "OrderState");

            migrationBuilder.DropTable(
                name: "Region");

            migrationBuilder.DropTable(
                name: "Treasury");

            migrationBuilder.DropTable(
                name: "IncomeType");

            migrationBuilder.DropTable(
                name: "OutComeType");

            migrationBuilder.DropTable(
                name: "ClientPayment");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
