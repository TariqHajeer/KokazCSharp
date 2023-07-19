using System.ComponentModel.DataAnnotations;

namespace Quqaz.Web.Dtos.Common
{
    public class DriverDto
    {
        public int? DriverId { get; set; }
        [MaxLength(50)]
        public string DriverName { get; set; }
    }
}
