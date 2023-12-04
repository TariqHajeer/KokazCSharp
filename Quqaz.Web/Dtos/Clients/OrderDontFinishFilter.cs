using Quqaz.Web.Models.Static;

namespace Quqaz.Web.Dtos.Clients
{
    public class OrderDontFinishFilter
    {
        public OrderPlace[] OrderPlacedId { get; set; }
        public bool IsClientDeleviredMoney { get; set; }
        public bool ClientDoNotDeleviredMoney { get; set; }
    }
}
