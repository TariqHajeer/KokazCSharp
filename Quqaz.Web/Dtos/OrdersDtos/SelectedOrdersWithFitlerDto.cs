using Quqaz.Web.Dtos.Common;

namespace Quqaz.Web.Dtos.OrdersDtos
{
    public class SelectedOrdersWithFitlerDto
    {
        public OrderFilter OrderFilter { get; set; }
        public bool IsSelectedAll { get; set; }
        public int[] SelectedIds { get; set; }
        public int[] ExceptIds { get; set; }
        public PagingDto Paging { get; set; }
    }
}
