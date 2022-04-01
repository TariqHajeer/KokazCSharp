using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class AgentOrderPrint
    {
        public int? AgentPrintId { get; set; }
        public int? OrderId { get; set; }

        public virtual AgentPrint AgentPrint { get; set; }
        public virtual Order Order { get; set; }
    }
}
