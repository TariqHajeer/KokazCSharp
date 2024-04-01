using System.Security.Cryptography;

namespace Quqaz.Web.Dtos.OrdersDtos
{
    public class OrderFromExcelDto
    {
        public string Code { get; set; }
        public string RecipientName { get; set; }
        public string Country { get; set; }
        public decimal Cost { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string OrderType { get; set; }
        public int? OrderTypeCount { get; set; }
        public string Note { get; set; }
    }
}
