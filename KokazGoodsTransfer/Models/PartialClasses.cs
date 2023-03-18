using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Models
{
    public partial class OrderLog
    {
        public static implicit operator OrderLog(Order o)
        {
            return new OrderLog()
            {
                OrderId = o.Id,
                Code = o.Code,
                ClientId = o.ClientId,
                IsSync = o.IsSync,
                CountryId = o.CountryId,
                Cost = o.Cost,
                ClientNote = o.ClientNote,
                AgentId = o.AgentId,
                AgentCost = o.AgentCost,
                Address = o.Address,
                DeliveryCost = o.DeliveryCost,
                IsClientDiliverdMoney = o.IsClientDiliverdMoney,
                Note = o.Note,
                OldCost = o.OldCost,
                RegionId = o.RegionId,
                DiliveryDate = o.DiliveryDate,
                RecipientName = o.RecipientName,
                RecipientPhones = o.RecipientPhones,
                MoenyPlacedId = (int)o.MoneyPlaced,
                OrderplacedId = (int)o.Orderplaced,
                OrderStateId = o.OrderStateId,
                Seen = o.Seen,
                UpdatedBy = o.UpdatedBy ?? o.CreatedBy,
                UpdatedDate = o.UpdatedDate,
                SystemNote = o.SystemNote,
            };
        }
    }
}
