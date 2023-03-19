using AutoMapper;
using Quqaz.Web.Dtos.OutComeTypeDtos;
using Quqaz.Web.Models;
using System.Linq;
namespace Quqaz.Web.Dtos.AutoMapperProfile
{
    public class OutcomeTypeProfile : Profile
    {
        public OutcomeTypeProfile()
        {
            CreateMap<OutComeType, OutComeTypeDto>()
                .ForMember(d => d.CanDelete, opt => opt.MapFrom(src => src.OutComes.Count() == 0));
            CreateMap<CreateOutComeTypeDto, OutComeType>();
            CreateMap<UpdateOutComeTypeDto, OutComeType>();
        }
    }
}
