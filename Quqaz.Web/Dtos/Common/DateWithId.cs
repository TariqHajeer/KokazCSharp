using System;

namespace Quqaz.Web.Dtos.Common
{
    public class DateWithId<T>
    {
        public DateTime Date { get; set; }
        public T Ids { get; set; }
    }
    public class DeleiverMoneyForClientDto
    {
        public int[] Ids { get; set; }
        public int? PointsSettingId { get; set; }
    }
    public class DeleiverMoneyForClientDto2
    {

    }
}
