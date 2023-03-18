using AutoMapper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.BranchDtos;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Helper;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Concret
{
    public class BranchService : CRUDService<Branch, BranchDto, CreateBranchDto, UpdateBranchDto>, IBranchService
    {
        public BranchService(IRepository<Branch> repository, IMapper mapper, Logging logging, IHttpContextAccessorService httpContextAccessorService) : base(repository, mapper, logging, httpContextAccessorService)
        {
        }
        public override Task<ErrorRepsonse<BranchDto>> AddAsync(CreateBranchDto createDto)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<NameAndIdDto>> GetLite()
        {
            var braches = await _repository.GetAll();
            return _mapper.Map<IEnumerable<NameAndIdDto>>(braches);
        }
    }
}
