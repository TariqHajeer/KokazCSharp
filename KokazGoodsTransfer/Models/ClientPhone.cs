using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class ClientPhone
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Phone { get; set; }

        public virtual Client Client { get; set; }
    }
}
