using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class Client
    {
        public Client()
        {
            ClientPhones = new HashSet<ClientPhone>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? RegionId { get; set; }
        public string Address { get; set; }
        public DateTime FirstDate { get; set; }
        public string Note { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int UserId { get; set; }

        public virtual Region Region { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<ClientPhone> ClientPhones { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
