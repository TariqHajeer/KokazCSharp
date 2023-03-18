using AutoMapper;
using KokazGoodsTransfer.Dtos.Clients;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Dtos.OrdersTypes;
using KokazGoodsTransfer.Dtos.Regions;
using KokazGoodsTransfer.Dtos.Users;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Static;
using System;
using System.Linq;

namespace KokazGoodsTransfer.Dtos.AutoMapperProfile
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<CreateOrderType, OrderType>();
            CreateMap<CreateOrderFromClient, Order>()
               .ForMember(s => s.RecipientPhones, opt => opt.MapFrom(src => String.Join(',', src.RecipientPhones)))
               .ForMember(s => s.OrderplacedId, opt => opt.MapFrom(src => (int)OrderplacedEnum.Client))
               .ForMember(dest => dest.MoenyPlacedId, opt => opt.MapFrom(src => (int)MoneyPalcedEnum.OutSideCompany))
               .ForMember(dest => dest.OrderStateId, opt => opt.MapFrom(src => (int)OrderStateEnum.Processing))
               .ForMember(dest => dest.IsSend, opt => opt.MapFrom(src => false))
               ;

            CreateMap<CreateOrderFromEmployee, Order>()
               .ForMember(s => s.RecipientPhones, opt => opt.MapFrom(src => String.Join(", ", src.RecipientPhones)))
               .ForMember(c => c.MoenyPlacedId, opt => opt.MapFrom(src => (int)MoneyPalcedEnum.OutSideCompany))
               .ForMember(c => c.OrderplacedId, opt => opt.MapFrom(src => (int)OrderplacedEnum.Store));
            CreateMap<CreateMultipleOrder, Order>()
                .ForMember(c => c.Seen, opt => opt.MapFrom(src => true))
                .ForMember(c => c.MoenyPlacedId, opt => opt.MapFrom(src => (int)MoneyPalcedEnum.OutSideCompany))
                .ForMember(c => c.IsClientDiliverdMoney, opt => opt.MapFrom(src => false))
                .ForMember(c => c.OrderStateId, opt => opt.MapFrom(src => (int)OrderStateEnum.Processing))
                .ForMember(c => c.OrderplacedId, opt => opt.MapFrom(src => (int)OrderplacedEnum.Store));

            CreateMap<DisAcceptOrder, OrderDto>()
                .ForMember(c => c.Region, opt => opt.MapFrom((order, dto, i, context) =>
                {
                    return context.Mapper.Map<RegionDto>(order.Region);
                }))
                .ForMember(c => c.Country, opt => opt.MapFrom((order, dto, i, context) =>
                {
                    return context.Mapper.Map<CountryDto>(order.Country);
                }))
                .ForMember(c => c.Client, opt => opt.MapFrom((order, dto, i, context) =>
                {
                    return context.Mapper.Map<ClientDto>(order.Client);
                }));

            CreateMap<Order, OrderDto>()
                .ForMember(c => c.Region, opt => opt.MapFrom((order, dto, i, context) =>
                {
                    return context.Mapper.Map<RegionDto>(order.Region);
                }))
                .ForMember(c => c.Country, opt => opt.MapFrom((order, dto, i, context) =>
                {
                    return context.Mapper.Map<NameAndIdDto>(order.Country);
                }))
                .ForMember(c => c.Client, opt => opt.MapFrom((order, dto, i, context) =>
                {
                    return context.Mapper.Map<ClientDto>(order.Client);
                }))
                .ForMember(c => c.MonePlaced, opt => opt.MapFrom((order, dto, i, context) =>
                {
                    return context.Mapper.Map<NameAndIdDto>(order.GetMoneyPlaced());
                }))
                .ForMember(c => c.Orderplaced, opt => opt.MapFrom((order, dto, i, context) =>
                {
                    return context.Mapper.Map<NameAndIdDto>(order.GetOrderPlaced());
                }))
                .ForMember(c => c.Agent, opt => opt.MapFrom((order, dto, i, context) =>
                {
                    return context.Mapper.Map<UserDto>(order.Agent);
                }))
                .ForMember(c => c.OrderItems, opt => opt.MapFrom((order, response, i, context) =>
                {
                    return context.Mapper.Map<ResponseOrderItemDto[]>(order.OrderItems);
                }))
                .ForMember(c => c.AgentPrintNumber, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return obj.AgentOrderPrints.LastOrDefault()?.AgentPrint?.Id ?? null;
                }))
                .ForMember(c => c.ClientPrintNumber, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return obj.OrderClientPaymnets.LastOrDefault()?.ClientPayment?.Id ?? null;
                }))
                .ForMember(c => c.OrderLogs, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return context.Mapper.Map<OrderLogDto[]>(obj.OrderLogs);
                }))
                .ForMember(c => c.AgentPrint, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return context.Mapper.Map<PrintOrdersDto[]>(obj.AgentOrderPrints.Select(c => c.AgentPrint)).ToList();
                }))
            .ForMember(c => c.ClientPrint, opt => opt.MapFrom((obj, dto, i, context) =>
            {
                return context.Mapper.Map<PrintOrdersDto[]>(obj.OrderClientPaymnets.Select(c => c.ClientPayment));
            }))
            .ForMember(c => c.ReceiptOfTheOrderStatusDetalis, opt => opt.MapFrom((src, dest, i, context) =>
            {
                return context.Mapper.Map<ReceiptOfTheOrderStatusDetaliOrderDto[]>(src.ReceiptOfTheOrderStatusDetalis);
            }))
            .ForMember(c => c.BranchName, opt => opt.MapFrom(src => src.Branch.Name));
            CreateMap<ReceiptOfTheOrderStatusDetali, ReceiptOfTheOrderStatusDetaliOrderDto>()
                .ForMember(c => c.OrderPlaced, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return context.Mapper.Map<NameAndIdDto>(obj.OrderPlaced);
                }))
                .ForMember(c => c.MoneyPlaced, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return context.Mapper.Map<NameAndIdDto>(obj.MoneyPlaced);
                }))
                .ForMember(c => c.Reciver, opt => opt.MapFrom(src => src.ReceiptOfTheOrderStatus.Recvier.Name))
                .ForMember(c => c.CreatedOn, opt => opt.MapFrom(src => src.ReceiptOfTheOrderStatus.CreatedOn));
            CreateMap<OrderLog, OrderLogDto>()
                .ForMember(c => c.Region, opt => opt.MapFrom((order, dto, i, context) =>
                {
                    return context.Mapper.Map<RegionDto>(order.Region);
                }))
                .ForMember(c => c.Country, opt => opt.MapFrom((order, dto, i, context) =>
                {
                    return context.Mapper.Map<CountryDto>(order.Country);
                }))
                .ForMember(c => c.Client, opt => opt.MapFrom((order, dto, i, context) =>
                {
                    return context.Mapper.Map<ClientDto>(order.Client);
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
                }));

            CreateMap<Order, OrderResponseClientDto>()
                .ForMember(c => c.MoenyPlaced, opt => opt.MapFrom(src => src.GetMoneyPlaced().Name))
                .ForMember(c => c.OrderPlaced, opt => opt.MapFrom(src => src.GetOrderPlaced().Name))
                .ForMember(c => c.RecipientPhones, opt => opt.MapFrom(src => src.RecipientPhones.Split(",", StringSplitOptions.None)))
                .ForMember(c => c.Region, opt => opt.MapFrom((order, response, i, context) =>
                {
                    return context.Mapper.Map<RegionDto>(order.Region);
                }))
                .ForMember(c => c.Country, opt => opt.MapFrom((order, response, i, context) =>
                {
                    return context.Mapper.Map<CountryDto>(order.Country);
                }))
                .ForMember(c => c.CanUpdateOrDelete, opt => opt.MapFrom(src => src.OrderplacedId <= 2));
            CreateMap<OrderType, OrderTypeDto>()
                .ForMember(c => c.CanDelete, opt => opt.MapFrom(src => src.OrderItems.Count() == 0));
            CreateMap<OrderType, NameAndIdDto>();
            CreateMap<OrderItem, ResponseOrderItemDto>()
                .ForMember(c => c.OrderTpye, opt => opt.MapFrom((orderItem, response, i, context) =>
                {
                    return context.Mapper.Map<OrderTypeDto>(orderItem.OrderTpye);
                }));

            CreateMap<Order, ApproveAgentEditOrderRequestDto>()
                .ForMember(c => c.Order, opt => opt.MapFrom((obj, dto, i, conext) =>
                {
                    return conext.Mapper.Map<OrderDto>(obj);
                }))
            .ForMember(c => c.Agent, opt => opt.MapFrom((obj, dto, i, conext) =>
            {
                return conext.Mapper.Map<UserDto>(obj.Agent);
            }))
            .ForMember(c => c.OrderPlaced, opt => opt.MapFrom((obj, dto, i, conext) =>
            {
                return conext.Mapper.Map<NameAndIdDto>(obj.NewOrderPlaced);
            }));
            CreateMap<Order, PayForClientDto>()
                .ForMember(c => c.Region, opt => opt.MapFrom((order, dto, i, context) =>
                {
                    return context.Mapper.Map<RegionDto>(order.Region);
                }))
                .ForMember(c => c.Country, opt => opt.MapFrom((order, dto, i, context) =>
                {
                    return context.Mapper.Map<NameAndIdDto>(order.Country);
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
                    return context.Mapper.Map<NameAndIdDto>(order.GetMoneyPlaced());
                }))
                .ForMember(c => c.Orderplaced, opt => opt.MapFrom((order, dto, i, context) =>
                {
                    return context.Mapper.Map<NameAndIdDto>(order.GetOrderPlaced());
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

            CreateMap<ReceiptOfTheOrderStatusDetali, ReceiptOfTheOrderStatusDetaliDto>()
                .ForMember(c => c.Agent, opt => opt.MapFrom((obj, dto, i, context) =>
               {
                   return context.Mapper.Map<NameAndIdDto>(obj.Agent);
               }))
                .ForMember(c => c.OrderPlaced, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return context.Mapper.Map<NameAndIdDto>(obj.OrderPlaced);
                }))
                .ForMember(c => c.MoneyPlaced, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return context.Mapper.Map<NameAndIdDto>(obj.MoneyPlaced);
                }))
                .ForMember(c => c.Client, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return context.Mapper.Map<NameAndIdDto>(obj.Client);
                }));

            CreateMap<ReceiptOfTheOrderStatus, ReceiptOfTheOrderStatusDto>()
                .ForMember(c => c.ReciverName, opt => opt.MapFrom(src => src.Recvier.Name))
                .ForMember(c => c.ReceiptOfTheOrderStatusDetalis, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return context.Mapper.Map<ReceiptOfTheOrderStatusDetaliDto[]>(obj.ReceiptOfTheOrderStatusDetalis);
                }))
                .ForMember(c => c.OrderTotal, opt => opt.MapFrom(src => src.ReceiptOfTheOrderStatusDetalis.Sum(c => c.Cost)))
                .ForMember(c => c.AgentTotal, opt => opt.MapFrom(src => src.ReceiptOfTheOrderStatusDetalis.Sum(c => c.AgentCost)))
                .ForMember(c => c.TreasuryIncome, opt => opt.MapFrom(src => src.ReceiptOfTheOrderStatusDetalis.Where(c => c.MoneyPlacedId == (int)MoneyPalcedEnum.Delivered || c.MoneyPlacedId == (int)MoneyPalcedEnum.InsideCompany).Sum(c => c.Cost - c.AgentCost)));
        }
    }
}
