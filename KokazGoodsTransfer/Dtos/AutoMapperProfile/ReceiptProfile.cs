using AutoMapper;
using KokazGoodsTransfer.Dtos.ReceiptDtos;
using KokazGoodsTransfer.Models;

namespace KokazGoodsTransfer.Dtos.AutoMapperProfile
{
    public class ReceiptProfile:Profile
    {
        public ReceiptProfile()
        {
            CreateMap<Receipt, ReceiptDto>()
                .ForMember(c => c.ClientName, opt => opt.MapFrom(c => c.Client.Name))
                .ForMember(c => c.PrintNumber, opt => opt.MapFrom(c => c.ClientPayment.Id));
        }
    }
}
