namespace Quqaz.Web.DAL.Helper
{
    public class LiteEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class LiteCanDelete:LiteEntity
    {
        public bool CanDelete { get; set; }
    }
}
