using KokazGoodsTransfer.Dtos.EditRequestDtos;
using KokazGoodsTransfer.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IEditRequestService : ICRUDService<EditRequest, EditRequestDto, CreateEditRequestDto, UpdateEditRequestDto>
    {
        Task<IEnumerable<EditRequestDto>> NewEditRequest();
        Task DisAccpet(int id);
        Task Accept(int id);
    }
}
