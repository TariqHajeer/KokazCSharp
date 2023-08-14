using System;
using System.ComponentModel.DataAnnotations;

namespace Quqaz.Web.Dtos.OrdersDtos
{
    public class CreateOrderFromEmployee
    {

        public string Code { get; set; }
        public int ClientId { get; set; }
        public int CountryId { get; set; }
        public int? RegionId { get; set; }
        public string RegionName { get; set; }
        public string Address { get; set; }
        public int? AgentId { get; set; }
        public decimal Cost { get; set; }
        public decimal DeliveryCost { get; set; }
        public string RecipientName { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DiliveryDate { get; set; }
        public string Note { get; set; }
        [Required]
        public string[] RecipientPhones { set; get; }
        public int? BranchId { get; set; }

        public int? TransferToOtherBranchNumber { get; set; }
    }
    public class OrderItemDto
    {
        /// <summary>
        /// نوع الطلب
        /// </summary>
        public string OrderTypeName { get; set; }
        /// <summary>
        /// معرف الطلب
        /// </summary>
        public int? OrderTypeId { get; set; }
        /// <summary>
        /// العدد
        /// </summary>
        public int Count { get; set; }
    }
}
