using AutoMapper;
using KokazGoodsTransfer.Dtos.IncomesDtos;
using KokazGoodsTransfer.Dtos.IncomeTypes;
using KokazGoodsTransfer.Models;

namespace KokazGoodsTransfer.Dtos.AutoMapperProfile
{
    public class IncomeProfile:Profile
    {
        public IncomeProfile()
        {
            CreateMap<CreateIncomeDto, Income>();
            CreateMap<UpdateIncomeDto, Income>();
            CreateMap<Income, IncomeDto>()
                .ForMember(c => c.IncomeType, opt => opt.MapFrom((income, incomeDto, i, context) =>
                {
                    return context.Mapper.Map<IncomeTypeDto>(income.IncomeType);
                }))
                .ForMember(c => c.CreatedBy, opt => opt.MapFrom(src => src.User.Name));
        }
    }
}
