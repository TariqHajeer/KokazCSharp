using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.Common
{
    public class PagingDetalis
    {
        public int TotalRows { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public bool HasNexPage { get; set; }
        public bool HasPreviousPage { get; set; }
        public string NextUrl { get; set; }
        public string PreviousUrl { get; set; }
    }
}
