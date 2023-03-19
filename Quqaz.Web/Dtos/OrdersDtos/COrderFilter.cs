using Quqaz.Web.Models.Static;

namespace Quqaz.Web.Dtos.OrdersDtos
{
    public class COrderFilter
    {
        /// <summary>
        /// الكود
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// رقم المستلم
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// المدينة
        /// </summary>
        public int? CountryId { get; set; }
        /// <summary>
        /// المنقطة
        /// </summary>
        public int? RegionId { get; set; }
        /// <summary>
        /// اسم المستلم
        /// </summary>
        public string RecipientName { get; set; }
        /// <summary>
        /// موقع المبلغ
        /// </summary>
        public MoneyPalce? MonePlacedId { get; set; }
        /// <summary>
        /// موقع الشحنة
        /// </summary>
        public OrderPlace? OrderplacedId { get; set; }
        /// <summary>
        /// إذا تم التسديد
        /// </summary>
        public bool? IsClientDiliverdMoney { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ClientPrintNumber { get; set; }
    }
}
