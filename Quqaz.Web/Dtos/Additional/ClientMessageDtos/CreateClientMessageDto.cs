using Microsoft.AspNetCore.Http;

namespace Quqaz.Web.Dtos.Additional.ClientMessageDtos
{
    public class CreateClientMessageDto
    {
        public string Message { get; set; }
        public string Name { get; set; }
        public IFormFile Logo { get; set; }
    }
}
