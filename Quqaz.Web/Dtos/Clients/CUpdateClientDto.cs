using Microsoft.AspNetCore.Http;

namespace Quqaz.Web.Dtos.Clients
{
    public class CUpdateClientDto
    {
        public IFormFile? Photo { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string[] Phones { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string FacebookLinke { get; set; }
        public string IGLink { get; set; }
    }
}
