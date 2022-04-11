using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class OrderFromExcel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string RecipientName { get; set; }
        public string Country { get; set; }
        public decimal Cost { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
    }
}
