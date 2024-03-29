﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Quqaz.Web.Models
{
    public partial class CashMovment
    {
        public CashMovment()
        {
            TreasuryHistories = new HashSet<TreasuryHistory>();
        }

        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int TreasuryId { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string CreatedBy { get; set; }
        public string Note { get; set; }

        public virtual Treasury Treasury { get; set; }
        public virtual ICollection<TreasuryHistory> TreasuryHistories { get; set; }
    }
}
