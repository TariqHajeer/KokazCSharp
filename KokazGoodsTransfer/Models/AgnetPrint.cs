using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class AgnetPrint
    {
        public int Id { get; set; }
        public int PrintId { get; set; }
        public string Code { get; set; }
        public decimal Total { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string ClientName { get; set; }
        public string Note { get; set; }

        public virtual Printed Print { get; set; }
    }
}
