using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.Clients
{
    public class ClientBlanaceDto
    {

        public string ClientName { get; set; }
        public decimal Account { get; set; }
        public decimal TotalOrder { get; set; }
    }
}
