using AutoMapper;
using KokazGoodsTransfer.Dtos.OutComeDtos;
using KokazGoodsTransfer.Dtos.OutComeTypeDtos;
using KokazGoodsTransfer.Models;

namespace KokazGoodsTransfer.Dtos.AutoMapperProfile
{
    public class OutComeProfile:Profile
    {
        public OutComeProfile()
        {
            CreateMap<CreateOutComeDto, OutCome>();
            CreateMap<OutCome, OutComeDto>()
                .ForMember(c => c.CreatedBy, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(c => c.OutComeType, opt => opt.MapFrom((outcome, dto, i, context) =>
                {
                    return context.Mapper.Map<OutComeDto>(outcome);
                }));
            CreateMap<OutCome, OutComeDto>()
                .ForMember(d => d.OutComeType, opt => opt.MapFrom((outcome, dto, i, context) =>
                {
                    return context.Mapper.Map<OutComeTypeDto>(outcome.OutComeType);
                }))
                .ForMember(c => c.CreatedBy, opt => opt.MapFrom(src => src.User.Name));
            CreateMap<UpdateOuteComeDto, OutCome>();
        }
    }
}
