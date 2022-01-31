using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.DAL.Helper
{
    public class Paging
    {
        int rowCount = 10;
        public int RowCount { get => rowCount; set => rowCount = Math.Min(100, value); }
        public int Page { get; set; } = 1;
    }
}
