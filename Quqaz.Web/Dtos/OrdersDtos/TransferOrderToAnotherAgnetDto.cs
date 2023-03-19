using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Dtos.OrdersDtos
{
    public class TransferOrderToAnotherAgnetDto
    {
        public int NewAgentId { get; set; }
        public int[] Ids { get; set; }
    }
}
