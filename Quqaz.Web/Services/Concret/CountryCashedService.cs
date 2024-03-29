﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Quqaz.Web.DAL.Infrastructure.Interfaces;
using Quqaz.Web.Dtos.Countries;
using Quqaz.Web.Helpers;
using Quqaz.Web.Models;
using Quqaz.Web.Services.Helper;
using Quqaz.Web.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Quqaz.Web.Services.Concret
{
    public class CountryCashedService : CashService<Country, CountryDto, CreateCountryDto, UpdateCountryDto>, ICountryCashedService
    {
        private readonly IRepository<MediatorCountry> _mediatorCountryRepo;
        public CountryCashedService(IRepository<Country> repository, IMapper mapper, IMemoryCache cache, Logging logging, IHttpContextAccessorService httpContextAccessorService, IRepository<MediatorCountry> mediatorCountryRepo)
            : base(repository, mapper, cache, logging, httpContextAccessorService)
        {
            _mediatorCountryRepo = mediatorCountryRepo;
        }
        public async Task<List<CountryDto>> GetCountriesFromBrachToCurrentBranch(int brnachId)
        {
            var mddileCountris = await _mediatorCountryRepo.GetAsync(c => c.FromCountryId == brnachId && c.MediatorCountryId == _currentBranch);
            var countriesId = mddileCountris.Select(c => c.ToCountryId).ToList();
            countriesId.Add(_currentBranch);
            var countreis = await _repository.GetAsync(c => countriesId.Contains(c.Id), c => c.AgentCountries.Select(c => c.Agent));
            return _mapper.Map<List<CountryDto>>(countreis);
        }
        public override async Task<CountryDto> GetById(int id)
        {
            var list = await GetCashed();
            return list.First(c => c.Id == id);
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

            await _repository.AddAsync(country);
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
                entites = await GetAsync(null, c => c.Regions, c => c.Branch, c => c.ToCountries, c => c.BranchToCountryDeliverryCosts, c => c.AgentCountries.Select(a => a.Agent));
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
            var country = await _repository.GetById(updateDto.Id);
            await _repository.LoadCollection(country, c => c.BranchToCountryDeliverryCosts);
            var branchToCountryDeliverryCosts = country.BranchToCountryDeliverryCosts.First(c => c.BranchId == _currentBranch);

            branchToCountryDeliverryCosts.DeliveryCost = updateDto.DeliveryCost;
            branchToCountryDeliverryCosts.Points = updateDto.Points;
            country.Name = updateDto.Name;
            await _repository.Update(country);
            response.Data = _mapper.Map<CountryDto>(country);
            RemoveCash();
            return response;

        }
    }
}
