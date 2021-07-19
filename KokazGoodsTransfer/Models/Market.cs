using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class Market
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MarketUrl { get; set; }
        public string LogoPath { get; set; }
        public string Description { get; set; }
        public int? ClientId { get; set; }
        public bool IsActive { get; set; }

        public virtual Client Client { get; set; }
    }
}
