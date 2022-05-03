using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class Treasury
    {
        public Treasury()
        {
            TreasuryHistories = new HashSet<TreasuryHistory>();
        }

        public int Id { get; set; }
        public decimal Total { get; set; }
        public DateTime CreateOnUtc { get; set; }
        public bool IsActive { get; set; }

        public virtual User IdNavigation { get; set; }
        public virtual ICollection<TreasuryHistory> TreasuryHistories { get; set; }
    }
}
