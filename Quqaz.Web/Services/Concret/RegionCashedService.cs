using AutoMapper;
using Quqaz.Web.DAL.Infrastructure.Interfaces;
using Quqaz.Web.Dtos.Regions;
using Quqaz.Web.Helpers;
using Quqaz.Web.Models;
using Quqaz.Web.Services.Helper;
using Quqaz.Web.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Services.Concret
{
    public class RegionCashedService : CashService<Region, RegionDto, CreateRegionDto, UpdateRegionDto>, IRegionCashedService
    {

        public RegionCashedService(IRepository<Region> repository, IMapper mapper, IMemoryCache cache, Logging logging, IHttpContextAccessorService httpContextAccessorService)
            : base(repository, mapper, cache, logging, httpContextAccessorService)
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
            var entity = _mapper.Map<Region>(createDto);
            await _repository.AddAsync(entity);
            await _repository.LoadRefernces(entity, c => c.Country);
            response = new ErrorRepsonse<RegionDto>(_mapper.Map<RegionDto>(entity));
            RemoveCash();
            return response;
        }
        public override async Task<ErrorRepsonse<RegionDto>> Update(UpdateRegionDto updateDto)
        {
            var region = await _repository.GetById(updateDto.Id);
            var similar = await _repository.Any(c => c.CountryId == region.CountryId && c.Name == updateDto.Name && c.Id != updateDto.Id);
            if (similar)
            {
                return new ErrorRepsonse<RegionDto>()
                {
                    Errors = new List<string>()
                    {
                        "Region.Exsist"
                    }
                };
            }

            region.Name = updateDto.Name;
            await _repository.Update(region);
            var response = new ErrorRepsonse<RegionDto>(_mapper.Map<RegionDto>(region));
            if (!response.Errors.Any())
                RemoveCash();
            return response;
        }
        public override async Task<IEnumerable<RegionDto>> GetCashed()
        {
            if (!_cache.TryGetValue(cashName, out IEnumerable<RegionDto> entites))
            {
                entites = await GetAsync(null, c => c.Country);
                _cache.Set(cashName, entites);
            }
            return entites;
        }

    }
}
