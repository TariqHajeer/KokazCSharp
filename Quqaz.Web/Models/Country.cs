﻿using System.Collections.Generic;
using Quqaz.Web.Models.Infrastrcuter;
#nullable disable

namespace Quqaz.Web.Models
{
    public partial class Country : IIndex
    {
        public Country()
        {
            AgentCountries = new HashSet<AgentCountry>();
            Clients = new HashSet<Client>();
            DisAcceptOrders = new HashSet<DisAcceptOrder>();
            OrderCountries = new HashSet<Order>();
            OrderLogs = new HashSet<OrderLog>();
            Regions = new HashSet<Region>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public Branch Branch { get; set; }
        public virtual ICollection<AgentCountry> AgentCountries { get; set; }
        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<DisAcceptOrder> DisAcceptOrders { get; set; }
        public virtual ICollection<Order> OrderCountries { get; set; }
        public virtual ICollection<OrderLog> OrderLogs { get; set; }
        public virtual ICollection<Region> Regions { get; set; }
        public virtual ICollection<MediatorCountry> FromCountries { get; set; }
        public virtual ICollection<MediatorCountry> MediatorCountries { get; set; }
        public virtual ICollection<MediatorCountry> ToCountries { get; set; }
        public virtual ICollection<BranchToCountryDeliverryCost> BranchToCountryDeliverryCosts { get; set; }
    }
}
