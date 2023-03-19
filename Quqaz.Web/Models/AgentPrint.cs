using Quqaz.Web.Models.Infrastrcuter;
using System;
using System.Collections.Generic;

#nullable disable

namespace Quqaz.Web.Models
{
    public partial class AgentPrint : IHaveBranch
    {
        public AgentPrint()
        {
            AgentOrderPrints = new HashSet<AgentOrderPrint>();
            AgentPrintDetails = new HashSet<AgentPrintDetail>();
        }

        public int Id { get; set; }
        public string PrinterName { get; set; }
        public DateTime Date { get; set; }
        public string DestinationName { get; set; }
        public string DestinationPhone { get; set; }

        public virtual ICollection<AgentOrderPrint> AgentOrderPrints { get; set; }
        public virtual ICollection<AgentPrintDetail> AgentPrintDetails { get; set; }
        public int BranchId { get; set; }
        public Branch Branch { get; set; }
    }
}
