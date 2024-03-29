using Quqaz.Web.Models.Infrastrcuter;

namespace Quqaz.Web.Models.Additional
{
    public class ExternalShipment:IIdEntity
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string phone { get; set; }
        public string Email { get; set; }
        public string ProductUrl { get; set; }
    }
}
