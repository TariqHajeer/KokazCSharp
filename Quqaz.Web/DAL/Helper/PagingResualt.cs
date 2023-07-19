namespace Quqaz.Web.DAL.Helper
{
    public class PagingResualt<T>
    {
        public int Total { get; set; }
        public T Data { get; set; }
    }
}
