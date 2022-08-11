using AutoMapper;
using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models.Infrastrcuter;
using KokazGoodsTransfer.Services.Helper;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Concret
{
    public class CashService<TEntity, TDTO, CreateDto, UpdateDto> : CRUDService<TEntity, TDTO, CreateDto, UpdateDto>, ICashService<TEntity, TDTO, CreateDto, UpdateDto> where TEntity : class, IIdEntity where TDTO : class where CreateDto : class where UpdateDto : class
    {

        protected readonly IMemoryCache _cache;
        protected readonly string cashName;
        public CashService(IRepository<TEntity> repository, IMapper mapper, IMemoryCache cache, Logging logging, IHttpContextAccessorService httpContextAccessorService) : base(repository, mapper, logging, httpContextAccessorService)
        {
            _cache = cache;
            cashName = typeof(TEntity).FullName + _currentBranch;

        }
        public override async Task<ErrorRepsonse<TDTO>> AddAsync(CreateDto createDto)
        {
            var repsonse = await base.AddAsync(createDto);
            RemoveCash();
            return repsonse;
        }

        public override async Task<ErrorRepsonse<TDTO>> Delete(int id)
        {
            var repsonse = await base.Delete(id);
            if (!repsonse.Errors.Any())
                RemoveCash();
            return repsonse;

        }
        public async override Task<ErrorRepsonse<TDTO>> Update(UpdateDto updateDto)
        {
            var response = await base.Update(updateDto);
            if (!response.Errors.Any())
                RemoveCash();
            return response;
        }
        public virtual async Task<IEnumerable<TDTO>> GetCashed()
        {
            if (!_cache.TryGetValue(cashName, out IEnumerable<TDTO> entites))
            {
                var list = await GetAsync();
                entites = _mapper.Map<TDTO[]>(list);
                _cache.Set(cashName, entites, new TimeSpan(8, 0, 0));
            }
            return entites;
        }

        public void RemoveCash()
        {
            _cache.Remove(cashName);
        }
    }
}
