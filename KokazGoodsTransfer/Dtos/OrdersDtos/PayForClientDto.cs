using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Dtos.Regions;
using KokazGoodsTransfer.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.OrdersDtos
{
    public class PayForClientDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public UserDto Agent { get; set; }
        public decimal DeliveryCost { get; set; }
        public decimal Cost { get; set; }
        public decimal? OldCost { get; set; }
        public decimal PayForClient { get; set; }
        public CountryDto Country { get; set; }
        public RegionDto Region { get; set; }
        public NameAndIdDto MonePlaced { get; set; }
        public NameAndIdDto Orderplaced { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DiliveryDate { get; set; }
        public int? AgentPrintNumber { get; set; }
        public int? ClientPrintNumber { get; set; }
        public string ClientNote { get; set; }
        public string Note { get; set; }
        public bool IsClientDiliverdMoney { get; set; }
    }
}
