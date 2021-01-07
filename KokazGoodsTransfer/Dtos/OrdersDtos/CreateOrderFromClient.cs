using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.OrdersDtos
{
    public class CreateOrderFromClient
    {
        public string Code { get; set; }
        public int CountryId { get; set; }
        public string RegioName { get; set; }
        public int? RegionId { get; set; }
        public string ClientNote { get; set; }
        public decimal Cost { get; set; }
        public List<CreateOrderOrderTypeDto> OrderType { get; set; }
    }
}
