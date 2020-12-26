using AutoMapper;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Dtos.Regions;
using KokazGoodsTransfer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.Common
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            CreateMap<Region, RegionDto>()
                .ForMember(d => d.Country, src => src.MapFrom((region, regionDto, i, context) =>
                     {
                         return context.Mapper.Map<CountryDto>(region.Country);
                     })
                ).MaxDepth(1);

            CreateMap<Country, CountryDto>()
                .ForMember(c => c.CanDelete, opt => opt.MapFrom(src => src.Regions.Count() == 0 && src.Users.Count() == 0))
                .ForMember(c => c.Regions, src => src.MapFrom((country, countryDto, i, context) =>
                {
                    return context.Mapper.Map<RegionDto[]>(country.Regions);
                }))
                .MaxDepth(2);
            
        }
    }
}
