﻿using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class OrderPlaced
    {
        public OrderPlaced()
        {
            ApproveAgentEditOrderRequests = new HashSet<ApproveAgentEditOrderRequest>();
            ClientPaymentDetails = new HashSet<ClientPaymentDetail>();
            Notfications = new HashSet<Notfication>();
            OrderLogs = new HashSet<OrderLog>();
            Orders = new HashSet<Order>();
            ReceiptOfTheOrderStatusDetalis = new HashSet<ReceiptOfTheOrderStatusDetali>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ApproveAgentEditOrderRequest> ApproveAgentEditOrderRequests { get; set; }
        public virtual ICollection<ClientPaymentDetail> ClientPaymentDetails { get; set; }
        public virtual ICollection<Notfication> Notfications { get; set; }
        public virtual ICollection<OrderLog> OrderLogs { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<ReceiptOfTheOrderStatusDetali> ReceiptOfTheOrderStatusDetalis { get; set; }
    }
}
