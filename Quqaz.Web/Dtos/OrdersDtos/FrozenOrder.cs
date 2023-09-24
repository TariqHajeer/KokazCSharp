using System;
using System.ComponentModel.DataAnnotations;

namespace Quqaz.Web.Dtos.OrdersDtos
{
    public class FrozenOrder
    {
        [Required(ErrorMessage ="Hour is Requird")]
        public int Hour { get; set; }
        public int? CountryId { get; set; }
        public int? ClientId { get; set; }
        public int? AgentId { get; set; }
        public DateTime CurrentDate { get; set; }
        public bool IsInStock { get; set; }
        public bool IsInWay { get; set; }
        public bool IsWithAgent { get; set; }
    }
}
