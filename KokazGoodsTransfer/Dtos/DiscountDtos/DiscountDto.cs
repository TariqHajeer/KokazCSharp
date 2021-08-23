using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.DiscountDtos
{
    public class DiscountDto
    {
        public int Id { get; set; }
        public int Points { get; set; }
        public decimal Money { get; set; }
    }
}
