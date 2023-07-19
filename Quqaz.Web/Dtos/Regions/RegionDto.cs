using Quqaz.Web.Dtos.Countries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Dtos.Regions
{
    public class RegionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool CanDelete { get; set; } = true;
        public CountryDto Country { get; set; }
    }
}
