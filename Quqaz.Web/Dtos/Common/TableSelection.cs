namespace Quqaz.Web.Dtos.Common
{
    public class TableSelection
    {
        public bool IsSelectedAll { get; set; }
        public int[] SelectedIds { get; set; }
        public int[] ExceptIds { get; set; }
    }
}
