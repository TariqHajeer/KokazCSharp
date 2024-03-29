using Quqaz.Web.Models.Static;

namespace Quqaz.Web.Dtos.OrdersDtos
{
    public class ShipmentTrackingResponse
    {
        public string Source { get; set; }
        public string Mediator { get; set; }
        public string Destination { get; set; }
        public OrderPlace OrderPlace { get; set; }
        public string CurrentPlace { get; set; }

    }
}
