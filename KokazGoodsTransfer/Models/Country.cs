using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class Country
    {
        public Country()
        {
            Clients = new HashSet<Client>();
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
