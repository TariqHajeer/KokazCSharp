using KokazGoodsTransfer.Dtos.Regions;
using KokazGoodsTransfer.Dtos.Users;
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
        public decimal DeliveryCost { get; set; }
        public bool CanDelete { get; set; }
        public bool CanDeleteWithRegion { get; set; }
        public int[] BranchesIds { get; set; }
        public bool IsMain { get; set; }
        public int Points { get; set; } 
        public List<RegionDto> Regions { get; set; }
        public List<UserDto> Agnets { get; set; }
        public CountryDto Mediator { get; set; }


    }   
}
