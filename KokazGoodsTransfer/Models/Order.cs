﻿using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Models.Infrastrcuter;
using KokazGoodsTransfer.Models.Static;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace KokazGoodsTransfer.Models
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
        public int MoenyPlacedId { get; set; }
        public int OrderplacedId { get; set; }
        public bool InWayToBranch { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DiliveryDate { get; set; }
        public string Note { get; set; }
        public int? AgentId { get; set; }
        public bool? Seen { get; set; }
        public bool IsClientDiliverdMoney { get; set; }
        public bool IsSync { get; set; }
        public int OrderStateId { get; set; }
        public bool IsDollar { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string SystemNote { get; set; }
        public decimal? OldDeliveryCost { get; set; }
        public bool? IsSend { get; set; }
        public decimal? ClientPaied { get; set; }
        public int? CurrentCountry { get; set; }
        public int PrintedTimes { get; set; }
        public int AgentRequestStatus { get; set; }
        public decimal? NewCost { get; set; }
        public int? NewOrderPlacedId { get; set; }
        public int BranchId { get; set; }
        public int? TargetBranchId { get; set; }
        public bool IsReturnedByBranch { get; set; }
        public virtual User Agent { get; set; }
        public virtual Client Client { get; set; }
        public virtual Country Country { get; set; }
        public virtual Country CurrentCountryNavigation { get; set; }
        public virtual MoenyPlaced MoenyPlaced { get; set; }
        public virtual OrderState OrderState { get; set; }
        public virtual OrderPlaced Orderplaced { get; set; }
        public virtual Region Region { get; set; }
        public virtual ICollection<AgentOrderPrint> AgentOrderPrints { get; set; }
        public virtual ICollection<OrderClientPaymnet> OrderClientPaymnets { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<OrderLog> OrderLogs { get; set; }
        public virtual ICollection<ReceiptOfTheOrderStatusDetali> ReceiptOfTheOrderStatusDetalis { get; set; }
        public Branch Branch { get; set; }
        [ForeignKey(nameof(TargetBranchId))]
        public virtual Branch TargetBranch { get; set; }
        /// <summary>
        /// for agent
        /// </summary>
        [ForeignKey(nameof(NewOrderPlacedId))]
        public OrderPlaced NewOrderPlaced { get; set; }
        public int CurrentBranchId { get; set; }
        [ForeignKey(nameof(CurrentBranchId))]
        public virtual Branch CurrentBranch { get; set; }
        public int? NextBranchId { get; set; }
        [ForeignKey(nameof(NextBranchId))]
        public Branch NextBranch { get; set; }
        public NameAndIdDto GetOrderPlaced()
        {
            return (OrderplacedEnum)this.OrderplacedId;
        }
        public NameAndIdDto GetMoneyPlaced()
        {
            return (MoneyPalcedEnum)this.MoenyPlacedId;
        }
        public bool IsOrderReturn()
        {
            if (OrderplacedId == (int)OrderplacedEnum.CompletelyReturned)
                return true;
            if (OrderplacedId == (int)OrderplacedEnum.PartialReturned)
                return true;
            if (OrderplacedId == (int)OrderplacedEnum.Unacceptable)
                return true;
            return false;
        }
        public decimal ShouldToPay()
        {
            if (OrderplacedId == (int)OrderplacedEnum.CompletelyReturned)
                return 0;
            return Cost - DeliveryCost;
        }
        public bool IsOrderInMyStore()
        {
            if (OrderplacedId == (int)OrderplacedEnum.Store)
                return true;
            if (MoenyPlacedId != (int)MoneyPalcedEnum.InsideCompany)
                return false;
            if (OrderplacedId == (int)OrderplacedEnum.CompletelyReturned)
                return true;
            if (OrderplacedId == (int)OrderplacedEnum.PartialReturned)
                return true;
            if (OrderplacedId == (int)OrderplacedEnum.Unacceptable)
                return true;
            if (OrderplacedId == (int)OrderplacedEnum.Delayed)
                return true;
            return false;
        }
    }
}
