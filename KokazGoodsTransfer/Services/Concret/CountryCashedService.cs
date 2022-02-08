using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Helper;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace KokazGoodsTransfer.Services.Concret
{
    public class CountryCashedService : CashService<Country, CountryDto, CreateCountryDto, UpdateCountryDto>, ICountryCashedService
    {
        public CountryCashedService(IRepository<Country> repository, IMapper mapper, IMemoryCache cache) : base(repository, mapper, cache)
        {
        }
        public override Task<List<CountryDto>> GetAsync(Expression<Func<Country, bool>> filter = null, params Expression<Func<Country, object>>[] propertySelectors)
        {
            return base.GetAsync(null, c => c.Clients, c => c.Regions, c => c.AgentCountrs.Select(c => c.Agent));
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
            response.Data = _mapper.Map<CountryDto>(country);
            await RefreshCash();
            return response;
        }
        public override async Task<ErrorRepsonse<CountryDto>> Delete(int id)
        {
            var country = await _repository.GetById(id);
            await _repository.LoadCollection(country, c => c.Clients);
            await _repository.LoadCollection(country, c => c.AgentCountrs);
            var response = new ErrorRepsonse<CountryDto>();
            if (country.AgentCountrs.Any() || country.Clients.Any())
            {
                response.Errors.Add("Cant.Delete");
                return response;
            }
            response = await base.Delete(id);
            await RefreshCash();
            return response;
        }

    }
}
