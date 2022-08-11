using AutoMapper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.BranchDtos;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Helper;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Concret
{
    public class BranchService : CRUDService<Branch, BranchDto, CreateBranchDto, UpdateBranchDto>, IBranchService
    {
        public BranchService(IRepository<Branch> repository, IMapper mapper, Logging logging, IHttpContextAccessorService httpContextAccessorService) : base(repository, mapper, logging, httpContextAccessorService)
        {
        }
        public override async Task<ErrorRepsonse<BranchDto>> AddAsync(CreateBranchDto createDto)
        {
            var any =await _repository.Any(c=>c.CountryId==createDto.CountryId);
            if (any)
            {

            }
            return await base.AddAsync(createDto);
        }
    }
}
