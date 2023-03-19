using AutoMapper;
using Quqaz.Web.Dtos.BranchDtos;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Models;

namespace Quqaz.Web.Dtos.AutoMapperProfile
{
    public class BranchProfile : Profile
    {
        public BranchProfile()
        {
            CreateMap<Branch, BranchDto>()
                .ForMember(c => c.CountryName, opt => opt.MapFrom(src => src.Country.Name));
            CreateMap<Branch, NameAndIdDto>();
        }
    }
}
