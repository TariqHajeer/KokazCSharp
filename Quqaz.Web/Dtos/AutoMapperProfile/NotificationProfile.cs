using AutoMapper;
using Quqaz.Web.Dtos.NotifcationDtos;
using Quqaz.Web.Models;

namespace Quqaz.Web.Dtos.AutoMapperProfile
{
    public class NotificationProfile:Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notfication, NotificationDto>()
                .ForMember(c => c.MoneyPlaced, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return obj.MoneyPlace;
                }))
                .ForMember(c => c.OrderPlaced, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return obj.OrderPlace;
                }));
        }
    }
}
