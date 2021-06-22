using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.Common
{
    public class DateWithId
    {
        public DateTime Date { get; set; }
        public int[] Ids { get; set; }
    }
}
