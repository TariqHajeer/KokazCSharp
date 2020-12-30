using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class OrderOrderType
    {
        public int OrderId { get; set; }
        public int OrderTpyeId { get; set; }
        public float? Count { get; set; }

        public virtual Order Order { get; set; }
        public virtual OrderType OrderTpye { get; set; }
    }
}
