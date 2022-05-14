using KokazGoodsTransfer.Models;
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
        public virtual bool EqualToOrder(Order order)
        {
            return order.Id == Id && order.MoenyPlacedId == (int)MoenyPlacedId && order.OrderplacedId == (int)OrderplacedId && order.DeliveryCost == DeliveryCost && order.AgentCost == AgentCost && order.Note == Note; ;
        }
    }
    public class ReceiptOfTheStatusOfTheDeliveredShipmentWithCostDto : ReceiptOfTheStatusOfTheDeliveredShipmentDto
    {
        public decimal Cost { get; set; }
        public override bool EqualToOrder(Order order)
        {
            if (base.EqualToOrder(order))
                return order.Cost == Cost;
            return false;
        }
    }

}
