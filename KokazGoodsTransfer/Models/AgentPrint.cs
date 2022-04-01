using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class AgentPrint
    {
        public AgentPrint()
        {
            AgentPrintDetails = new HashSet<AgentPrintDetail>();
        }

        public int Id { get; set; }
        public string PrinterName { get; set; }
        public DateTime Date { get; set; }
        public string DestinationName { get; set; }
        public string DestinationPhone { get; set; }

        public virtual ICollection<AgentPrintDetail> AgentPrintDetails { get; set; }
        public virtual ICollection<AgentOrderPrint> AgentOrderPrints { get; set; }
    }
}
