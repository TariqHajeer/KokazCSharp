using Quqaz.Web.Models.Static;
using System;
using System.Collections.Generic;

#nullable disable

namespace Quqaz.Web.Models
{
    public partial class ClientPaymentDetail
    {
        public int Id { get; set; }
        public int? ClientPaymentId { get; set; }
        public string Code { get; set; }
        public decimal? LastTotal { get; set; }
        public decimal Total { get; set; }
        public decimal DeliveryCost { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string RecipientPhones { get; set; }
        public MoneyPalce? MoneyPlace { get; set; }
        public OrderPlace? OrderPlace { get; set; }
        public DateTime? Date { get; set; }
        public string Note { get; set; }
        public decimal? PayForClient { get; set; }
        public string ClientNote { get; set; }

        public virtual ClientPayment ClientPayment { get; set; }
    }
}
