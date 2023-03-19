using System.ComponentModel.DataAnnotations.Schema;

namespace Quqaz.Web.Models
{
    public class UserBranch
    {
        public int UserId { get; set; }
        public int BranchId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
        [ForeignKey(nameof(BranchId))]
        public virtual Branch Branch  { get; set; }
    }
}
