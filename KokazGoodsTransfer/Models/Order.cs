using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderOrderTypes = new HashSet<OrderOrderType>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public int ClientId { get; set; }
        public int CountryId { get; set; }
        public decimal DeliveryCost { get; set; }
        public decimal Cost { get; set; }
        public string RecipientName { get; set; }
        public int? RegionId { get; set; }
        public string Address { get; set; }
        public string ClientNote { get; set; }
        public string CreatedBy { get; set; }
        public int MoenyPlacedId { get; set; }
        public int OrderplacedId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DiliveryDate { get; set; }
        public string Note { get; set; }
        public int AgentId { get; set; }

        public virtual Client Client { get; set; }
        public virtual Country Country { get; set; }
        public virtual MoenyPlaced MoenyPlaced { get; set; }
        public virtual OrderPlaced Orderplaced { get; set; }
        public virtual Region Region { get; set; }
        public virtual ICollection<OrderOrderType> OrderOrderTypes { get; set; }
    }
}
