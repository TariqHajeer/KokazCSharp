using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class Order
    {
        public Order()
        {
            AgentOrderPrints = new HashSet<AgentOrderPrint>();
            ApproveAgentEditOrderRequests = new HashSet<ApproveAgentEditOrderRequest>();
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

        public virtual User Agent { get; set; }
        public virtual Client Client { get; set; }
        public virtual Country Country { get; set; }
        public virtual Country CurrentCountryNavigation { get; set; }
        public virtual MoenyPlaced MoenyPlaced { get; set; }
        public virtual OrderState OrderState { get; set; }
        public virtual OrderPlaced Orderplaced { get; set; }
        public virtual Region Region { get; set; }
        public virtual ICollection<AgentOrderPrint> AgentOrderPrints { get; set; }
        public virtual ICollection<ApproveAgentEditOrderRequest> ApproveAgentEditOrderRequests { get; set; }
        public virtual ICollection<OrderClientPaymnet> OrderClientPaymnets { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<OrderLog> OrderLogs { get; set; }
        public virtual ICollection<ReceiptOfTheOrderStatusDetali> ReceiptOfTheOrderStatusDetalis { get; set; }
    }
}
