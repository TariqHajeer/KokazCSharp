using KokazGoodsTransfer.Models.Infrastrcuter;

namespace KokazGoodsTransfer.Models
{
    public partial class PointsSetting : IIdEntity, IHaveBranch
    {
        public int Id { get; set; }
        public int Points { get; set; }
        public decimal Money { get; set; }
        public int BranchId { get; set; }
        public Branch Branch { get; set; }
    }
}
