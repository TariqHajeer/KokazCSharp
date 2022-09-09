using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Models.Static
{
    public static class OrderHelper
    {
        public static decimal GetFromAgent(this Order order)
        {
            return 0;
        }
        public static decimal ShouldToPay(this Order order)
        {
            if (order.OrderplacedId == (int)OrderplacedEnum.CompletelyReturned)
                return 0;
            return order.Cost - order.DeliveryCost;
        }
    }
}
