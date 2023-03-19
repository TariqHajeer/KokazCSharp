using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Dtos.OrdersDtos
{
    public class UpdateOrder
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int ClientId { get; set; }
        public int CountryId { get; set; }
        public int? RegionId { get; set; }
        public string RegionName { get; set; }
        public string Address { get; set; }
        public int AgentId { get; set; }
        public int OrderplacedId { get; set; }
        public int MoenyPlacedId { get; set; }
        public decimal Cost { get; set; }
        public decimal DeliveryCost { get; set; }
        public string RecipientName { get; set; }
        public DateTime? ActionDate { get; set; }
        public DateTime? DiliveryDate { get; set; }
        public string Note { get; set; }
        [Required]
        public string[] RecipientPhones { set; get; }
    }
}
