using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class ClientOrderType
    {
        public int ClientId { get; set; }
        public int OrderTypeId { get; set; }

        public virtual Client Client { get; set; }
        public virtual OrderType OrderType { get; set; }
    }
}
