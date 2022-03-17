using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Helper
{
    public class ErrorRepsonse<T>
    {
        public ErrorRepsonse(T Data)
        {
            this.Data = Data;
            Errors = new List<string>();
        }
        public ErrorRepsonse()
        {
            Errors = new List<string>();
        }
        public T Data { get; set; }
        public List<string> Errors { get; set; }
        public bool NotFound { get; set; }
        public bool CantDelete { get; set; }
        public bool Sucess => CantDelete == false && NotFound == false && !Errors.Any();
    }
}
