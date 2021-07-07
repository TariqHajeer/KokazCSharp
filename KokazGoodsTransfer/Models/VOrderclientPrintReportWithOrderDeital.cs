using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class VOrderclientPrintReportWithOrderDeital
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int ClientId { get; set; }
        public int MoenyPlacedId { get; set; }
        public int OrderplacedId { get; set; }
        public decimal Cost { get; set; }
        public decimal DeliveryCost { get; set; }
        public decimal? OldCost { get; set; }
        public decimal? OldDeliveryCost { get; set; }
        public int OrderStateId { get; set; }
        public int PrintId { get; set; }
        public int? Ocount { get; set; }
    }
}
