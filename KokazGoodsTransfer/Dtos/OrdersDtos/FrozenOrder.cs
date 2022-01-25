using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.OrdersDtos
{
    public class FrozenOrder
    {
        [Required(ErrorMessage ="Hour is Requird")]
        public int Hour { get; set; }
        public int? AgentId { get; set; }
        public DateTime CurrentDate { get; set; }
    }
}
