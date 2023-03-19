using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quqaz.Web.Migrations
{
    public partial class SettingsSeeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "Id", "DeliveryCost", "IsMain", "mediatorId", "Name", "Points" },
                values: new object[,]
                {
                    { 1, 5000m, false, null, "خارجي اربيل", 2 },
                    { 28, 5000m, false, null, "مخمور", 0 },
                    { 29, 5000m, false, null, "مصيف", 0 },
                    { 30, 5000m, false, null, "سوران", 0 },
                    { 31, 5000m, false, null, "كوية", 0 },
                    { 33, 5000m, false, null, "ملا عمر", 0 },
                    { 34, 5000m, false, null, "بارازان", 0 },
                    { 35, 5000m, false, null, "سلافا ستي", 0 },
                    { 36, 5000m, false, null, "كوير", 0 },
                    { 37, 5000m, false, null, "برده رش", 0 },
                    { 38, 5000m, false, null, "ديانا", 0 },
                    { 39, 5000m, false, null, "خليفان", 0 },
                    { 40, 5000m, false, null, "راوندوز", 1 },
                    { 41, 5000m, false, null, "رانيا", 2 },
                    { 42, 5000m, false, null, "طق طق", 2 },
                    { 43, 6000m, false, null, "قلادزي", 1 },
                    { 44, 4000m, false, null, "كرخ محمد", 1 },
                    { 45, 8000m, false, null, "عمار بابل", 0 },
                    { 46, 6000m, false, null, "الرصافة", 2 },
                    { 47, 6000m, false, null, "كرخ 2", 5 },
                    { 48, 6000m, false, null, "قوقز فرع بغداد", 0 },
                    { 49, 8000m, false, null, "سماوة", 0 },
                    { 27, 7000m, false, null, "سيروان", 0 },
                    { 26, 8000m, false, null, "ميسان", 4 },
                    { 32, 5000m, false, null, "شقلاوة", 0 },
                    { 24, 8000m, false, null, "كوت", 4 },
                    { 2, 5000m, false, null, "دهوك", 2 },
                    { 3, 5000m, false, null, "سليمانية", 2 },
                    { 4, 5000m, false, null, "موصل", 2 },
                    { 5, 6000m, false, null, "بغداد", 3 },
                    { 6, 8000m, false, null, "محافظات جنوبية", 0 },
                    { 25, 8000m, false, null, "ذي قار", 4 },
                    { 8, 4000m, false, null, "اربيل", 2 },
                    { 9, 5000m, false, null, "خبات", 1 },
                    { 10, 8000m, false, null, "انبار", 3 },
                    { 11, 8000m, false, null, "تكريت", 4 },
                    { 12, 8000m, false, null, "ديالى", 3 },
                    { 7, 5000m, false, null, "كركوك", 4 },
                    { 14, 8000m, false, null, "نجف", 3 },
                    { 15, 8000m, false, null, "كربلاء", 3 },
                    { 16, 8000m, false, null, "ديوانية", 4 },
                    { 17, 8000m, false, null, "مثنى", 4 }
                });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "Id", "DeliveryCost", "IsMain", "mediatorId", "Name", "Points" },
                values: new object[,]
                {
                    { 18, 8000m, false, null, "صلاح الدين", 4 },
                    { 19, 8000m, false, null, "سامراء", 4 },
                    { 20, 8000m, false, null, "بصره", 3 },
                    { 21, 8000m, false, null, "عمارة", 4 },
                    { 22, 8000m, false, null, "ناصرية", 4 },
                    { 23, 8000m, false, null, "بابل", 3 },
                    { 13, 8000m, false, null, "واسط", 4 }
                });

            migrationBuilder.InsertData(
                table: "Group",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "مجموعة المدراء" });

            migrationBuilder.InsertData(
                table: "MoenyPlaced",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 4, "تم تسليمها" },
                    { 3, "داخل الشركة" },
                    { 1, "خارج الشركة" },
                    { 2, "مندوب" }
                });

            migrationBuilder.InsertData(
                table: "OrderPlaced",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 8, "مؤجل" },
                    { 1, "عند العميل" },
                    { 2, "في المخزن" },
                    { 3, "في الطريق" },
                    { 4, "تم التسليم" },
                    { 5, "مرتجع كلي" },
                    { 6, "مرتجع جزئي" },
                    { 7, "مرفوض" }
                });

            migrationBuilder.InsertData(
                table: "OrderState",
                columns: new[] { "Id", "State" },
                values: new object[,]
                {
                    { 2, "يحب اخذ النقود من العميل" },
                    { 3, "منتهية" },
                    { 1, "قيد المعالجة" }
                });

            migrationBuilder.InsertData(
                table: "Privilege",
                columns: new[] { "Id", "Name", "SysName" },
                values: new object[,]
                {
                    { 34, "تعديل انواع الواردات", "UpdateIncomeType" },
                    { 35, "حذف انواع الواردات", "DeleteIncomeType" },
                    { 36, "عرض انواع الصادرات", "ShowOutComeType" },
                    { 37, "إضافة انواع الصادرات", "AddOutComeType" },
                    { 38, "تعديل انواع الصادرات", "UpdateOutComeType" },
                    { 39, "حذف انواع الصادرات", "DeleteOutComeType" },
                    { 40, "إضافة طلبات", "AddOrder" },
                    { 41, "تعديل الطلبات", "UpdateOrder" },
                    { 42, "حذف الطلبات", "DeleteOrder" },
                    { 43, "عرض الطلبات", "ShowOrder" },
                    { 44, "اضافة صادرات", "AddOutCome" },
                    { 45, "عرض الصادرات", "ShowOutCome" },
                    { 46, "تعديل الصادرات", "UpdateOutCome" },
                    { 47, "حذف الصادرات", "DeleteOutCome" },
                    { 48, "إضافة واردات", "AddIncome" },
                    { 49, "عرض الواردات", "ShowIncome" },
                    { 50, "تعديل الواردات", "UpdateIncome" },
                    { 51, "حذف الواردات", "DeleteIncome" },
                    { 52, "عرض التقارير", "ShowReports" }
                });

            migrationBuilder.InsertData(
                table: "Privilege",
                columns: new[] { "Id", "Name", "SysName" },
                values: new object[,]
                {
                    { 53, "طباعة عميل", "PrintClient" },
                    { 54, "طباعة مندوب", "PrintAgent" },
                    { 55, "تسديد", "Pay" },
                    { 56, "إدارة الصناديق", "TreasuryManagment" },
                    { 57, "استلام الشحنات المسلمة", "ReceiptOfTheStatusOfTheDeliveredShipment" },
                    { 58, "استلام الشحنات المرتجعة", "ReceiptOfTheStatusOfTheReturnedShipment" },
                    { 59, "تسديد في الطريق", "PayInWay" },
                    { 60, "تسديد مرتجع كلي", "PayCompletelyReturned" },
                    { 61, "تسديد مرتجع جزئي", "PayPartialReturned" },
                    { 62, "تسديد تم الستليم", "PayDelivered" },
                    { 33, "إضافة انواع الواردات", "AddIncomeType" },
                    { 63, "تسديد المرفوض", "PayUnacceptable" },
                    { 32, "عرض انواع الواردات", "ShowIncomeType" },
                    { 30, "إضافة قسم", "AddDepartment" },
                    { 1, "عرض المجموعات", "ShowGroup" },
                    { 2, "إضافة مجموعات", "AddGroup" },
                    { 3, "التعديل على المجموعات", "EditGroup" },
                    { 4, "حذف مجموعات", "DeleteGroup" },
                    { 5, "عرض الموظفين", "ShowUser" },
                    { 6, "إضافة موظفين", "AddUser" },
                    { 7, "تعديل الموظفين", "EditUser" },
                    { 8, "حذف موظفين", "DeleteUser" },
                    { 9, "عرض انواع الطلبات", "ShowOrderType" },
                    { 10, "إضافة انواع الطلبات", "AddOrderType" },
                    { 11, "تعديل انواع الطلبات", "EditOrderType" },
                    { 12, "حذف انواع الطلبات", "DeleteOrderType" },
                    { 13, "عرض المدن", "ShowCountry" },
                    { 14, "إضافة المدن", "AddCountry" },
                    { 15, "تعديل المدن", "EditCountry" },
                    { 16, "حذف المدن", "DeleteCountry" },
                    { 17, "إضافة منطقة", "AddRegion" },
                    { 18, "تعديل منطقة", "EditRegion" },
                    { 19, "عرض المناطق", "ShowRegion" },
                    { 20, "حذف منقطة", "DeleteRegion" },
                    { 21, "إضافة عملاء", "AddClient" },
                    { 22, "عرض العملاء", "ShowClient" },
                    { 23, "تعديل العملاء", "UpdateClient" },
                    { 24, "حذف العملاء", "DeleteClient" },
                    { 25, "عرض العملات", "ShowCurrency" },
                    { 26, "إضافة عملات", "AddCurrency" },
                    { 27, "تعديل العملات", "UpdateCurrency" },
                    { 28, "حذف العملات", "DeleteCurrency" }
                });

            migrationBuilder.InsertData(
                table: "Privilege",
                columns: new[] { "Id", "Name", "SysName" },
                values: new object[] { 29, "عرض الأقسام", "ShowDepartment" });

            migrationBuilder.InsertData(
                table: "Privilege",
                columns: new[] { "Id", "Name", "SysName" },
                values: new object[] { 31, "تعديل قسم", "UpdateDepartment" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Adress", "CanWorkAsAgent", "Experince", "HireDate", "IsActive", "Name", "Note", "Password", "Salary", "UserName" },
                values: new object[] { 1, null, false, null, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "admin", null, "21232f297a57a5a743894a0e4a801fc3", null, "admin" });

            migrationBuilder.InsertData(
                table: "GroupPrivilege",
                columns: new[] { "GroupId", "PrivilegId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 36 },
                    { 1, 37 },
                    { 1, 38 },
                    { 1, 39 },
                    { 1, 40 },
                    { 1, 41 },
                    { 1, 42 },
                    { 1, 43 },
                    { 1, 44 },
                    { 1, 45 },
                    { 1, 46 },
                    { 1, 47 },
                    { 1, 48 },
                    { 1, 35 },
                    { 1, 49 },
                    { 1, 51 },
                    { 1, 52 },
                    { 1, 53 },
                    { 1, 54 },
                    { 1, 55 },
                    { 1, 56 },
                    { 1, 57 },
                    { 1, 58 },
                    { 1, 59 },
                    { 1, 60 },
                    { 1, 61 },
                    { 1, 62 },
                    { 1, 63 },
                    { 1, 50 },
                    { 1, 34 },
                    { 1, 33 },
                    { 1, 16 },
                    { 1, 4 },
                    { 1, 5 },
                    { 1, 6 },
                    { 1, 7 },
                    { 1, 8 },
                    { 1, 9 },
                    { 1, 10 },
                    { 1, 11 },
                    { 1, 12 }
                });

            migrationBuilder.InsertData(
                table: "GroupPrivilege",
                columns: new[] { "GroupId", "PrivilegId" },
                values: new object[,]
                {
                    { 1, 13 },
                    { 1, 14 },
                    { 1, 15 },
                    { 1, 32 },
                    { 1, 3 },
                    { 1, 17 },
                    { 1, 19 },
                    { 1, 20 },
                    { 1, 21 },
                    { 1, 22 },
                    { 1, 23 },
                    { 1, 24 },
                    { 1, 25 },
                    { 1, 26 },
                    { 1, 27 },
                    { 1, 28 },
                    { 1, 29 },
                    { 1, 30 },
                    { 1, 31 },
                    { 1, 18 },
                    { 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "Treasury",
                columns: new[] { "Id", "CreateOnUtc", "IsActive", "Total" },
                values: new object[] { 1, new DateTime(2019, 12, 31, 22, 0, 0, 0, DateTimeKind.Utc), true, 0m });

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
                table: "Country",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 5 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 6 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 7 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 8 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 9 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 10 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 11 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 12 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 13 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 14 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 15 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 16 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 17 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 18 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 19 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 20 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 21 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 22 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 23 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 24 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 25 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 26 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 27 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 28 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 29 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 30 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 31 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 32 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 33 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 34 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 35 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 36 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 37 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 38 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 39 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 40 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 41 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 42 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 43 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 44 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 45 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 46 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 47 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 48 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 49 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 50 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 51 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 52 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 53 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 54 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 55 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 56 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 57 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 58 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 59 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 60 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 61 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 62 });

            migrationBuilder.DeleteData(
                table: "GroupPrivilege",
                keyColumns: new[] { "GroupId", "PrivilegId" },
                keyValues: new object[] { 1, 63 });

            migrationBuilder.DeleteData(
                table: "MoenyPlaced",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MoenyPlaced",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MoenyPlaced",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MoenyPlaced",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "OrderPlaced",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OrderPlaced",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OrderPlaced",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OrderPlaced",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "OrderPlaced",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "OrderPlaced",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "OrderPlaced",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "OrderPlaced",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "OrderState",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OrderState",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OrderState",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Treasury",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UserGroup",
                keyColumns: new[] { "GroupId", "UserId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "Group",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Privilege",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
