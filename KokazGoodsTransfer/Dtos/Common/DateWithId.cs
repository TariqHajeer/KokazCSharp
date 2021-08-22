using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.Common
{
    public class DateWithId<T>
    {
        public DateTime Date { get; set; }
        public T Ids { get; set; }
    }
    public class DeleiverMoneyForClientDto
    {
        public DateWithId<int[]> DateWithId { get; set; }
        public int PointsSettingId { get; set; }
    }
    //public class IdWithCost
    //{
    //    public int Id { get; set; }
    //    public decimal Cost { get; set; }
    //}

}
