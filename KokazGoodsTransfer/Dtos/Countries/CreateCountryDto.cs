using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.Countries
{
    public class CreateCountryDto
    {
        public string Name { get; set; }
        public decimal DeliveryCost { get; set; }
        public string[] Regions { get; set; }
        public int? MediatorId { get; set; }
        public int Points { get; set; } = 0;
    }
}
