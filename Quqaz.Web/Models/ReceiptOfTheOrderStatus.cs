using System;
using System.Collections.Generic;

#nullable disable

namespace Quqaz.Web.Models
{
    public partial class ReceiptOfTheOrderStatus
    {
        public ReceiptOfTheOrderStatus()
        {
            ReceiptOfTheOrderStatusDetalis = new HashSet<ReceiptOfTheOrderStatusDetali>();
            TreasuryHistories = new HashSet<TreasuryHistory>();
        }

        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public int RecvierId { get; set; }

        public virtual User Recvier { get; set; }
        public virtual ICollection<ReceiptOfTheOrderStatusDetali> ReceiptOfTheOrderStatusDetalis { get; set; }
        public virtual ICollection<TreasuryHistory> TreasuryHistories { get; set; }
    }
}
