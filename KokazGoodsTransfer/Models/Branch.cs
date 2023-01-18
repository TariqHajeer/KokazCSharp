using KokazGoodsTransfer.Models.Infrastrcuter;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KokazGoodsTransfer.Models
{
    public class Branch : IIndex
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]
        public virtual Country Country { get; set; }
        public virtual List<MediatorBranch> FromBranches { get; set; }
        public virtual List<MediatorBranch> MediatorBranches { get; set; }
        public virtual List<MediatorBranch> ToBranches { get; set; }
        public virtual ICollection<UserBranch> Users { get; set; }
    }
}
