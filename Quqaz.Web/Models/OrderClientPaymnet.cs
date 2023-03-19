using System;
using System.Collections.Generic;

#nullable disable

namespace Quqaz.Web.Models
{
    public partial class OrderClientPaymnet
    {
        public int OrderId { get; set; }
        public int ClientPaymentId { get; set; }

        public virtual ClientPayment ClientPayment { get; set; }
        public virtual Order Order { get; set; }
    }
}
