using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.OrdersDtos
{
    public class IdCost
    {
        public int Id { get; set; }
        public decimal Cost { get; set; }
    }
    public class DateIdCost
    {
        public DateTime Date { get; set; }
        public List<IdCost> IdCosts { get; set; }
    }
}
