using System.Collections.Generic;

namespace Quqaz.Web.Dtos.OrdersDtos
{
    public class EarningsDto
    {
        public IEnumerable<OrderDto> Orders { get; set; }
        public decimal TotalEarinig { get; set; }
        public int TotalRecord { get; set; }
    }
}
