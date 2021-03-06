﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.OrdersDtos
{
    public class CreateMultipleOrder
    {
        public string Code { get; set; }
        public int ClientId { get; set; }
        public int CountryId { get; set; }
        public int AgentId { get; set; }
        public int OrderplacedId { get; set; }
        public string RecipientName { get; set; }
        public decimal Amount { get; set; }
        public string RecipientPhones { set; get; }

    }
}