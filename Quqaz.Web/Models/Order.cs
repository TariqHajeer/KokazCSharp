using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.OrdersDtos;
using Quqaz.Web.Models.Infrastrcuter;
using Quqaz.Web.Models.Static;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Quqaz.Web.Models
{
    public partial class Order : IHaveBranch
    {
        public Order()
        {
            AgentOrderPrints = new HashSet<AgentOrderPrint>();
            OrderClientPaymnets = new HashSet<OrderClientPaymnet>();
            OrderItems = new HashSet<OrderItem>();
            OrderLogs = new HashSet<OrderLog>();
            ReceiptOfTheOrderStatusDetalis = new HashSet<ReceiptOfTheOrderStatusDetali>();
        }

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
        public string CreatedBy { get; set; }
        public MoneyPalce MoneyPlace { get; set; }
        public OrderPlace OrderPlace { get; set; }
        public bool InWayToBranch { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DiliveryDate { get; set; }
        public string Note { get; set; }
        public int? AgentId { get; set; }
        public bool? Seen { get; set; }
        public bool IsClientDiliverdMoney { get; set; }
        public bool IsSync { get; set; }
        public OrderState OrderState { get; set; }
        public bool IsDollar { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string SystemNote { get; set; }
        public decimal? OldDeliveryCost { get; set; }
        public bool? IsSend { get; set; }
        public decimal? ClientPaied { get; set; }
        public int PrintedTimes { get; set; }
        public int AgentRequestStatus { get; set; }
        public decimal? NewCost { get; set; }
        public OrderPlace? NewOrderPlace { get; set; }
        public int BranchId { get; set; }
        public int? TargetBranchId { get; set; }
        public bool IsReturnedByBranch { get; set; }
        public bool? NegativeAlert { get; set; }
        public virtual User Agent { get; set; }
        public virtual Client Client { get; set; }
        public virtual Country Country { get; set; }
        public virtual Region Region { get; set; }
        public virtual ICollection<AgentOrderPrint> AgentOrderPrints { get; set; }
        public virtual ICollection<OrderClientPaymnet> OrderClientPaymnets { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<OrderLog> OrderLogs { get; set; }
        public virtual ICollection<ReceiptOfTheOrderStatusDetali> ReceiptOfTheOrderStatusDetalis { get; set; }
        public Branch Branch { get; set; }
        [ForeignKey(nameof(TargetBranchId))]
        public virtual Branch TargetBranch { get; set; }
        public int CurrentBranchId { get; set; }
        [ForeignKey(nameof(CurrentBranchId))]

        public virtual Branch CurrentBranch { get; set; }
        public int? NextBranchId { get; set; }
        [ForeignKey(nameof(NextBranchId))]
        public Branch NextBranch { get; set; }
        [NotMapped]
        public string[] RecipientPhonesAsArray
        {
            get
            {
                return this.RecipientPhones.Split(",");
            }
        }
        public NameAndIdDto GetOrderPlaced()
        {
            return this.OrderPlace;
        }
        public bool CanChangeTheAgent()
        {
            return (OrderPlace == OrderPlace.Store && MoneyPlace == MoneyPalce.OutSideCompany);
        }
        public NameAndIdDto GetMoneyPlaced()
        {
            return this.MoneyPlace;
        }
        public NameAndIdDto GetNewOrderPlaced()
        {
            if (this.NewOrderPlace.HasValue)
                return this.NewOrderPlace.Value;
            return null;
        }
        public bool IsOrderReturn()
        {
            if (OrderPlace == OrderPlace.CompletelyReturned)
                return true;
            if (OrderPlace == OrderPlace.PartialReturned)
                return true;
            if (OrderPlace == OrderPlace.Unacceptable)
                return true;
            return false;
        }
        public decimal ShouldToPay()
        {
            if (OrderPlace == OrderPlace.CompletelyReturned)
                return 0;
            return Cost - DeliveryCost;
        }
        public bool IsOrderInMyStore()
        {
            if (OrderPlace == OrderPlace.Store)
                return true;
            if (MoneyPlace != MoneyPalce.InsideCompany)
                return false;
            if (OrderPlace == OrderPlace.CompletelyReturned)
                return true;
            if (OrderPlace == OrderPlace.PartialReturned)
                return true;
            if (OrderPlace == OrderPlace.Unacceptable)
                return true;
            if (OrderPlace == OrderPlace.Delayed)
                return true;
            return false;
        }
        public bool IsTheOrderInTheMainBrnach()
        {
            return this.CurrentBranchId == this.BranchId;
        }
        public bool IsTheOrderWithTheOtherBracnh()
        {
            return this.CurrentBranchId == this.NextBranchId;
        }
        
        public void Map(UpdateOrder updateOrder)
        {
            this.Code = updateOrder.Code;
            this.DeliveryCost = updateOrder.DeliveryCost;
            this.Cost = updateOrder.Cost;
            this.RegionId = updateOrder.RegionId;
            this.Address = updateOrder.Address;
            this.RecipientName = updateOrder.RecipientName;
            this.RecipientPhones = String.Join(",", updateOrder.RecipientPhones);
            this.Note = updateOrder.Note;
            this.UpdatedDate = DateTime.UtcNow;
        }
    }
}
