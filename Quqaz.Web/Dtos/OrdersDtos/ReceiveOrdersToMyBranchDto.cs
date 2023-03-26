namespace Quqaz.Web.Dtos.OrdersDtos
{
    public class SetOrdersToMyBranchDto
    {
        public int OrderId { get; set; }
        public int AgentId { get; set; }
        public int? RegionId { get; set; }
        public decimal Cost { get; set; }
        public decimal DeliveryCost { get; set; }
    }
    public class ReceiveOrdersToMyBranchDto:SelectedOrdersWithFitlerDto<SetOrdersToMyBranchDto>
    {
        public int? AgentId { get; set; }
        public int? RegionId { get; set; }
    }
}
