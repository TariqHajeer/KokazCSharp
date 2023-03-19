using AutoMapper;
using Quqaz.Web.Dtos.Clients;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.PayemntRequestDtos;
using Quqaz.Web.Models;
using System.Linq;

namespace Quqaz.Web.Dtos.AutoMapperProfile
{
    public class PaymentWayProfile:Profile
    {
        public PaymentWayProfile()
        {
            CreateMap<PaymentWay, NameAndIdDto>()
                .ForMember(c => c.CanDelete, opt => opt.MapFrom(src => src.PaymentRequests.Count() == 0));
            CreateMap<PaymentRequest, PaymentRequestDto>()
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
