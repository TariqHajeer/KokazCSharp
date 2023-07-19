using Quqaz.Web.Models.Infrastrcuter;
using System.ComponentModel.DataAnnotations;

namespace Quqaz.Web.Models
{
    public class Driver:IIndex
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
