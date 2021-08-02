using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class PaymentWay
    {
        public PaymentWay()
        {
            PaymentRequests = new HashSet<PaymentRequest>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PaymentRequest> PaymentRequests { get; set; }
    }
}
