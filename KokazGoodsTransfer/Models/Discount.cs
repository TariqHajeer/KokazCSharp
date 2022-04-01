using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class Discount
    {
        public int Id { get; set; }
        public int Points { get; set; }
        public decimal Money { get; set; }
        public int PrintedId { get; set; }
        public int? ClientPaymentId { get; set; }

        public virtual ClientPayment ClientPayment { get; set; }
        public virtual Printed Printed { get; set; }
    }
}
