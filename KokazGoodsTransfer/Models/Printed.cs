using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class Printed
    {
        public Printed()
        {
            OrderAgentPrintNumberNavigations = new HashSet<Order>();
            OrderClientPrintNumberNavigations = new HashSet<Order>();
        }

        public int Id { get; set; }
        public int PrintNmber { get; set; }
        public string PrinterName { get; set; }
        public DateTime Date { get; set; }
        public string DestinationName { get; set; }
        public string DestinationPhone { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Order> OrderAgentPrintNumberNavigations { get; set; }
        public virtual ICollection<Order> OrderClientPrintNumberNavigations { get; set; }
    }
}
