using AutoMapper;
using Quqaz.Web.Dtos.DiscountDtos;
using Quqaz.Web.Models;

namespace Quqaz.Web.Dtos.AutoMapperProfile
{
    public class DiscountProfile:Profile
    {
        public DiscountProfile()
        {
            CreateMap<Discount, DiscountDto>();
        }
    }
}
