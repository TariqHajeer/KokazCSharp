using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.Clients
{
    public class AccountFilterDto
    {
        public bool? IsPay { get; set; }
        public int? ClientId { get; set; }
        public DateTime? Date { get; set; }
    }
}
