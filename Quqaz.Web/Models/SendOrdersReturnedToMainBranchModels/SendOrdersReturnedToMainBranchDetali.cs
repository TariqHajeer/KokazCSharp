namespace Quqaz.Web.Models.SendOrdersReturnedToMainBranchModels
{
    public class SendOrdersReturnedToMainBranchDetali
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public string CountryName { get; set; }
        public string ClientName { get; set; }
        public decimal DeliveryCost { get; set; }
        public string Note { get; set; }

    }
}
