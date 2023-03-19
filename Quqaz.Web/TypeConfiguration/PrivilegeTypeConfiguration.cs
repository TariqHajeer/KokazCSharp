using Quqaz.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace Quqaz.Web.TypeConfiguration
{
    public class PrivilegeTypeConfiguration : IEntityTypeConfiguration<Privilege>
    {
        public void Configure(EntityTypeBuilder<Privilege> builder)
        {
            var id = 1;
            var list = new List<Privilege>() {
                new Privilege() { Id = id++, Name = "عرض المجموعات", SysName = "ShowGroup" },
                new Privilege() { Id = id++, Name = "إضافة مجموعات", SysName = "AddGroup" },
                new Privilege() { Id = id++, Name = "التعديل على المجموعات", SysName = "EditGroup" },
                new Privilege() { Id = id++, Name = "حذف مجموعات", SysName = "DeleteGroup" },
                new Privilege() { Id = id++, Name = "عرض الموظفين", SysName = "ShowUser" },
                new Privilege() { Id = id++, Name = "إضافة موظفين", SysName = "AddUser" },
                new Privilege() { Id = id++, Name = "تعديل الموظفين", SysName = "EditUser" },
                new Privilege() { Id = id++, Name = "حذف موظفين", SysName = "DeleteUser" },
                new Privilege() { Id = id++, Name = "عرض انواع الطلبات", SysName = "ShowOrderType" },
                new Privilege() { Id = id++, Name = "إضافة انواع الطلبات", SysName = "AddOrderType" },
                new Privilege() { Id = id++, Name = "تعديل انواع الطلبات", SysName = "EditOrderType" },
                new Privilege() { Id = id++, Name = "حذف انواع الطلبات", SysName = "DeleteOrderType" },
                new Privilege() { Id = id++, Name = "عرض المدن", SysName = "ShowCountry" },
                new Privilege() { Id = id++, Name = "إضافة المدن", SysName = "AddCountry" },
                new Privilege() { Id = id++, Name = "تعديل المدن", SysName = "EditCountry" },
                new Privilege() { Id = id++, Name = "حذف المدن", SysName = "DeleteCountry" },
                new Privilege() { Id = id++, Name = "إضافة منطقة", SysName = "AddRegion" },
                new Privilege() { Id = id++, Name = "تعديل منطقة", SysName = "EditRegion" },
                new Privilege() { Id = id++, Name = "عرض المناطق", SysName = "ShowRegion" },
                new Privilege() { Id = id++, Name = "حذف منقطة", SysName = "DeleteRegion" },
                new Privilege() { Id = id++, Name = "إضافة عملاء", SysName = "AddClient" },
                new Privilege() { Id = id++, Name = "عرض العملاء", SysName = "ShowClient" },
                new Privilege() { Id = id++, Name = "تعديل العملاء", SysName = "UpdateClient" },
                new Privilege() { Id = id++, Name = "حذف العملاء", SysName = "DeleteClient" },
                new Privilege() { Id = id++, Name = "عرض العملات", SysName = "ShowCurrency" },
                new Privilege() { Id = id++, Name = "إضافة عملات", SysName = "AddCurrency" },
                new Privilege() { Id = id++, Name = "تعديل العملات", SysName = "UpdateCurrency" },
                new Privilege() { Id = id++, Name = "حذف العملات", SysName = "DeleteCurrency" },
                new Privilege() { Id = id++, Name = "عرض الأقسام", SysName = "ShowDepartment" },
                new Privilege() { Id = id++, Name = "إضافة قسم", SysName = "AddDepartment" },
                new Privilege() { Id = id++, Name = "تعديل قسم", SysName = "UpdateDepartment" },
                new Privilege() { Id = id++, Name = "عرض انواع الواردات", SysName = "ShowIncomeType" },
                new Privilege() { Id = id++, Name = "إضافة انواع الواردات", SysName = "AddIncomeType" },
                new Privilege() { Id = id++, Name = "تعديل انواع الواردات", SysName = "UpdateIncomeType" },
                new Privilege() { Id = id++, Name = "حذف انواع الواردات", SysName = "DeleteIncomeType" },
                new Privilege() { Id = id++, Name = "عرض انواع الصادرات", SysName = "ShowOutComeType" },
                new Privilege() { Id = id++, Name = "إضافة انواع الصادرات", SysName = "AddOutComeType" },
                new Privilege() { Id = id++, Name = "تعديل انواع الصادرات", SysName = "UpdateOutComeType" },
                new Privilege() { Id = id++, Name = "حذف انواع الصادرات", SysName = "DeleteOutComeType" },
                new Privilege() { Id = id++, Name = "إضافة طلبات", SysName = "AddOrder" },
                new Privilege() { Id = id++, Name = "تعديل الطلبات", SysName = "UpdateOrder" },
                new Privilege() { Id = id++, Name = "حذف الطلبات", SysName = "DeleteOrder" },
                new Privilege() { Id = id++, Name = "عرض الطلبات", SysName = "ShowOrder" },
                new Privilege() { Id = id++, Name = "اضافة صادرات", SysName = "AddOutCome" },
                new Privilege() { Id = id++, Name = "عرض الصادرات", SysName = "ShowOutCome" },
                new Privilege() { Id = id++, Name = "تعديل الصادرات", SysName = "UpdateOutCome" },
                new Privilege() { Id = id++, Name = "حذف الصادرات", SysName = "DeleteOutCome" },
                new Privilege() { Id = id++, Name = "إضافة واردات", SysName = "AddIncome" },
                new Privilege() { Id = id++, Name = "عرض الواردات", SysName = "ShowIncome" },
                new Privilege() { Id = id++, Name = "تعديل الواردات", SysName = "UpdateIncome" },
                new Privilege() { Id = id++, Name = "حذف الواردات", SysName = "DeleteIncome" },
                new Privilege() { Id = id++, Name = "عرض التقارير", SysName = "ShowReports" },
                new Privilege() { Id = id++, Name = "طباعة عميل", SysName = "PrintClient" },
                new Privilege() { Id = id++, Name = "طباعة مندوب", SysName = "PrintAgent" },
                new Privilege() { Id = id++, Name = "تسديد", SysName = "Pay" },
                new Privilege() { Id = id++, Name = "إدارة الصناديق", SysName = "TreasuryManagment" },
                new Privilege() { Id = id++, Name = "استلام الشحنات المسلمة", SysName = "ReceiptOfTheStatusOfTheDeliveredShipment" },
                new Privilege() { Id = id++, Name = "استلام الشحنات المرتجعة", SysName = "ReceiptOfTheStatusOfTheReturnedShipment" },
                new Privilege() { Id = id++, Name = "تسديد في الطريق", SysName = "PayInWay" },
                new Privilege() { Id = id++, Name = "تسديد مرتجع كلي", SysName = "PayCompletelyReturned" },
                new Privilege() { Id = id++, Name = "تسديد مرتجع جزئي", SysName = "PayPartialReturned" },
                new Privilege() { Id = id++, Name = "تسديد تم الستليم", SysName = "PayDelivered" },
                new Privilege() { Id = id++, Name = "تسديد المرفوض", SysName = "PayUnacceptable" } };
            builder.HasData(list);
        }
    }
}
