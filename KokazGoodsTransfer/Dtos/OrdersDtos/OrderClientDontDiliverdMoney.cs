using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.OrdersDtos
{
    public class OrderClientDontDiliverdMoney
    {
        public int ClientId { get; set; }
        public int[] OrderPlacedId { get; set; } 
    }
}
