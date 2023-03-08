using AutoMapper;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Dtos.Regions;
using KokazGoodsTransfer.Dtos.Users;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KokazGoodsTransfer.Dtos.AutoMapperProfile
{
    public class CountryProfile : Profile
    {
        public CountryProfile()
        {
            CreateMap<Country, CountryDto>()
                .ForMember(c => c.CanDelete, opt => opt.MapFrom(src => src.Regions.Count() == 0 && src.AgentCountries.Count() == 0))
                .ForMember(c => c.CanDeleteWithRegion, opt => opt.MapFrom(src => src.AgentCountries.Count() == 0 && src.Clients.Count() == 0))
                .ForMember(c => c.Regions, src => src.MapFrom((country, countryDto, i, context) =>
                {
                    if (country.Regions == null)
                        return null;
                    country.Regions.ToList().ForEach(c => c.Country = null);
                    return context.Mapper.Map<NameAndIdDto[]>(country.Regions);
                }))
                .ForMember(c => c.Points, opt => opt.MapFrom<PointsValueResolver>())
                .ForMember(c => c.DeliveryCost, opt => opt.MapFrom<DeliveryCostValueResolver>())
                .ForMember(c => c.Agnets, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    if (obj.AgentCountries == null)
                        return null;
                    obj.AgentCountries.ToList().ForEach(c => c.Agent.AgentCountries = null);
                    return context.Mapper.Map<NameAndIdDto[]>(obj.AgentCountries.Select(c => c.Agent));
                })).MaxDepth(2)
                .ForMember(c => c.RequiredAgent, opt => opt.MapFrom<RequiredAgentValueResolver>());
            CreateMap<Country, NameAndIdDto>();
            CreateMap<UpdateCountryDto, Country>();
            CreateMap<CreateCountryDto, Country>()
                .ForMember(c => c.Regions, opt => opt.MapFrom((dto, obj, i, context) =>
                {

                    List<Region> regions = new List<Region>();
                    if (dto.Regions != null)
                        foreach (var item in dto.Regions)
                        {
                            regions.Add(new Region()
                            {
                                Name = item
                            });
                        }
                    return regions;
                }));

        }
        public class PointsValueResolver : IValueResolver<Country, CountryDto, Int16>
        {
            public IHttpContextAccessorService _httpContextAccessorService { get; set; }

            public PointsValueResolver(IHttpContextAccessorService httpContextAccessorService)
            {
                _httpContextAccessorService = httpContextAccessorService;
            }

            public short Resolve(Country source, CountryDto destination, short destMember, ResolutionContext context)
            {
                var currentBranchId = _httpContextAccessorService.CurrentBranchId();

                return source.BranchToCountryDeliverryCosts.First(c => c.BranchId == currentBranchId).Points;
            }
        }
        public class DeliveryCostValueResolver : IValueResolver<Country, CountryDto, decimal>
        {
            public IHttpContextAccessorService _httpContextAccessorService { get; set; }
            public DeliveryCostValueResolver(IHttpContextAccessorService httpContextAccessorService)
            {
                _httpContextAccessorService = httpContextAccessorService;
            }
            public decimal Resolve(Country source, CountryDto destination, decimal destMember, ResolutionContext context)
            {
                var currentBranchId = _httpContextAccessorService.CurrentBranchId();
                return source.BranchToCountryDeliverryCosts.First(c => c.BranchId == currentBranchId).DeliveryCost;
            }
        }
        public class RequiredAgentValueResolver : IValueResolver<Country, CountryDto, bool>
        {
            public IHttpContextAccessorService _httpContextAccessorService { get; set; }
            public RequiredAgentValueResolver(IHttpContextAccessorService httpContextAccessorService)
            {
                _httpContextAccessorService = httpContextAccessorService;
            }
            public bool Resolve(Country source, CountryDto destination, bool destMember, ResolutionContext context)
            {
                var currentCountyId = _httpContextAccessorService.CurrentBranchId();
                if (source.Id == _httpContextAccessorService.CurrentBranchId())
                    return true;
                if (source.Branch != null)
                    return false;
                if (source.ToCountries == null)
                    return false;

                return !source.ToCountries.Select(c => c.FromCountryId).Contains(currentCountyId);
            }
        }
    }


}
