using KokazGoodsTransfer.Models.Infrastrcuter;
using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class OrderPlaced : IIndex
    {
        public OrderPlaced()
        {
            Notfications = new HashSet<Notfication>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Notfication> Notfications { get; set; }
    }
}
