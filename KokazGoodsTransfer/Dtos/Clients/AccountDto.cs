using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.Clients
{
    public class AccountDto
    {
        public int ClinetId { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; } = "";
        public string About { get; set; }
        public string Manager { get; set; }
        public bool IsPay { get; set; }
        public string Mail { get; set; }
    }
}
