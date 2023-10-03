namespace Quqaz.Web.Dtos.OrdersDtos.Common
{
    public class SelectionOrderFilter<T> where T : class
    {

        public T Filter { get; set; }
        public bool IsSelectedAll { get; set; }
        public int[] SelectedIds { get; set; }
        public int[] ExceptIds { get; set; }
    }
}
