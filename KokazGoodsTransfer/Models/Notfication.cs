#nullable disable

using KokazGoodsTransfer.Models.Static;

namespace KokazGoodsTransfer.Models
{
    public partial class Notfication
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Note { get; set; }
        public int? OrderCount { get; set; }
        public OrderPlace? OrderPlace { get; set; }
        public MoneyPalce? MoneyPlace { get; set; }
        public bool? IsSeen { get; set; }

        public virtual Client Client { get; set; }
    }
}
