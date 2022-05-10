using System;
using System.Collections.Generic;

#nullable disable

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
        public int OrderStateId { get; set; }
        public int MoneyPlacedId { get; set; }
        public int ReceiptOfTheOrderStatusId { get; set; }
        public int OrderPlacedId { get; set; }

        public virtual User Agent { get; set; }
        public virtual MoenyPlaced MoneyPlaced { get; set; }
        public virtual OrderPlaced OrderPlaced { get; set; }
        public virtual OrderState OrderState { get; set; }
        public virtual ReceiptOfTheOrderStatus ReceiptOfTheOrderStatus { get; set; }
    }
}
