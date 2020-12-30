using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class OrderType
    {
        public OrderType()
        {
            OrderOrderTypes = new HashSet<OrderOrderType>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<OrderOrderType> OrderOrderTypes { get; set; }
    }
}
