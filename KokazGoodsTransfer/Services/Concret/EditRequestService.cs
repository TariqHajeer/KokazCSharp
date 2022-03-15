using AutoMapper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.EditRequestDtos;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Interfaces;
namespace KokazGoodsTransfer.Services.Concret
{
    public class EditRequestService : CRUDService<EditRequest, EditRequestDto, CreateEditRequestDto, UpdateEditRequestDto>, IEditRequestService
    {
        public EditRequestService(IRepository<EditRequest> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
