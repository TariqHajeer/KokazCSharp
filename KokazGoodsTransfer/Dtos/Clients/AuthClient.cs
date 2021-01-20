using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.Clients
{
    public class AuthClient
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public string Address { get; set; }
        public RegionDto Region { get; set; }
        public List<PhoneDto> Phones { get; set; }
    }
}
