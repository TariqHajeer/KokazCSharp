namespace KokazGoodsTransfer.Dtos.OrdersDtos
{
    public class ReceiptOfTheOrderStatusDetaliDto
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public int ClientId { get; set; }
        public decimal Cost { get; set; }
        public decimal AgentCost { get; set; }
        public int AgentId { get; set; }
        public int MoneyPlacedId { get; set; }
        public int ReceiptOfTheOrderStatusId { get; set; }
        public int OrderPlacedId { get; set; }
        
    }
}
