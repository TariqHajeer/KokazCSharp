using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.Regions;
using Quqaz.Web.Dtos.Users;
using System;

namespace Quqaz.Web.Dtos.OrdersDtos
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
        public NameAndIdDto Country { get; set; }
        public RegionDto Region { get; set; }
        public NameAndIdDto MonePlaced { get; set; }
        public NameAndIdDto Orderplaced { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DiliveryDate { get; set; }
        public int? AgentPrintNumber { get; set; }
        public int? ClientPrintNumber { get; set; }
        public string ClientNote { get; set; }
        public string Note { get; set; }
        public string RecipientPhones { get; set; }
        public bool IsClientDiliverdMoney { get; set; }
    }
}
