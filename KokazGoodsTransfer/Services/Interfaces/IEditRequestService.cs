using KokazGoodsTransfer.Dtos.EditRequestDtos;
using KokazGoodsTransfer.Models;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IEditRequestService : ICRUDService<EditRequest, EditRequestDto,CreateEditRequestDto,UpdateEditRequestDto>
    {
    }
}
