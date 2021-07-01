using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.OrdersDtos
{
    public class CreateOrderFromClient
    {
        public string Code { get; set; }
        public int CountryId { get; set; }
        public string Address { get; set; }
        public string RecipientName { get; set; }
        public string RegioName { get; set; }
        public int? RegionId { get; set; }
        public string ClientNote { get; set; }
        public decimal Cost { get; set; }
        public DateTime Date { get; set; }
        public List<OrderItemDto> OrderItem { get; set; }
        public string[] RecipientPhones { set; get; } = new string[0];
    }
}
