#nullable disable

using KokazGoodsTransfer.Models.Static;

namespace KokazGoodsTransfer.Models
{
    public partial class ReceiptOfTheOrderStatusDetali
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public int ClientId { get; set; }
        public decimal Cost { get; set; }
        public decimal AgentCost { get; set; }
        public int AgentId { get; set; }
        public OrderStateEnum OrderState { get; set; }
        public MoneyPalce MoneyPalce { get; set; }
        public int ReceiptOfTheOrderStatusId { get; set; }
        public OrderPlace OrderPlace { get; set; }
        public int? OrderId { get; set; }

        public virtual User Agent { get; set; }
        public virtual Client Client { get; set; }
        public virtual Order Order { get; set; }
        public virtual ReceiptOfTheOrderStatus ReceiptOfTheOrderStatus { get; set; }
    }
}
