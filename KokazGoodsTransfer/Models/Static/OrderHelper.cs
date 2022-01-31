using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Models.Static
{
    public static class OrderHelper
    {
        public static decimal ShouldToPay(this Order order)
        {
            if (order.OrderplacedId == (int)OrderplacedEnum.CompletelyReturned)
                return 0;
            return order.Cost - order.DeliveryCost;
        }
        public static decimal CalcClientBalanc(this Order order)
        {
            if (!order.IsClientDiliverdMoney)
            {
                if (order.OrderplacedId == (int)OrderplacedEnum.CompletelyReturned)
                    return 0;
                if (order.MoenyPlacedId == (int)MoneyPalcedEnum.WithAgent)
                    return 0;
                if (order.MoenyPlacedId == (int)MoneyPalcedEnum.OutSideCompany)
                    return 0;
                if (order.MoenyPlacedId == (int)MoneyPalcedEnum.InsideCompany)
                    return order.Cost - order.DeliveryCost;
            }
            if (order.MoenyPlacedId == (int)MoneyPalcedEnum.Delivered)
                return 0;
            return (decimal)order.ClientPaied * -1;
        }

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
                    return (order.ClientPaied ?? 0) * -1;
                if (order.OrderplacedId == (int)OrderplacedEnum.Unacceptable)
                    return (order.ClientPaied ?? 0) * -1;
                //if (order.OrderplacedId == (int)OrderplacedEnum.PartialReturned)
                //    return (order.Cost - order.DeliveryCost) - (order.ClientPaied);
                var shouldToPay = order.Cost - order.DeliveryCost;
                //var lastPaied = order.ClientPaied ?? 0;
                return shouldToPay;

            }
        }
    }
}
