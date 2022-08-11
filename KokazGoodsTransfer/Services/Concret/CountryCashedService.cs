using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Helper;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace KokazGoodsTransfer.Services.Concret
{
    public class CountryCashedService : CashService<Country, CountryDto, CreateCountryDto, UpdateCountryDto>, ICountryCashedService
    {
        public CountryCashedService(IRepository<Country> repository, IMapper mapper, IMemoryCache cache, Logging logging, IHttpContextAccessorService httpContextAccessorService) 
            : base(repository, mapper, cache, logging, httpContextAccessorService)
        {
        }

        public override async Task<ErrorRepsonse<CountryDto>> AddAsync(CreateCountryDto createDto)
        {
            var response = new ErrorRepsonse<CountryDto>();
            var similer = await _repository.FirstOrDefualt(c => c.Name == createDto.Name);
            if (similer != null)
            {
                response.Errors.Add("Country.Similar");
                return response;
            }
            var country = _mapper.Map<Country>(createDto);
            if (!(await _repository.Any()))
            {
                country.IsMain = true;
            }

            await _repository.AddAsync(country);
            if (country.MediatorId != null)
                await _repository.LoadRefernces(country, c => c.Mediator);
            response = new ErrorRepsonse<CountryDto>(_mapper.Map<CountryDto>(country));
            RemoveCash();
            return response;
        }
        public override async Task<ErrorRepsonse<CountryDto>> Delete(int id)
        {
            var country = await _repository.GetById(id);
            await _repository.LoadCollection(country, c => c.Clients);
            await _repository.LoadCollection(country, c => c.AgentCountries);
            var response = new ErrorRepsonse<CountryDto>();
            if (country.AgentCountries.Any() || country.Clients.Any())
            {
                response.Errors.Add("Cant.Delete");
                return response;
            }
            response = await base.Delete(id);
            return response;
        }
        public override async Task<IEnumerable<CountryDto>> GetCashed()
        {
            if (!_cache.TryGetValue(cashName, out IEnumerable<CountryDto> entites))
            {
                entites = await GetAsync(null, c => c.Regions, c => c.Clients, c => c.AgentCountries.Select(a => a.Agent));
                _cache.Set(cashName, entites);
            }
            return entites;
        }
        public override async Task<ErrorRepsonse<CountryDto>> Update(UpdateCountryDto updateDto)
        {
            var similar = await _repository.Any(c => c.Name == updateDto.Name && c.Id != updateDto.Id);
            var response = new ErrorRepsonse<CountryDto>();
            if (similar)
            {
                response.Errors.Add("There.Is.Similar.Country");
                return response;
            }

            response = await base.Update(updateDto);
            return response;
        }

        public async Task SetMainCountry(int id)
        {
            var country = await _repository.GetById(id);
            var mainCountry = (await _repository.GetAsync(c => c.IsMain == true)).ToList();
            country.IsMain = true;
            mainCountry.ForEach(c => c.IsMain = false);
            mainCountry.Add(country);
            await _repository.Update(mainCountry);
        }
    }
}
