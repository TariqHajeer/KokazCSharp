using AutoMapper;
using Quqaz.Web.DAL.Infrastructure.Interfaces;
using Quqaz.Web.Dtos.IncomeTypes;
using Quqaz.Web.Models;
using System.Linq;

namespace Quqaz.Web.Dtos.AutoMapperProfile
{
    public class IncomeTypeProfile : Profile
    {
        public IncomeTypeProfile()
        {
            CreateMap<IncomeType, IncomeTypeDto>()
                .ForMember(c => c.CanDelete, opt => opt.MapFrom(src => src.Incomes.Count() == 0));
            CreateMap<CreateIncomeTypeDto, IncomeType>();
            CreateMap<UpdateIncomeTypeDto, IncomeType>();
        }
    }
}
