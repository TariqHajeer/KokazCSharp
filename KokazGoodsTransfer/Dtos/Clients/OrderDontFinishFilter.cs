﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.Clients
{
    public class OrderDontFinishFilter
    {
        public int[] OrderPlacedId { get; set; }
        public bool IsClientDeleviredMoney { get; set; }
        public bool ClientDoNotDeleviredMoney { get; set; }
    }
}
