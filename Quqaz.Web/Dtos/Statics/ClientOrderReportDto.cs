namespace Quqaz.Web.Dtos.Statics
{
    public class ClientOrderReportDto
    {
        public decimal DeliveredOrderRatio { get; set; }
        public decimal ReturnOrderRatio { get; set; }
        public int HighestRequestedCountryMapId { get; set; }
        public int HighestDeliveredCountryMapId { get; set; }
        public int HighestReturnedCountryMapId { get; set; }
    }
}