using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Dtos.NotifcationDtos
{
    public class AdminNotification
    {

        /// <summary>
        /// الطلبات الجديدة للعملاء
        /// </summary>
        public int NewOrdersCount { get; set; } = -1;
        /// <summary>
        /// طلبات جديدة لم ترسل 
        /// </summary>
        public int NewOrdersDontSendCount { get; set; } = -1;
        /// <summary>
        /// طلبات تعديل حالة الشحنة
        /// </summary>
        public int OrderRequestEditStateCount { get; set; } = -1;
        /// <summary>
        /// طلبات تعديل العملاء
        /// </summary>
        public int NewEditRquests { get; set; } = -1;
        /// <summary>
        /// طلبات دفعة جديدة 
        /// </summary>
        public int NewPaymentRequetsCount { get; set; } = -1;
    }
}
