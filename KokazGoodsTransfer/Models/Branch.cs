using KokazGoodsTransfer.Models.Infrastrcuter;
using System.ComponentModel.DataAnnotations.Schema;

namespace KokazGoodsTransfer.Models
{
    public class Branch : IIndex
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey(nameof(Id))]
        public virtual Country Country { get; set; }
    }
}
