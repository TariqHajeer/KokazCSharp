using System.ComponentModel.DataAnnotations.Schema;

namespace KokazGoodsTransfer.Models.Infrastrcuter
{
    public interface IMaybeHaveBranch
    {
        public int? BranchId { get; set; }
        [ForeignKey(nameof(BranchId))]
        public Branch Branch { get; set; }
    }
}
