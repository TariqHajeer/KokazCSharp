using AutoMapper;
using KokazGoodsTransfer.Dtos.OutComeTypeDtos;
using KokazGoodsTransfer.Models;
using System.Linq;
namespace KokazGoodsTransfer.Dtos.AutoMapperProfile
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
