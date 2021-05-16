using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.OrdersDtos
{
    public class OrderReSend
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public int RegionId { get; set; }
        public int AgnetId { get; set; }

    }
}
