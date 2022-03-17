using AutoMapper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.IncomeTypes;
using KokazGoodsTransfer.Models;
using System.Linq;

namespace KokazGoodsTransfer.Dtos.AutoMapperProfile
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
