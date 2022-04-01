using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
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
        public int? MoneyPlacedId { get; set; }
        public int? OrderPlacedId { get; set; }
        public DateTime? Date { get; set; }
        public string Note { get; set; }
        public decimal? PayForClient { get; set; }

        public virtual ClientPayment ClientPayment { get; set; }
        public virtual MoenyPlaced MoneyPlaced { get; set; }
        public virtual OrderPlaced OrderPlaced { get; set; }
    }
}
