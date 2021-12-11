using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.AgentDtos
{
    public class AgentStaticsDto
    {
        public int TotlaOwedOrder { get; set; }
        public int TotalOrderInSotre { get; set; }
        public int TotalOrderInWay { get; set; }
        public int TotalOrderSuspended { get; set; }
        public int TotlaPrintOrder { get; set; }
    }
}
