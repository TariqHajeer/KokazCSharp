using System.ComponentModel.DataAnnotations.Schema;

namespace Quqaz.Web.Models
{
    public class FCMTokens
    {
        public int Id { set; get; }
        public string Token { get; set; }

        public string MacAddress { get; set; }
        public int? ClientId { get; set; }
        [ForeignKey(nameof(ClientId))]
        public Client Client { get; set; }
    }
}