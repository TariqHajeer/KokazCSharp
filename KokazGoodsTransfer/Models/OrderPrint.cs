using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class OrderPrint
    {
        public int OrderId { get; set; }
        public int PrintId { get; set; }

        public virtual Order Order { get; set; }
        public virtual Printed Print { get; set; }
    }
}
