using KokazGoodsTransfer.Models.Infrastrcuter;
using System.Collections.Generic;


namespace KokazGoodsTransfer.Models
{
    public partial class OrderType:IIdEntity
    {
        public OrderType()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
