using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.OrdersDtos
{
    public class CreateOrdersFromEmployee
    {
        public string Code { get; set; }
        public int ClientId { get; set; }
        public int CountryId { get; set; }
        public int? RegionId { get; set; }
        public string RegionName { get; set; }
        public string Address { get; set; }
        public int AgentId { get; set; }
        public int OrderplacedId { get; set; }
        public int MoenyPlacedId { get; set; }
        public decimal DeliveryCost { get; set; }
        public decimal Cost { get; set; }
        public string RecipientName { get; set; }
        public string ClientNote { get; set; }
        public string CreatedBy { get; set; }
        public decimal Amount { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DiliveryDate { get; set; }
        public string Note { get; set; }
        public List<CreateOrderItemDto> OrderTypeDtos { get; set; }
    }
    public class CreateOrderItemDto
    {
        public int? OrderTypeId { get; set; }
        public string OrderTypeName { get; set; }

        public int Count { get; set; }
    }
}
