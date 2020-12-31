using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class Region
    {
        public Region()
        {
            Clients = new HashSet<Client>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
