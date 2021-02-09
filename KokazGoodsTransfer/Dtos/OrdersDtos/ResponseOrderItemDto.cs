using KokazGoodsTransfer.Dtos.OrdersTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.OrdersDtos
{
    public class ResponseOrderItemDto
    {
        public int Count { get; set; }
        public OrderTypeDto OrderTpye { get; set; }
    }
}
