using AutoMapper;
using KokazGoodsTransfer.Dtos.DiscountDtos;
using KokazGoodsTransfer.Models;

namespace KokazGoodsTransfer.Dtos.AutoMapperProfile
{
    public class DiscountProfile:Profile
    {
        public DiscountProfile()
        {
            CreateMap<Discount, DiscountDto>();
        }
    }
}
