using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace KokazGoodsTransfer.Models
{
    public class BranchToCountryDeliverryCost
    {
        public int BranchId { get; set; }
        public int CountryId { get; set; }
        [ForeignKey(nameof(BranchId))]
        public Branch Branch { get; set; }
        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; }
        
        public decimal DeliveryCost { get; set; }
        public Int16 Points { get; set; }
    }
}
