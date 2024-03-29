using Quqaz.Web.Dtos.Additional.ClientMessageDtos;
using Quqaz.Web.Models.Additional;
using System.Threading.Tasks;

namespace Quqaz.Web.Services.Interfaces.Additional
{
    public interface IClientMessageService: ICRUDService<ClientMessage, ClientMessageDto,CreateClientMessageDto,ClientMessageDto>
    {
        Task Publish(int id);
        Task UnPublish(int id);
    }
}
