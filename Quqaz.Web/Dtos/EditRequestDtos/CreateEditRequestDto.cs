namespace Quqaz.Web.Dtos.EditRequestDtos
{
    public class CreateEditRequestDto
    {
        public int ClientId { get; set; }
        public string OldName { get; set; }
        public string NewName { get; set; }
        public string OldUserName { get; set; }
        public string NewUserName { get; set; }
    }
}
