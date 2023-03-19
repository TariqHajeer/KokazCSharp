using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Dtos.Regions
{
    public class CreateRegionDto
    {
        public string Name { get; set; }
        public int CountryId { get; set; }
    }
}
