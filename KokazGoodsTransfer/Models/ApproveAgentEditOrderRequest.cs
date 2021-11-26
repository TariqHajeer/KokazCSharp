using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class ApproveAgentEditOrderRequest
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int OrderPlacedId { get; set; }
        public decimal NewAmount { get; set; }
        public int AgentId { get; set; }
        public bool? IsApprove { get; set; }

        public virtual User Agent { get; set; }
        public virtual Order Order { get; set; }
        public virtual OrderPlaced OrderPlaced { get; set; }
    }
}
