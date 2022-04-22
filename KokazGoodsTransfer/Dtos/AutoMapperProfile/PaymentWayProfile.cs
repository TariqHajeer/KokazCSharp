using AutoMapper;
using KokazGoodsTransfer.Dtos.Clients;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.PayemntRequestDtos;
using KokazGoodsTransfer.Models;

namespace KokazGoodsTransfer.Dtos.AutoMapperProfile
{
    public class PaymentWayProfile:Profile
    {
        public PaymentWayProfile()
        {
            CreateMap<PaymentWay, NameAndIdDto>()
                .ForMember(c => c.CanDelete, opt => opt.MapFrom(src => src.PaymentRequests.Count() == 0));
            CreateMap<PaymentRequest, PayemntRquestDto>()
                .ForMember(c => c.Client, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return context.Mapper.Map<ClientDto>(obj.Client);
                }))
                .ForMember(c => c.PaymentWay, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return context.Mapper.Map<NameAndIdDto>(obj.PaymentWay);
                }));
        }
    }
}
