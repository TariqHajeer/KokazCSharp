using AutoMapper;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.NotifcationDtos;
using KokazGoodsTransfer.Models;

namespace KokazGoodsTransfer.Dtos.AutoMapperProfile
{
    public class NotificationProfile:Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notfication, NotificationDto>()
                .ForMember(c => c.MoneyPlaced, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return context.Mapper.Map<NameAndIdDto>(obj.MoneyPlaced);
                }))
                .ForMember(c => c.OrderPlaced, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return context.Mapper.Map<NameAndIdDto>(obj.OrderPlaced);
                }));
        }
    }
}
