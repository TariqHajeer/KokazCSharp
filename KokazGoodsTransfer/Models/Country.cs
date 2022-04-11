using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class Country
    {
        public Country()
        {
            AgentCountrs = new HashSet<AgentCountr>();
            Clients = new HashSet<Client>();
            DisAcceptOrders = new HashSet<DisAcceptOrder>();
            InverseMediator = new HashSet<Country>();
            OrderCountries = new HashSet<Order>();
            OrderCurrentCountryNavigations = new HashSet<Order>();
            OrderLogs = new HashSet<OrderLog>();
            Regions = new HashSet<Region>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal DeliveryCost { get; set; }
        public int? MediatorId { get; set; }
        public bool IsMain { get; set; }
        public int Points { get; set; }

        public virtual Country Mediator { get; set; }
        public virtual ICollection<AgentCountr> AgentCountrs { get; set; }
        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<DisAcceptOrder> DisAcceptOrders { get; set; }
        public virtual ICollection<Country> InverseMediator { get; set; }
        public virtual ICollection<Order> OrderCountries { get; set; }
        public virtual ICollection<Order> OrderCurrentCountryNavigations { get; set; }
        public virtual ICollection<OrderLog> OrderLogs { get; set; }
        public virtual ICollection<Region> Regions { get; set; }
    }
}
