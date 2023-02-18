using System.ComponentModel.DataAnnotations.Schema;

namespace KokazGoodsTransfer.Models
{
    public class MediatorCountry
    {
        public int FromCountryId { get; set; }
        public int MediatorCountryId { get; set; }
        public int ToCountryId { get; set; }
        [ForeignKey(nameof(FromCountryId))]
        public Country FromCountry { get; set; }
        [ForeignKey(nameof(MediatorCountryId))]
        public Country MidCountry { get; set; }
        [ForeignKey(nameof(ToCountryId))]
        public Country ToCountry { get; set; }
    }
}
