namespace KokazGoodsTransfer.Dtos.OrdersDtos
{
    public class ReceiveOrdersToMyBranchDto
    {
        public int OrderId { get; set; }
        public int AgentId { get; set; }
        public int? RegionId { get; set; }
        public decimal Cost { get; set; }
        public decimal DeliveryCost { get; set; }
    }
}
