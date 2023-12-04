using AutoMapper;
using Quqaz.Web.Dtos.BranchDtos;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Models;
using System.Linq;

namespace Quqaz.Web.Dtos.AutoMapperProfile
{
    public class BranchProfile : Profile
    {
        public BranchProfile()
        {
            CreateMap<Branch, BranchDto>()
                .ForMember(c => c.CountryName, opt => opt.MapFrom(src => src.Country.Name));
            CreateMap<Branch, NameAndIdDto>();
            CreateMap<Branch, BranchPricesDto>()
                .ForMember(c => c.Prices, opt => opt.MapFrom((src, dest, i, context) =>
                {
                    return src.BranchToCountryDeliverryCosts.Select(c => new BranchPriceDto()
                    {
                        CountryName = c.Country.Name,
                        Price = c.DeliveryCost
                    });
                }));
        }
    }
}
