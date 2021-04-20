using KokazGoodsTransfer.Dtos.Countries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.Regions
{
    public class RegionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool CanDelete { get; set; } = true;
        public CountryDto Country { get; set; }
    }
}
