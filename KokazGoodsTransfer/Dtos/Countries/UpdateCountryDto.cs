using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.Countries
{
    public class UpdateCountryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UpdateRegionsNestadeCountry> Regions { get; set; }
    }
    public class UpdateRegionsNestadeCountry
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
