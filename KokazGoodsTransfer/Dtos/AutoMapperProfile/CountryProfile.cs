using AutoMapper;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Dtos.Regions;
using KokazGoodsTransfer.Dtos.Users;
using KokazGoodsTransfer.Models;
using System.Collections.Generic;
using System.Linq;

namespace KokazGoodsTransfer.Dtos.AutoMapperProfile
{
    public class CountryProfile : Profile
    {
        public CountryProfile()
        {
            CreateMap<Country, CountryDto>()
                .ForMember(c => c.CanDelete, opt => opt.MapFrom(src => src.Regions.Count() == 0 && src.AgentCountrs.Count() == 0))
                .ForMember(c => c.CanDeleteWithRegion, opt => opt.MapFrom(src => src.AgentCountrs.Count() == 0 && src.Clients.Count() == 0))
                .ForMember(c => c.Mediator, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return context.Mapper.Map<CountryDto>(obj.Mediator);
                }))
                .ForMember(c => c.Regions, src => src.MapFrom((country, countryDto, i, context) =>
                {
                    if (country.Regions == null)
                        return null;
                    country.Regions.ToList().ForEach(c => c.Country = null);
                    return context.Mapper.Map<RegionDto[]>(country.Regions);
                }))
                .ForMember(c => c.Agnets, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    if (obj.AgentCountrs == null)
                        return null;
                    obj.AgentCountrs.ToList().ForEach(c => c.Agent.AgentCountrs = null);
                    return context.Mapper.Map<UserDto[]>(obj.AgentCountrs.Select(c => c.Agent));
                })).MaxDepth(2);
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
                }))
                .ForMember(c => c.IsMain, opt => opt.MapFrom(src => false));

        }
    }
}
