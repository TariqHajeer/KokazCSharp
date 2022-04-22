using AutoMapper;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Models;

namespace KokazGoodsTransfer.Dtos.AutoMapperProfile
{
    public class AgentPrintProfile:Profile
    {
        public AgentPrintProfile()
        {
            CreateMap<AgentPrint, PrintOrdersDto>()
                .ForMember(c => c.PrintNmber, opt => opt.MapFrom(c => c.Id))
                .ForMember(c => c.Receipts, opt => opt.Ignore())
                .ForMember(c => c.Discount, opt => opt.Ignore())
                .ForMember(c => c.Orders, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return context.Mapper.Map<PrintDto[]>(obj.AgentPrintDetails);
                }));


            CreateMap<AgentPrintDetail, PrintDto>();
        }
    }
}
