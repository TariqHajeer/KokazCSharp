using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.Clients
{
    public class CUpdateClientDto
    {
        public string Password { get; set; }
        public int? RegionId { get; set; }
        public string Address { get; set; }
    }
}
