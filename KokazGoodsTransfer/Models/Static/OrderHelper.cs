using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Models.Static
{
    public static class OrderHelper
    {
        public static decimal PayForClient(this Order order)
        {
            if (!order.IsClientDiliverdMoney)
            {
                if (order.OrderplacedId == (int)OrderplacedEnum.CompletelyReturned)
                    return 0;
                return order.Cost - order.DeliveryCost;
            }
            else
            {

                if (order.OrderplacedId == (int)OrderplacedEnum.CompletelyReturned)
                    return ((decimal)order.ClientPaied) * -1;
                if (order.OrderplacedId == (int)OrderplacedEnum.Unacceptable)
                    return (decimal)order.ClientPaied * -1;
                //if (order.OrderplacedId == (int)OrderplacedEnum.PartialReturned)
                //    return (order.Cost - order.DeliveryCost) - (order.ClientPaied);
                return (order.Cost - order.DeliveryCost) - ((decimal)(order.ClientPaied == null ? 0: order.ClientPaied)) ;

            }
        }
    }
}
