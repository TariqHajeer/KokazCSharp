using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.OrdersDtos
{
    public class CreateOrderFromClient
    {
        /// <summary>
        /// الكود
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// المدينة
        /// </summary>
        public int CountryId { get; set; }
        /// <summary>
        /// العنوان
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// اسم المستلم
        /// </summary>
        public string RecipientName { get; set; }
        /// <summary>
        /// ملاحظات 
        /// </summary>
        public string ClientNote { get; set; }
        /// <summary>
        /// الكلفة مع التوصيل
        /// </summary>
        public decimal Cost { get; set; }
        /// <summary>
        /// تاريخ الإضافة
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// نوع الطلب
        /// </summary>
        public List<OrderItemDto> OrderItem { get; set; }
        /// <summary>
        /// ارقام الهواتف
        /// </summary>
        public string[] RecipientPhones { set; get; } = new string[0];
    }
}
