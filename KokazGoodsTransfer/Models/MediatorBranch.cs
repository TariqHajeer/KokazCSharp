using System.ComponentModel.DataAnnotations.Schema;

namespace KokazGoodsTransfer.Models
{
    public class MediatorBranch
    {

        public int FromBranchId { get; set; }
        public int ToBranchId { get; set; }
        
        public int MediatorBranchId { get; set; }

        [ForeignKey(nameof(FromBranchId))]
        public virtual Branch FromBranch { get; set; }
        [ForeignKey(nameof(ToBranchId))]
        public virtual Branch ToBranch { get; set; }
        [ForeignKey(nameof(MediatorBranchId))]
        public virtual Branch MidBranch { get; set; }
    }
}
