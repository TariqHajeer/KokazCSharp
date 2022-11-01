using System.ComponentModel.DataAnnotations.Schema;

namespace KokazGoodsTransfer.Models.TransferToBranchModels
{
    public class TransferToOtherBranchDetials
    {
        public int Id { get; set; }
        public int TransferToOtherBranchId { get; set; }
        public string Code { get; set; }
        public decimal Total { get; set; }
        public int CountryId { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
        [ForeignKey(nameof(CountryId))]
        public virtual Country Country { get; set; }




        [ForeignKey(nameof(TransferToOtherBranchId))]
        public virtual TransferToOtherBranch TransferToOtherBranch { get; set; }
    }
}
