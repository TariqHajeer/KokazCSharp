using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;
using Quqaz.Web.Dtos.Clients;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.Countries;
using Quqaz.Web.Dtos.OrdersDtos;
using Quqaz.Web.Dtos.OrdersTypes;
using Quqaz.Web.Dtos.Regions;
using Quqaz.Web.Dtos.Users;
using Quqaz.Web.Helpers.Extensions;
using Quqaz.Web.Models;
using Quqaz.Web.Models.Static;
using Quqaz.Web.Services.Interfaces;
using System;
using System.Linq;

namespace Quqaz.Web.Dtos.AutoMapperProfile
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<CreateOrderType, OrderType>();
            CreateMap<CreateOrderFromClient, Order>()
               .ForMember(s => s.RecipientPhones, opt => opt.MapFrom(src => String.Join(',', src.RecipientPhones)))
               .ForMember(s => s.OrderPlace, opt => opt.MapFrom(src => OrderPlace.Client))
               .ForMember(dest => dest.MoneyPlace, opt => opt.MapFrom(src => MoneyPalce.OutSideCompany))
               .ForMember(dest => dest.OrderState, opt => opt.MapFrom(src => OrderState.Processing))
               .ForMember(dest => dest.IsSend, opt => opt.MapFrom(src => false))
               ;

            CreateMap<CreateOrderFromEmployee, Order>()
               .ForMember(s => s.RecipientPhones, opt => opt.MapFrom(src => String.Join(", ", src.RecipientPhones)))
               .ForMember(c => c.MoneyPlace, opt => opt.MapFrom(src => MoneyPalce.OutSideCompany))
               .ForMember(c => c.OrderPlace, opt => opt.MapFrom(src => OrderPlace.Store));
            CreateMap<CreateMultipleOrder, Order>()
                .ForMember(c => c.Seen, opt => opt.MapFrom(src => true))
                .ForMember(c => c.MoneyPlace, opt => opt.MapFrom(src => MoneyPalce.OutSideCompany))
                .ForMember(c => c.IsClientDiliverdMoney, opt => opt.MapFrom(src => false))
                .ForMember(c => c.OrderState, opt => opt.MapFrom(src => OrderState.Processing))
                .ForMember(c => c.OrderPlace, opt => opt.MapFrom(src => OrderPlace.Store));

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
                .ForMember(c => c.DispalyMoneyPlace, opt => opt.MapFrom<DispalyMoneyPlaceValueRsolver>())
                .ForMember(c => c.DispalyOrderplaced, opt => opt.MapFrom<DisplayOrderPlaceValueResolver>())
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
                .ForMember(c => c.CurrentBrnach, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return context.Mapper.Map<NameAndIdDto>(obj.CurrentBranch);
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
                    return obj.OrderPlace;
                }))
                .ForMember(c => c.MoneyPlaced, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return obj.MoneyPalce;
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
                    return order.MoneyPalce;
                }))
                .ForMember(c => c.Orderplaced, opt => opt.MapFrom((order, dto, i, context) =>
                {
                    return order.OrderPlace;
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
                .ForMember(c => c.CanUpdateOrDelete, opt => opt.MapFrom(src => src.OrderPlace <= Quqaz.Web.Models.Static.OrderPlace.Store));
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
                return conext.Mapper.Map<NameAndIdDto>(obj.GetNewOrderPlaced());
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
                    return obj.OrderPlace;
                }))
                .ForMember(c => c.MoneyPlaced, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return obj.MoneyPalce;
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
                .ForMember(c => c.TreasuryIncome, opt => opt.MapFrom(src => src.ReceiptOfTheOrderStatusDetalis.Where(c => c.MoneyPalce == MoneyPalce.Delivered || c.MoneyPalce == MoneyPalce.InsideCompany).Sum(c => c.Cost - c.AgentCost)));
        }
        private class DispalyMoneyPlaceValueRsolver : IValueResolver<Order, OrderDto, string>
        {
            public string Resolve(Order source, OrderDto destination, string destMember, ResolutionContext context)
            {
                if (source.OrderState == OrderState.ShortageOfCash)
                    return "لديك مبلغ مع العميل";
                return source.MoneyPlace.GetDescription();
            }
        }
        private class DisplayOrderPlaceValueResolver : IValueResolver<Order, OrderDto, string>
        {
            private readonly IHttpContextAccessorService _httpContextAccessorService;

            public DisplayOrderPlaceValueResolver(IHttpContextAccessorService httpContextAccessorService)
            {
                _httpContextAccessorService = httpContextAccessorService;
            }

            public string Resolve(Order source, OrderDto destination, string destMember, ResolutionContext context)
            {
                if (source.OrderState == OrderState.ShortageOfCash)
                    return "لديك مبلغ مع العميل";
                if (source.OrderPlace == OrderPlace.Way && source.InWayToBranch)
                {
                    var currentBranch = _httpContextAccessorService.CurrentBranchId();
                    if (source.BranchId == currentBranch)
                    {
                        if (source.NextBranch != null)
                        {
                            return string.Format("في الطريق إلى فرع {0}", source.NextBranch.Name);
                        }
                        return "في الطريق إلى فرع";
                    }
                    if (source.NextBranchId == currentBranch)
                    {
                        if (source.Branch != null)
                            return string.Format("قادم من فرع {0}", source.Branch.Name);
                        return "قادم من فرع";
                    }
                }
                return source.OrderPlace.GetDescription();
            }
        }
    }
}
