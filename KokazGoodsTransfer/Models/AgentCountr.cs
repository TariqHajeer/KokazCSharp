using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class AgentCountr
    {
        public int AgentId { get; set; }
        public int CountryId { get; set; }

        public virtual User Agent { get; set; }
        public virtual Country Country { get; set; }
    }
}
