using Microsoft.AspNetCore.Mvc.RazorPages;
using Quqaz.Web.Dtos.OrdersDtos;
using System;

namespace Quqaz.Web.Dtos.Common
{
    public class DateWithId<T>
    {
        public DateTime Date { get; set; }
        public T Ids { get; set; }
    }
    public class DeleiverMoneyForClientDto2
    {
        public OrderDontFinishedFilter Filter { get; set; }
        //public bool IsSelectedAll { get; set; }
        //public int[] SelectedIds { get; set; }
        //public int[] ExceptIds { get; set; }
        public int? PointsSettingId { get; set; }
    }
}
