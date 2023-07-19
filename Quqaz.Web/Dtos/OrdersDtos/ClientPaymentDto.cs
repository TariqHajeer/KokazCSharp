using System;

namespace Quqaz.Web.Dtos.OrdersDtos
{
    public class ClientPaymentDto
    {
        public string Code { get; set; }
        public string Country { get; set; }
        public DateTime Date { get; set; }
        public decimal Cost { get; set; }
        public decimal? OldCost { get; set; }
        public decimal DeliveryCost { get; set; }
        public decimal PayForClient { get => Cost - DeliveryCost; }
        public string ClientNote { get; set; }
        public string Note { get; set; }
    }
}
