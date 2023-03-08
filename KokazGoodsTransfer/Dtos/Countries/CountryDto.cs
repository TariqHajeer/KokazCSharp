using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.Regions;
using KokazGoodsTransfer.Dtos.Users;
using System;
using System.Collections.Generic;

namespace KokazGoodsTransfer.Dtos.Countries
{
    public class CountryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal DeliveryCost { get; set; }
        public bool CanDelete { get; set; }
        public bool CanDeleteWithRegion { get; set; }
        public Int16 Points { get; set; }
        public List<NameAndIdDto> Regions { get; set; }
        public List<NameAndIdDto> Agnets { get; set; }
        public bool RequiredAgent { get; set; }

    }
}
