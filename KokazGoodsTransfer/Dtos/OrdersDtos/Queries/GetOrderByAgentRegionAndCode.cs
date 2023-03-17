namespace KokazGoodsTransfer.Dtos.OrdersDtos.Queries
{
    public class GetOrdersByAgentRegionAndCodeQuery
    {
        public int AgentId { get; set; }
        public int CountryId { get; set; }
        public string Code { get; set; }
    }
}
