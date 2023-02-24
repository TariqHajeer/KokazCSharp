using KokazGoodsTransfer.Dtos.Regions;
using KokazGoodsTransfer.Dtos.Users;
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
        public bool IsMain { get; set; }
        public int Points { get; set; } 
        public List<RegionDto> Regions { get; set; }
        public List<UserDto> Agnets { get; set; }
        public CountryDto Mediator { get; set; }
        public IEnumerable<int> CountriesNeedMidBranch { get; set; }
        public bool HaveBranch { get; set; }

    }   
}
