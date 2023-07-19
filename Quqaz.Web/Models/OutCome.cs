using Quqaz.Web.Models.Infrastrcuter;
using System;
using System.Collections.Generic;

#nullable disable

namespace Quqaz.Web.Models
{
    public partial class OutCome : IHaveBranch, IIdEntity
    {
        public OutCome()
        {
            TreasuryHistories = new HashSet<TreasuryHistory>();
        }

        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public string Note { get; set; }
        public int UserId { get; set; }
        public int OutComeTypeId { get; set; }

        public virtual OutComeType OutComeType { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<TreasuryHistory> TreasuryHistories { get; set; }
        public int BranchId { get; set; }
        public Branch Branch { get; set; }
    }
}
