using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class Printed
    {
        public Printed()
        {
            ClientPrints = new HashSet<ClientPrint>();
            Discounts = new HashSet<Discount>();
            OrderPrints = new HashSet<OrderPrint>();
            Receipts = new HashSet<Receipt>();
        }

        public int Id { get; set; }
        public int PrintNmber { get; set; }
        public string PrinterName { get; set; }
        public DateTime? Date { get; set; }
        public string DestinationName { get; set; }
        public string DestinationPhone { get; set; }
        public string Type { get; set; }

        public virtual ICollection<ClientPrint> ClientPrints { get; set; }
        public virtual ICollection<Discount> Discounts { get; set; }
        public virtual ICollection<OrderPrint> OrderPrints { get; set; }
        public virtual ICollection<Receipt> Receipts { get; set; }
    }
}
