using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.OrdersDtos
{
    public class CreateMultipleOrder
    {
        public string Code { get; set; }
        public int ClientId { get; set; }
        public int CountryId { get; set; }
        public int? AgentId { get; set; }
        public string RecipientName { get; set; }
        public decimal Cost { get; set; }
        public string RecipientPhones { set; get; }
        public decimal DeliveryCost { set; get; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
        public int? RegionId { get; set; }
    }
}
