using AutoMapper;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Dtos.Regions;
using KokazGoodsTransfer.Models;

namespace KokazGoodsTransfer.Dtos.AutoMapperProfile
{
    public class RegionProfile:Profile
    {
        public RegionProfile()
        {
            CreateMap<Region, RegionDto>()
                .ForMember(d => d.Country, src => src.MapFrom((region, regionDto, i, context) =>
                {

                    if (region.Country != null)
                    {
                        region.Country.Regions = null;
                    }
                    return context.Mapper.Map<CountryDto>(region.Country);
                })
                ).MaxDepth(1);
            CreateMap<CreateRegionDto, Region>();
            CreateMap<UpdateRegionDto, Region>();
            CreateMap<Region, NameAndIdDto>();
        }
    }
}
