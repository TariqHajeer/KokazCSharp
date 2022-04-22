using AutoMapper;
using KokazGoodsTransfer.Dtos.Clients;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Dtos.DiscountDtos;
using KokazGoodsTransfer.Dtos.EditRequestDtos;
using KokazGoodsTransfer.Dtos.NotifcationDtos;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Dtos.PointSettingsDtos;
using KokazGoodsTransfer.Dtos.ReceiptDtos;
using KokazGoodsTransfer.Dtos.Regions;
using KokazGoodsTransfer.Dtos.Users;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Infrastrcuter;
using KokazGoodsTransfer.Models.Static;
using System.Linq;

namespace KokazGoodsTransfer.Dtos.Common
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {


            CreateMap<IIndex, NameAndIdDto>();


            CreateMap<Order, PayForClientDto>()
                .ForMember(c => c.Region, opt => opt.MapFrom((order, dto, i, context) =>
                {
                    return context.Mapper.Map<RegionDto>(order.Region);
                }))
                .ForMember(c => c.Country, opt => opt.MapFrom((order, dto, i, context) =>
                {
                    return context.Mapper.Map<CountryDto>(order.Country);
                }))
                .ForMember(c => c.AgentPrintNumber, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return obj.AgentOrderPrints.LastOrDefault()?.AgentPrint?.Id ?? null;
                }))
                .ForMember(c => c.ClientPrintNumber, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return obj.OrderClientPaymnets.LastOrDefault()?.ClientPayment?.Id ?? null;
                }))
                .ForMember(c => c.MonePlaced, opt => opt.MapFrom((order, dto, i, context) =>
                {
                    return context.Mapper.Map<NameAndIdDto>(order.MoenyPlaced);
                }))
                .ForMember(c => c.Orderplaced, opt => opt.MapFrom((order, dto, i, context) =>
                {
                    return context.Mapper.Map<NameAndIdDto>(order.Orderplaced);
                }))
                .ForMember(c => c.Agent, opt => opt.MapFrom((order, dto, i, context) =>
                {
                    return context.Mapper.Map<UserDto>(order.Agent);
                }))
                .ForMember(c => c.PayForClient, opt => opt.MapFrom((order, dto, i, context) =>
                {
                    var shoudToPay = order.ShouldToPay();
                    return shoudToPay - (order.ClientPaied ?? 0);
                }));
            
            CreateMap<AgentPrint, PrintOrdersDto>()
                .ForMember(c => c.PrintNmber, opt => opt.MapFrom(c => c.Id))
                .ForMember(c => c.Receipts, opt => opt.Ignore())
                .ForMember(c => c.Discount, opt => opt.Ignore())
                .ForMember(c => c.Orders, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return context.Mapper.Map<PrintDto[]>(obj.AgentPrintDetails);
                }));

            //CreateMap<Printed, PrintOrdersDto>()
            //    .ForMember(src => src.Orders, opt => opt.MapFrom((obj, dto, i, context) =>
            //         {
            //             List<PrintDto> x = new List<PrintDto>();
            //             x.AddRange(context.Mapper.Map<PrintDto[]>(obj.ClientPrints));
            //             return x;
            //         }))
            //    .ForMember(c => c.Receipts, opt => opt.MapFrom((obj, dto, i, context) =>
            //    {
            //        return context.Mapper.Map<ReceiptDto[]>(obj.Receipts);
            //    }))
            //    .ForMember(c => c.Discount, opt => opt.MapFrom((obj, dto, i, context) =>
            //    {
            //        return context.Mapper.Map<DiscountDto>(obj.Discounts.FirstOrDefault());
            //    }));
            CreateMap<AgentPrintDetail, PrintDto>();
            CreateMap<Discount, DiscountDto>();
            CreateMap<ClientPaymentDetail, PrintDto>()
                .ForMember(c => c.Orderplaced, opt => opt.MapFrom((order, dto, i, context) =>
                {
                    return context.Mapper.Map<NameAndIdDto>(order.OrderPlaced);
                }));

            CreateMap<Receipt, ReceiptDto>()
                .ForMember(c => c.ClientName, opt => opt.MapFrom(c => c.Client.Name))
                .ForMember(c => c.PrintNumber, opt => opt.MapFrom(c => c.ClientPayment.Id));

            

        }
    }
}
