using KokazGoodsTransfer.Models.Infrastrcuter;
using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class MoenyPlaced: IIndex
    {
        public MoenyPlaced()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
