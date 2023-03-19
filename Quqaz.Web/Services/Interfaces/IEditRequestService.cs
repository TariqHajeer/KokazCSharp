using Quqaz.Web.Dtos.EditRequestDtos;
using Quqaz.Web.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace Quqaz.Web.Services.Interfaces
{
    public interface IEditRequestService : ICRUDService<EditRequest, EditRequestDto, CreateEditRequestDto, UpdateEditRequestDto>
    {
        Task<IEnumerable<EditRequestDto>> NewEditRequest();
        Task DisAccpet(int id);
        Task Accept(int id);
    }
}
