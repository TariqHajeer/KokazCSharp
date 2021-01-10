using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class OutCome
    {

        public int Id { get; set; }
        public int CurrencyId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public string Note { get; set; }
        public int UserId { get; set; }
        public int OutComeTypeId { get; set; }

        public virtual Currency Currency { get; set; }
        public virtual OutComeType OutComeType { get; set; }
        public virtual User User { get; set; }
    }
}
