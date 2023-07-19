using System;
using System.Collections.Generic;

#nullable disable

namespace Quqaz.Web.Models
{
    public partial class OrderItem
    {
        public int OrderId { get; set; }
        public int OrderTpyeId { get; set; }
        public float? Count { get; set; }

        public virtual Order Order { get; set; }
        public virtual OrderType OrderTpye { get; set; }
    }
}
