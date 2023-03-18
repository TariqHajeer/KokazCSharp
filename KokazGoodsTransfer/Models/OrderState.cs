using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class OrderState
    {
        public OrderState()
        {
            ReceiptOfTheOrderStatusDetalis = new HashSet<ReceiptOfTheOrderStatusDetali>();
        }

        public int Id { get; set; }
        public string State { get; set; }
        public virtual ICollection<ReceiptOfTheOrderStatusDetali> ReceiptOfTheOrderStatusDetalis { get; set; }
    }
}
