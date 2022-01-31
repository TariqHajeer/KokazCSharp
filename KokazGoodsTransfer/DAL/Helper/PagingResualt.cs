using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.DAL.Helper
{
    public class PagingResualt<T>
    {
        public int Total { get; set; }
        public T Data { get; set; }
    }
}
