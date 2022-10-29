using Microsoft.EntityFrameworkCore.Migrations;

namespace KokazGoodsTransfer.Migrations
{
    public partial class PrvilegesSeeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Privilege",
                columns: new[] { "Id", "Name", "SysName" },
                values: new object[,]
                {
                    { 1, "عرض المجموعات", "ShowGroup" },
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
                    { 33, "إضافة انواع الواردات", "AddIncomeType" },
                    { 47, "حذف الصادرات", "DeleteOutCome" },
                    { 49, "عرض الواردات", "ShowIncome" },
                    { 50, "تعديل الواردات", "UpdateIncome" },
                    { 51, "حذف الواردات", "DeleteIncome" },
                    { 52, "عرض التقارير", "ShowReports" },
                    { 53, "طباعة عميل", "PrintClient" },
                    { 54, "طباعة مندوب", "PrintAgent" },
                    { 55, "تسديد", "Pay" },
                    { 56, "إدارة الصناديق", "TreasuryManagment" },
                    { 57, "استلام الشحنات المسلمة", "ReceiptOfTheStatusOfTheDeliveredShipment" },
                    { 58, "استلام الشحنات المرتجعة", "ReceiptOfTheStatusOfTheReturnedShipment" },
                    { 59, "تسديد في الطريق", "PayInWay" },
                    { 60, "تسديد مرتجع كلي", "PayCompletelyReturned" },
                    { 61, "تسديد مرتجع جزئي", "PayPartialReturned" },
                    { 48, "إضافة واردات", "AddIncome" },
                    { 62, "تسديد تم الستليم", "PayDelivered" },
                    { 32, "عرض انواع الواردات", "ShowIncomeType" },
                    { 30, "إضافة قسم", "AddDepartment" },
                    { 2, "إضافة مجموعات", "AddGroup" },
                    { 3, "التعديل على المجموعات", "EditGroup" },
                    { 4, "حذف مجموعات", "DeleteGroup" },
                    { 5, "عرض الموظفين", "ShowUser" },
                    { 6, "إضافة موظفين", "AddUser" },
                    { 7, "تعديل الموظفين", "EditUser" },
                    { 8, "حذف موظفين", "DeleteUser" },
                    { 9, "عرض انواع الطلبات", "ShowOrderType" },
                    { 10, "إضافة انواع الطلبات", "AddOrderType" }
                });

            migrationBuilder.InsertData(
                table: "Privilege",
                columns: new[] { "Id", "Name", "SysName" },
                values: new object[,]
                {
                    { 11, "تعديل انواع الطلبات", "EditOrderType" },
                    { 12, "حذف انواع الطلبات", "DeleteOrderType" },
                    { 13, "عرض المدن", "ShowCountry" },
                    { 14, "إضافة المدن", "AddCountry" },
                    { 31, "تعديل قسم", "UpdateDepartment" },
                    { 15, "تعديل المدن", "EditCountry" },
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
                    { 28, "حذف العملات", "DeleteCurrency" },
                    { 29, "عرض الأقسام", "ShowDepartment" },
                    { 16, "حذف المدن", "DeleteCountry" },
                    { 63, "تسديد المرفوض", "PayUnacceptable" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
