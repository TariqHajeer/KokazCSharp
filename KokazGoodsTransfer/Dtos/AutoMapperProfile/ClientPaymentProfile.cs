using AutoMapper;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.DiscountDtos;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Dtos.ReceiptDtos;
using KokazGoodsTransfer.Models;
using System.Linq;

namespace KokazGoodsTransfer.Dtos.AutoMapperProfile
{
    public class ClientPaymentProfile : Profile
    {
        public ClientPaymentProfile()
        {
            CreateMap<ClientPaymentDetail, PrintDto>()
                .ForMember(c => c.DeliveCost, opt => opt.MapFrom(src => src.DeliveryCost))
                .ForMember(c => c.Orderplaced, opt => opt.MapFrom((order, dto, i, context) =>
                {
                    return order.OrderPlace;
                }));
            CreateMap<ClientPayment, PrintOrdersDto>()
                .ForMember(c => c.PrintNmber, opt => opt.MapFrom(src => src.Id))
                .ForMember(c => c.Receipts, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return context.Mapper.Map<ReceiptDto[]>(obj.Receipts);
                }))
                 .ForMember(c => c.Discount, opt => opt.MapFrom((obj, dto, i, context) =>
                 {
                     return context.Mapper.Map<DiscountDto>(obj.Discounts.FirstOrDefault());
                 }))
                 .ForMember(c => c.Orders, opt => opt.MapFrom((obj, dto, i, context) =>
                 {
                     return context.Mapper.Map<PrintDto[]>(obj.ClientPaymentDetails);
                 }));
        }
    }
}
