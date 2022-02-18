using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.Countries;
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
        public string Mail { get; set; }
        public CountryDto Country { get; set; }
        public List<PhoneDto> Phones { get; set; }
        public bool CanAddOrder { get; set; }

    }
}
