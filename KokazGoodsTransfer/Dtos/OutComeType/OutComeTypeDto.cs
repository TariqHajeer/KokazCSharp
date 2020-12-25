using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.OutComeType
{
    public class OutComeTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool CanDelete { get; set; }
    }
}
