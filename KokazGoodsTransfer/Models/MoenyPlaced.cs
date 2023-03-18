using KokazGoodsTransfer.Models.Infrastrcuter;
using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class MoenyPlaced: IIndex
    {
        public MoenyPlaced()
        {
            Notfications = new HashSet<Notfication>();
            OrderLogs = new HashSet<OrderLog>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Notfication> Notfications { get; set; }
        public virtual ICollection<OrderLog> OrderLogs { get; set; }
    }
}
