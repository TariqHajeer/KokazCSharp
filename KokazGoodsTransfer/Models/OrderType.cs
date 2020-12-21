using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class OrderType
    {
        public OrderType()
        {
            ClientOrderTypes = new HashSet<ClientOrderType>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }

        public virtual ICollection<ClientOrderType> ClientOrderTypes { get; set; }
    }
}
