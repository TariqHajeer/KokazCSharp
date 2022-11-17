using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace KokazGoodsTransfer.Models.TransferToBranchModels
{
    public class TransferToOtherBranchDetials
    {
        public int Id { get; set; }
        public int TransferToOtherBranchId { get; set; }
        public string Code { get; set; }
        public decimal Total { get; set; }
        public string CountryName { get; set; }
        public string ClientName { get; set; }
        public DateTime OrderDate { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }




        [ForeignKey(nameof(TransferToOtherBranchId))]
        public virtual TransferToOtherBranch TransferToOtherBranch { get; set; }
    }
}
