using Quqaz.Web.Dtos.Common;

namespace Quqaz.Web.Dtos.OrdersDtos
{
    public class SelectedOrdersWithFitlerDto<T>
    {
        public OrderFilter OrderFilter { get; set; }
        public bool IsSelectedAll { get; set; }
        public T[] SelectedIds { get; set; }
        public int[] ExceptIds { get; set; }
        public PagingDto Paging { get; set; }
    }
}
