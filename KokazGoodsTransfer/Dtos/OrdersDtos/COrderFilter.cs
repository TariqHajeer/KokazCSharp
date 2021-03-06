﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.OrdersDtos
{
    public class COrderFilter
    {
        public string Code { get; set; }
        public string Phone { get; set; }
        public int? CountryId { get; set; }
        public int? RegionId { get; set; }
        public string RecipientName { get; set; }
        public int? MonePlacedId { get; set; }
        public int? OrderplacedId { get; set; }
        public bool? IsClientDiliverdMoney { get; set; }
        public int? ClientPrintNumber { get; set; }
    }
}
