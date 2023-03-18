using KokazGoodsTransfer.Models.Static;
using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class OrderLog
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int ClientId { get; set; }
        public int CountryId { get; set; }
        public decimal DeliveryCost { get; set; }
        public decimal Cost { get; set; }
        public decimal? OldCost { get; set; }
        public decimal AgentCost { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPhones { get; set; }
        public int? RegionId { get; set; }
        public string Address { get; set; }
        public string ClientNote { get; set; }
        public MoneyPalce MoneyPalce { get; set; }
        public OrderPlace OrderPlace { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DiliveryDate { get; set; }
        public string Note { get; set; }
        public int? AgentId { get; set; }
        public bool? Seen { get; set; }
        public bool IsClientDiliverdMoney { get; set; }
        public bool IsSync { get; set; }
        public OrderState OrderState { get; set; }
        public string UpdatedBy { get; set; }
        public int OrderId { get; set; }
        public bool? IsDollar { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string SystemNote { get; set; }

        public virtual User Agent { get; set; }
        public virtual Client Client { get; set; }
        public virtual Country Country { get; set; }
        public virtual Order Order { get; set; }
        public virtual Region Region { get; set; }
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
                MoneyPalce = o.MoneyPlace,
                OrderPlace = o.OrderPlace,
                OrderState = o.OrderState,
                Seen = o.Seen,
                UpdatedBy = o.UpdatedBy ?? o.CreatedBy,
                UpdatedDate = o.UpdatedDate,
                SystemNote = o.SystemNote,
            };
        }
    }
}
