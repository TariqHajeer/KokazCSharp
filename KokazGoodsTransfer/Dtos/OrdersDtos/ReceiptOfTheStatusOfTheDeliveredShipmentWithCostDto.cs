using KokazGoodsTransfer.Models.Static;

namespace KokazGoodsTransfer.Dtos.OrdersDtos
{
    public class ReceiptOfTheStatusOfTheDeliveredShipmentDto
    {
        public int Id { get; set; }
        public MoneyPalcedEnum MoenyPlacedId { get; set; }
        public OrderplacedEnum OrderplacedId { get; set; }
        public string Note { get; set; }
        public decimal DeliveryCost { get; set; }
        public decimal AgentCost { get; set; }
    }
    public class ReceiptOfTheStatusOfTheDeliveredShipmentWithCostDto : ReceiptOfTheStatusOfTheDeliveredShipmentDto
    {
        public decimal Cost { get; set; }
    }
    
}
