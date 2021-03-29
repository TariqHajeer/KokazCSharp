﻿using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class ClientPrint
    {
        public int Id { get; set; }
        public int PrintId { get; set; }
        public string Code { get; set; }
        public string LastTotal { get; set; }
        public decimal Total { get; set; }
        public decimal DeliveCost { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }

        public virtual Printed Print { get; set; }
    }
}