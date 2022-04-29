using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class Receipt
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public string CreatedBy { get; set; }
        public string About { get; set; }
        public string Manager { get; set; }
        public bool IsPay { get; set; }
        public int? ClientPaymentId { get; set; }

        public virtual Client Client { get; set; }
        public virtual ClientPayment ClientPayment { get; set; }
    }
}
