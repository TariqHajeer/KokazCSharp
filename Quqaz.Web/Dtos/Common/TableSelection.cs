namespace Quqaz.Web.Dtos.Common
{
    public class TableFilterDto
    {
        public bool IsSelectedAll { get; set; }
        public int[] SelectedIds { get; set; }
        public int[] ExceptIds { get; set; }
    }
}
