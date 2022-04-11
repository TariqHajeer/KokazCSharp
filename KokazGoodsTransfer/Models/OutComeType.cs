using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class OutComeType
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
