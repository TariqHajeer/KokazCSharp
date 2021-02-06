using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.Common
{
    public class PagingDto
    {
        int rowCount = 100;
        public int RowCount { get => rowCount; set => rowCount = Math.Min(100, value); }
        public int Page { get; set; } = 1;
    }

}
