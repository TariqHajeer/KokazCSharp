using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.Clients
{
    public class CreateClientDto
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? RegionId { get; set; }
        public string Address { get; set; }
        public DateTime FirstDate { get; set; }
        public string Note { get; set; }

    }
}
