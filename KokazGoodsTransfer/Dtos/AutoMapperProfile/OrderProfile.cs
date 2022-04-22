using AutoMapper;
using KokazGoodsTransfer.Dtos.Clients;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Dtos.OrdersTypes;
using KokazGoodsTransfer.Dtos.Regions;
using KokazGoodsTransfer.Dtos.Users;
using KokazGoodsTransfer.Models;
using System;
using System.Linq;

namespace KokazGoodsTransfer.Dtos.AutoMapperProfile
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<CreateOrderFromClient, Order>()
               .ForMember(s => s.RecipientPhones, opt => opt.MapFrom(src => String.Join(", ", src.RecipientPhones)));

            CreateMap<CreateOrdersFromEmployee, Order>()
               .ForMember(s => s.RecipientPhones, opt => opt.MapFrom(src => String.Join(", ", src.RecipientPhones)));
            CreateMap<CreateMultipleOrder, Order>();

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
            }));

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
                .ForMember(c => c.MoenyPlaced, opt => opt.MapFrom(src => src.MoenyPlaced.Name))
                .ForMember(c => c.OrderPlaced, opt => opt.MapFrom(src => src.Orderplaced.Name))
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
            CreateMap<OrderItem, ResponseOrderItemDto>()
                .ForMember(c => c.OrderTpye, opt => opt.MapFrom((orderItem, response, i, context) =>
                {
                    return context.Mapper.Map<OrderTypeDto>(orderItem.OrderTpye);
                }));

            CreateMap<ApproveAgentEditOrderRequest, ApproveAgentEditOrderRequestDto>()
                .ForMember(c => c.Order, opt => opt.MapFrom((obj, dto, i, conext) =>
                {
                    return conext.Mapper.Map<OrderDto>(obj.Order);
                }))
            .ForMember(c => c.Agent, opt => opt.MapFrom((obj, dto, i, conext) =>
            {
                return conext.Mapper.Map<UserDto>(obj.Agent);
            }))
            .ForMember(c => c.OrderPlaced, opt => opt.MapFrom((obj, dto, i, conext) =>
            {
                return conext.Mapper.Map<NameAndIdDto>(obj.OrderPlaced);
            }));
        }
    }
}
