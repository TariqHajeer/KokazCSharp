using Quqaz.Web.Models.Infrastrcuter;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quqaz.Web.Models
{
    public class Branch : IIndex
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey(nameof(Id))]
        public virtual Country Country { get; set; }
        public virtual ICollection<BranchToCountryDeliverryCost> BranchToCountryDeliverryCosts { get; set; }
    }
}
