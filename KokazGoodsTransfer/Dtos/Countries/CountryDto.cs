using KokazGoodsTransfer.Dtos.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.Countries
{
    public class CountryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool CanDelete { get; set; }
        public List<RegionDto> Regions { get; set; }

    }
}
