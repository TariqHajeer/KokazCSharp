using System.ComponentModel.DataAnnotations.Schema;

namespace Quqaz.Web.Models.Infrastrcuter
{
    public interface IMaybeHaveBranch
    {
        public int? BranchId { get; set; }
        [ForeignKey(nameof(BranchId))]
        public Branch Branch { get; set; }
    }
}
