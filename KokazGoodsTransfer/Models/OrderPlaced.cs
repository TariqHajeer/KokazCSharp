using KokazGoodsTransfer.Models.Infrastrcuter;
using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class OrderPlaced : IIndex
    {
        public OrderPlaced()
        {
            ApproveAgentEditOrderRequests = new HashSet<ApproveAgentEditOrderRequest>();
            ClientPrints = new HashSet<ClientPrint>();
            Notfications = new HashSet<Notfication>();
            OrderLogs = new HashSet<OrderLog>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ApproveAgentEditOrderRequest> ApproveAgentEditOrderRequests { get; set; }
        public virtual ICollection<ClientPrint> ClientPrints { get; set; }
        public virtual ICollection<Notfication> Notfications { get; set; }
        public virtual ICollection<OrderLog> OrderLogs { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
