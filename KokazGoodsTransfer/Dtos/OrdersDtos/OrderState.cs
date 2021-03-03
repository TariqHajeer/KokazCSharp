using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.OrdersDtos
{
    public class OrderStateDto
    {
        public int Id { get; set; }
        public decimal Cost { get; set; }
        public int MoenyPlacedId { get; set; }
        public int OrderplacedId { get; set; }

    }
}
