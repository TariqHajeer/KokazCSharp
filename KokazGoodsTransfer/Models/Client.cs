using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class Client
    {
        public Client()
        {
            ClientOrderTypes = new HashSet<ClientOrderType>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Country { get; set; }
        public string Region { get; set; }
        public string Address { get; set; }
        public DateTime FirstDate { get; set; }
        public string Note { get; set; }
        public int UserId { get; set; }

        public virtual Country CountryNavigation { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<ClientOrderType> ClientOrderTypes { get; set; }
    }
}
