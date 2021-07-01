using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.ClientDtos
{
    public class StaticsDto
    {
        public int TotalOrder { get; set; }
        public int OrderWithClient { get; set; }
        public int OrderInWay { get; set; }
        public int OrderInCompany { get; set; }
        public int DeliveredOrder { get; set; }
        public int CompletelyReturnedOrder { get; set; }
        public int PartialReturnedOrder { get; set; }
        public int UnacceptableOrder { get; set; }
        public int DelayedOrder { get; set; }
    }
}
