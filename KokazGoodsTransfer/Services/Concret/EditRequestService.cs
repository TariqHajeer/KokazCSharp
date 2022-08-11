using AutoMapper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.EditRequestDtos;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace KokazGoodsTransfer.Services.Concret
{
    public class EditRequestService : CRUDService<EditRequest, EditRequestDto, CreateEditRequestDto, UpdateEditRequestDto>, IEditRequestService
    {
        public EditRequestService(IRepository<EditRequest> repository, IMapper mapper, Logging logging, IHttpContextAccessorService httpContextAccessorService) : base(repository, mapper, logging, httpContextAccessorService)
        {
        }
    }
}
