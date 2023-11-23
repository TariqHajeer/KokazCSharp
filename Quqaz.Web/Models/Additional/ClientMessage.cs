using Quqaz.Web.Models.Infrastrcuter;

namespace Quqaz.Web.Models.Additional
{
    public class ClientMessage : IIdEntity
    {
        public int Id { get; set; }
        public string Logo { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public bool IsPublished { get; set; }
    }
}
