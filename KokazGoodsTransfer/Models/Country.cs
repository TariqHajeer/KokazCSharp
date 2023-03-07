using System.Collections.Generic;
using KokazGoodsTransfer.Models.Infrastrcuter;
#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class Country : IIndex
    {
        public Country()
        {
            AgentCountries = new HashSet<AgentCountry>();
            Clients = new HashSet<Client>();
            DisAcceptOrders = new HashSet<DisAcceptOrder>();
            InverseMediator = new HashSet<Country>();
            OrderCountries = new HashSet<Order>();
            OrderLogs = new HashSet<OrderLog>();
            Regions = new HashSet<Region>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal DeliveryCost { get; set; }
        public int? MediatorId { get; set; }
        public int Points { get; set; }
        public Branch Branch { get; set; }

        public virtual Country Mediator { get; set; }
        public virtual ICollection<AgentCountry> AgentCountries { get; set; }
        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<DisAcceptOrder> DisAcceptOrders { get; set; }
        public virtual ICollection<Country> InverseMediator { get; set; }
        public virtual ICollection<Order> OrderCountries { get; set; }
        public virtual ICollection<OrderLog> OrderLogs { get; set; }
        public virtual ICollection<Region> Regions { get; set; }
        public virtual ICollection<MediatorCountry> FromCountries { get; set; }
        public virtual ICollection<MediatorCountry> MediatorCountries { get; set; }
        public virtual ICollection<MediatorCountry> ToCountries { get; set; }
    }
}
