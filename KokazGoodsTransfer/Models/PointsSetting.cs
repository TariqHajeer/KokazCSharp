using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class PointsSetting
    {
        public int Id { get; set; }
        public int Points { get; set; }
        public decimal Money { get; set; }
    }
}
