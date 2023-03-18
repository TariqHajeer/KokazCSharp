using KokazGoodsTransfer.Models.Static;

namespace KokazGoodsTransfer.Dtos.OrdersDtos
{
    public class OrderDontFinishedFilter
    {
        public int ClientId { get; set; }
        public OrderPlace[] OrderPlacedId { get; set; }
        public bool IsClientDeleviredMoney { get; set; }
        public bool ClientDoNotDeleviredMoney { get; set; }
    }
}
