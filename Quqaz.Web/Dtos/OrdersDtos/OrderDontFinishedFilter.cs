using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Models.Static;

namespace Quqaz.Web.Dtos.OrdersDtos
{
    public class OrderDontFinishedFilter
    {
        public int ClientId { get; set; }

        public OrderPlace[] OrderPlacedId { get; set; }
        public bool IsClientDeleviredMoney { get; set; }
        public bool ClientDoNotDeleviredMoney { get; set; }
        public TableSelection  TableSelection { get; set; }
    }

}
