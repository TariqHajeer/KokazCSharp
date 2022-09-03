using KokazGoodsTransfer.Models.Infrastrcuter;
using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class Income :IHaveBranch ,IIdEntity
    {
        public Income()
        {
            TreasuryHistories = new HashSet<TreasuryHistory>();
        }

        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public decimal Earining { get; set; }
        public string Note { get; set; }
        public int UserId { get; set; }
        public int IncomeTypeId { get; set; }

        public virtual IncomeType IncomeType { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<TreasuryHistory> TreasuryHistories { get; set; }
        public int BranchId { get; set; }
        public Branch Branch { get; set; }
    }
}
