using AutoMapper;
using Quqaz.Web.DAL.Infrastructure.Interfaces;
using Quqaz.Web.Dtos.BranchDtos;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Helpers;
using Quqaz.Web.Models;
using Quqaz.Web.Services.Helper;
using Quqaz.Web.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quqaz.Web.Services.Concret
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
