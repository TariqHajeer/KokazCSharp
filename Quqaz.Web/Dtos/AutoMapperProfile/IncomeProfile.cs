using AutoMapper;
using Quqaz.Web.Dtos.IncomesDtos;
using Quqaz.Web.Dtos.IncomeTypes;
using Quqaz.Web.Models;

namespace Quqaz.Web.Dtos.AutoMapperProfile
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
