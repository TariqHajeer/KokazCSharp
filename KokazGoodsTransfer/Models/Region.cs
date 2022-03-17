using KokazGoodsTransfer.Models.Infrastrcuter;
using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class Region: IIdEntity
    {
        public Region()
        {
            DisAcceptOrders = new HashSet<DisAcceptOrder>();
            OrderLogs = new HashSet<OrderLog>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<DisAcceptOrder> DisAcceptOrders { get; set; }
        public virtual ICollection<OrderLog> OrderLogs { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
