using AutoMapper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.EditRequestDtos;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Concret
{
    public class EditRequestService : CRUDService<EditRequest, EditRequestDto, CreateEditRequestDto, UpdateEditRequestDto>, IEditRequestService
    {
        private readonly IUintOfWork _uintOfWork;
        public EditRequestService(IRepository<EditRequest> repository, IMapper mapper, Logging logging, IHttpContextAccessorService httpContextAccessorService, IUintOfWork uintOfWork) : base(repository, mapper, logging, httpContextAccessorService)
        {
            _uintOfWork = uintOfWork;
        }

        public async Task Accept(int id)
        {
            var editRequest = await _uintOfWork.Repository<EditRequest>().FirstOrDefualt(c => c.Id == id);
            editRequest.Accept = true;
            editRequest.UserId = _httpContextAccessorService.AuthoticateUserId();
            var client = await _uintOfWork.Repository<Client>().FirstOrDefualt(c=>c.Id==editRequest.ClientId);
            client.Name = editRequest.NewName;
            client.UserName = editRequest.NewUserName;
            await _uintOfWork.Update(editRequest);
            await _uintOfWork.Update(client);
            await _uintOfWork.Commit();
        }

        public async Task DisAccpet(int id)
        {
            var editRequest = await _repository.FirstOrDefualt(c => c.Id == id);
            editRequest.Accept = false;
            editRequest.UserId = _httpContextAccessorService.AuthoticateUserId();
            await _repository.Update(editRequest);
        }

        public async Task<IEnumerable<EditRequestDto>> NewEditRequest()
        {
            var editRequests = await _repository.GetAsync(c => c.Accept == null, c => c.Client);
            return _mapper.Map<IEnumerable<EditRequestDto>>(editRequests);
        }
    }
}
