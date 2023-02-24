using KokazGoodsTransfer.Models.Infrastrcuter;
using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class OutComeType: IIndex
    {
        public OutComeType()
        {
            OutComes = new HashSet<OutCome>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<OutCome> OutComes { get; set; }
    }
}
