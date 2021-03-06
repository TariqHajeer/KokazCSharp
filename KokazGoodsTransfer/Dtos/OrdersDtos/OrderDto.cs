using KokazGoodsTransfer.Dtos.Clients;
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
    public class OrderDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal DeliveryCost { get; set; }
        public decimal Cost { get; set; }
        public decimal AgentCost { get; set; }
        public decimal? OldCost { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPhones { get; set; }
        public string Address { get; set; }
        public string ClientNote { get; set; }
        public string CreatedBy { get; set; }
        public bool Seen { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DiliveryDate { get; set; }
        public string Note { get; set; }
        public bool IsClientDiliverdMoney { get; set; }
        public ClientDto Client { get; set; }
        public CountryDto Country { get; set; }
        public RegionDto Region { get; set; }
        public NameAndIdDto MonePlaced { get; set; }
        public NameAndIdDto Orderplaced { get; set; }
        public UserDto Agent { get; set; }
        public List<ResponseOrderItemDto> OrderItems { get; set; }


    }
}
