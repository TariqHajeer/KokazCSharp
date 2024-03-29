using AutoMapper;
using Quqaz.Web.Dtos.Additional.ClientMessageDtos;
using Quqaz.Web.Models.Additional;

namespace Quqaz.Web.Dtos.AutoMapperProfile
{
    public class ClientMessageProfile:Profile
    {
        public ClientMessageProfile()
        {
            CreateMap<ClientMessage, ClientMessageDto>();
        }
    }
}
