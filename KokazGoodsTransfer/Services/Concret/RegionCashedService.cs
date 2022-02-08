using AutoMapper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.Regions;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Helper;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Concret
{
    public class RegionCashedService : CashService<Region, RegionDto, CreateRegionDto, UpdateRegionDto>, IRegionCashedService
    {
        public RegionCashedService(IRepository<Region> repository, IMapper mapper, IMemoryCache cache) : base(repository, mapper, cache)
        {
        }
        public override async Task<ErrorRepsonse<RegionDto>> AddAsync(CreateRegionDto createDto)
        {
            var response = new ErrorRepsonse<RegionDto>();
            var similar = await _repository.Any(c => c.Name == createDto.Name && c.CountryId == createDto.CountryId);
            if (similar)
            {
                response.Errors.Add("Region.Exisit");
                return response;
            }
            return await base.AddAsync(createDto);
        }
        public override async Task<IEnumerable<RegionDto>> GetCashed()
        {
            var name = typeof(Region).FullName;
            if (!_cache.TryGetValue(name, out IEnumerable<RegionDto> entites))
            {
                var list = await GetAsync(null, c => c.Country);
                entites = _mapper.Map<RegionDto[]>(list);
                _cache.Set(name, entites);
            }
            return entites;
        }

    }
}
