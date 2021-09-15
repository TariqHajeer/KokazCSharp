using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.Clients
{
    public class GiveOrDiscountPointsDto
    {
        public int ClientId { get; set; }
        public bool IsGive { get; set; }
        public int Points { get; set; }
    }
}
