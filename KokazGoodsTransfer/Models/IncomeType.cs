using KokazGoodsTransfer.Models.Infrastrcuter;
using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class IncomeType : IIndex
    {
        public IncomeType()
        {
            Incomes = new HashSet<Income>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Income> Incomes { get; set; }
    }
}
