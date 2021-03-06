﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.Statics
{
    public class MainStaticsDto
    {
        public int TotalClient { get; set; }
        public int TotalAgent { get; set; }
        public int TotlaOrder { get; set; }
        public int TotalOrderInSotre { get; set; }
        public int TotalOrderOutStore { get; set; }
        public int TotalOrderDiliverd { get; set; }
    }
}
