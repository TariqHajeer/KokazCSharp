using System.Collections.Generic;

namespace Quqaz.Web.Dtos.OrdersDtos
{
    public class SetOrderAgentToMyBranchDto
    {
        public int OrderId { get; set; }
        public int AgentId { get; set; }
        public int? RegionId { get; set; }
        public decimal Cost { get; set; }
        public decimal DeliveryCost { get; set; }
    }
    public class ReceiveOrdersToMyBranchDto:SelectedOrdersWithFitlerDto
    {
        public IEnumerable<SetOrderAgentToMyBranchDto> CustomOrderAgent { get; set; }
        public int? AgentId { get; set; }
        public int? RegionId { get; set; }
    }
}
