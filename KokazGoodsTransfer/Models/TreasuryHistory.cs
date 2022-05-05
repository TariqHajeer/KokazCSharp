using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class TreasuryHistory
    {
        public int Id { get; set; }
        public int TreasuryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public int? ClientPaymentId { get; set; }
        public int? CashMovmentId { get; set; }
        public int? ReceiptId { get; set; }
        public int? ReceiptOfTheOrderStatusId { get; set; }

        public virtual CashMovment CashMovment { get; set; }
        public virtual ClientPayment ClientPayment { get; set; }
        public virtual Receipt Receipt { get; set; }
        public virtual ReceiptOfTheOrderStatus ReceiptOfTheOrderStatus { get; set; }
        public virtual Treasury Treasury { get; set; }
    }
}
