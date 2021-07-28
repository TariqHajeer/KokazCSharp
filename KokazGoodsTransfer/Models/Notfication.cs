﻿using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class Notfication
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Note { get; set; }
        public int? OrderCount { get; set; }
        public int? OrderPlacedId { get; set; }
        public int? MoneyPlacedId { get; set; }
        public bool? IsSeen { get; set; }

        public virtual Client Client { get; set; }
        public virtual MoenyPlaced MoneyPlaced { get; set; }
        public virtual OrderPlaced OrderPlaced { get; set; }
    }
}
