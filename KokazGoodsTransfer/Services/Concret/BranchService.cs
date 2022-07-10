using AutoMapper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.BranchDtos;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace KokazGoodsTransfer.Services.Concret
{
    public class BranchService : CRUDService<Branch, BranchDto, CreateBranchDto, UpdateBranchDto>, IBranchService
    {
        public BranchService(IRepository<Branch> repository, IMapper mapper, Logging logging, IHttpContextAccessor httpContextAccessor) : base(repository, mapper, logging,httpContextAccessor)
        {
        }

    }
}
