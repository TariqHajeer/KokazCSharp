﻿using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class Printed
    {
        public Printed()
        {
            AgnetPrints = new HashSet<AgnetPrint>();
            ClientPrints = new HashSet<ClientPrint>();
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

        public virtual ICollection<AgnetPrint> AgnetPrints { get; set; }
        public virtual ICollection<ClientPrint> ClientPrints { get; set; }
        public virtual ICollection<OrderPrint> OrderPrints { get; set; }
        public virtual ICollection<Receipt> Receipts { get; set; }
    }
}
