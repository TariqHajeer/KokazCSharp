using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Dtos.Countries
{
    public class UpdateCountryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal DeliveryCost { get; set; }
        public Int16 Points { get; set; } = 0;
    }
}
