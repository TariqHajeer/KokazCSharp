using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class PersonsPhone
    {
        public int OrderId { get; set; }
        public string Phone { get; set; }

        public virtual Order Order { get; set; }
    }
}
