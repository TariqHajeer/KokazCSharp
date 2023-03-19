namespace Quqaz.Web.Dtos.OrdersDtos
{
    public class OrderReSend
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public int? RegionId { get; set; }
        public int AgnetId { get; set; }
        public decimal DeliveryCost { get; set; }

    }
}
