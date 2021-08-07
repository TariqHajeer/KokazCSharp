using AutoMapper;
using KokazGoodsTransfer.Dtos.Clients;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Dtos.Currencies;
using KokazGoodsTransfer.Dtos.DepartmentDtos;
using KokazGoodsTransfer.Dtos.EditRequestDtos;
using KokazGoodsTransfer.Dtos.IncomesDtos;
using KokazGoodsTransfer.Dtos.IncomeTypes;
using KokazGoodsTransfer.Dtos.NotifcationDtos;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Dtos.OrdersTypes;
using KokazGoodsTransfer.Dtos.OutComeDtos;
using KokazGoodsTransfer.Dtos.OutComeTypeDtos;
using KokazGoodsTransfer.Dtos.PayemntRequestDtos;
using KokazGoodsTransfer.Dtos.ReceiptDtos;
using KokazGoodsTransfer.Dtos.Regions;
using KokazGoodsTransfer.Dtos.Users;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.Common
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {

            CreateMap<Region, RegionDto>()
                .ForMember(d => d.Country, src => src.MapFrom((region, regionDto, i, context) =>
                     {
                         return context.Mapper.Map<CountryDto>(region.Country);
                     })
                ).MaxDepth(1);
            CreateMap<IncomeType, IncomeTypeDto>()
                .ForMember(c => c.CanDelete, opt => opt.MapFrom(src => src.Incomes.Count() == 0));

            CreateMap<Country, CountryDto>()
                .ForMember(c => c.CanDelete, opt => opt.MapFrom(src => src.Regions.Count() == 0 && src.AgentCountrs.Count() == 0))
                .ForMember(c => c.CanDeleteWithRegion, opt => opt.MapFrom(src => src.AgentCountrs.Count() == 0 && src.Clients.Count() == 0))
                .ForMember(c => c.Mediator, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return context.Mapper.Map<CountryDto>(obj.Mediator);
                }))
                .ForMember(c => c.Regions, src => src.MapFrom((country, countryDto, i, context) =>
                {
                    return context.Mapper.Map<RegionDto[]>(country.Regions);
                }))
                .MaxDepth(2)
                .ForMember(c => c.Agnets, opt => opt.MapFrom((obj, dto, i, context) =>
                     {
                         return context.Mapper.Map<UserDto[]>(obj.AgentCountrs.Select(c => c.Agent));
                     }));
            CreateMap<User, UserDto>()
                .ForMember(d => d.Phones, opt => opt.MapFrom((user, dto, i, context) =>
                {
                    return context.Mapper.Map<PhoneDto[]>(user.UserPhones);
                }))
                .ForMember(c => c.GroupsId, opt => opt.MapFrom(src => src.UserGroups.Select(c => c.GroupId)))
                .ForMember(c => c.Countries, opt => opt.MapFrom((user, dto, i, context) =>
                  {
                      return context.Mapper.Map<CountryDto[]>(user.AgentCountrs.Select(c => c.Country));
                  }));


            CreateMap<CreateClientDto, Client>()
                .ForMember(c => c.Password, opt => opt.MapFrom(src => MD5Hash.GetMd5Hash(src.Password)));
            CreateMap<Privilege, UserPrivilegeDto>();
            CreateMap<User, AuthenticatedUserDto>()
                .ForMember(c => c.Privileges, opt => opt.MapFrom((user, authDto, i, context) =>
                     {
                         return context.Mapper.Map<UserPrivilegeDto[]>(user.UserGroups.SelectMany(c => c.Group.GroupPrivileges.Select(c => c.Privileg).Distinct()));
                     }));
            CreateMap<Client, ClientDto>()
                .ForMember(c => c.CreatedBy, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(c => c.Total, opt => opt.MapFrom(src => src.Receipts.Where(c => c.PrintId == null).Sum(c => c.Amount)))
                .ForMember(d => d.Country, opt => opt.MapFrom((client, clientDto, i, context) =>
                {
                    return context.Mapper.Map<CountryDto>(client.Country);
                }))
                .ForMember(d => d.Phones, opt => opt.MapFrom((client, clientDto, i, context) =>
                     {
                         return context.Mapper.Map<PhoneDto[]>(client.ClientPhones);
                     }));
            CreateMap<ClientPhone, PhoneDto>();
            CreateMap<UserPhone, PhoneDto>();
            CreateMap<Client, AuthClient>()
                .ForMember(d => d.Country, opt => opt.MapFrom((client, authclient, i, context) =>
                {
                    return context.Mapper.Map<Country>(client.Country);
                }))
                .ForMember(d => d.Phones, opt => opt.MapFrom((client, auth, i, context) =>
                {
                    return context.Mapper.Map<PhoneDto[]>(client.ClientPhones);
                }));
            CreateMap<CreateOutComeDto, OutCome>();
            CreateMap<OutCome, OutComeDto>()
                .ForMember(c => c.CreatedBy, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(c => c.OutComeType, opt => opt.MapFrom((outcome, dto, i, context) =>
                {

                    return context.Mapper.Map<OutComeDto>(outcome);
                }));
            CreateMap<UpdateClientDto, Client>()
                .ForMember(c => c.Password, opt => opt.MapFrom(src => MD5Hash.GetMd5Hash(src.Password)));
            CreateMap<CUpdateClientDto, Client>()
                .ForMember(c => c.Password, opt => opt.MapFrom(src => src.Password == null ? "" : MD5Hash.GetMd5Hash(src.Password)));
            CreateMap<CreateIncomeDto, Income>();
            CreateMap<UpdateIncomeDto, Income>();
            CreateMap<Income, IncomeDto>()
                .ForMember(c => c.IncomeType, opt => opt.MapFrom((income, incomeDto, i, context) =>
                {
                    return context.Mapper.Map<IncomeTypeDto>(income.IncomeType);
                }))
                .ForMember(c => c.CreatedBy, opt => opt.MapFrom(src => src.User.Name));
            //CreateMap<Currency, CurrencyDto>()
            //.ForMember(c => c.CanDelete, opt => opt.MapFrom(src => src.Incomes.Count() == 0 && src.OutComes.Count() == 0));
            CreateMap<OutComeType, OutComeTypeDto>()
                .ForMember(d => d.CanDelete, opt => opt.MapFrom(src => src.OutComes.Count() == 0));

            CreateMap<OutCome, OutComeDto>()
                .ForMember(d => d.OutComeType, opt => opt.MapFrom((outcome, dto, i, context) =>
                {
                    return context.Mapper.Map<OutComeTypeDto>(outcome.OutComeType);
                }))
                .ForMember(c => c.CreatedBy, opt => opt.MapFrom(src => src.User.Name));
            CreateMap<UpdateOuteComeDto, OutCome>();
            CreateMap<OrderPlaced, NameAndIdDto>();
            CreateMap<MoenyPlaced, NameAndIdDto>();
            CreateMap<CreateOrderFromClient, Order>()
                .ForMember(s => s.RecipientPhones, opt => opt.MapFrom(src => String.Join(", ", src.RecipientPhones)));
            CreateMap<CreateOrdersFromEmployee, Order>()
                .ForMember(s => s.RecipientPhones, opt => opt.MapFrom(src => String.Join(", ", src.RecipientPhones)));
            CreateMap<CreateMultipleOrder, Order>();
            CreateMap<OrderType, NameAndIdDto>();
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
                    return obj.OrderPrints.Where(c => c.Print.Type == PrintType.Agent).LastOrDefault()?.Print?.PrintNmber ?? null;
                }))
                .ForMember(c => c.ClientPrintNumber, opt => opt.MapFrom((obj, dto, i, context) =>
                   {
                       return obj.OrderPrints.Where(c => c.Print.Type == PrintType.Client).LastOrDefault()?.Print?.PrintNmber ?? null;
                   }))
                .ForMember(c => c.OrderLogs, opt => opt.MapFrom((obj, dto, i, context) =>
                     {
                         return context.Mapper.Map<OrderLogDto[]>(obj.OrderLogs);
                     }))
                .ForMember(c => c.AgentPrint, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return context.Mapper.Map<PrintOrdersDto[]>(obj.OrderPrints.Select(c => c.Print).Where(c => c.Type == PrintType.Agent));
                }))
            .ForMember(c => c.ClientPrint, opt => opt.MapFrom((obj, dto, i, context) =>
             {
                 return context.Mapper.Map<PrintOrdersDto[]>(obj.OrderPrints.Select(c => c.Print).Where(c => c.Type == PrintType.Client));
             }));
            ;
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
                    return obj.OrderPrints.Where(c => c.Print.Type == PrintType.Agent).LastOrDefault()?.Print?.PrintNmber ?? null;
                }))
                .ForMember(c => c.ClientPrintNumber, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return obj.OrderPrints.Where(c => c.Print.Type == PrintType.Client).LastOrDefault()?.Print?.PrintNmber ?? null;
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

            CreateMap<Printed, PrintOrdersDto>()

                .ForMember(src => src.Orders, opt => opt.MapFrom((obj, dto, i, context) =>
                     {
                         List<PrintDto> x = new List<PrintDto>();
                         x.AddRange(context.Mapper.Map<PrintDto[]>(obj.AgnetPrints));
                         x.AddRange(context.Mapper.Map<PrintDto[]>(obj.ClientPrints));
                         return x;
                     }))
                .ForMember(c => c.Receipts, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return context.Mapper.Map<ReceiptDto[]>(obj.Receipts);
                }));
            CreateMap<AgnetPrint, PrintDto>();
            CreateMap<ClientPrint, PrintDto>()
                .ForMember(c => c.Orderplaced, opt => opt.MapFrom((order, dto, i, context) =>
                {
                    return context.Mapper.Map<NameAndIdDto>(order.OrderPlaced);
                }));

            CreateMap<Receipt, ReceiptDto>()
                .ForMember(c => c.ClientName, opt => opt.MapFrom(c => c.Client.Name));
            CreateMap<EditRequest, EditRequestDto>()
                .ForMember(c => c.Client, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return context.Mapper.Map<ClientDto>(obj.Client);
                }));
            CreateMap<Notfication, NotficationDto>()
                .ForMember(c => c.MonePlaced, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return context.Mapper.Map<NameAndIdDto>(obj.MoneyPlaced);
                }))
                .ForMember(c => c.Orderplaced, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return context.Mapper.Map<NameAndIdDto>(obj.OrderPlaced);
                }));
            CreateMap<PaymentWay, NameAndIdDto>()
                .ForMember(c => c.CanDelete, opt => opt.MapFrom(src => src.PaymentRequests.Count() == 0));
            CreateMap<PaymentRequest, PayemntRquestDto>()
                .ForMember(c => c.Client, opt => opt.MapFrom((obj, dto, i, context) =>
                     {
                         return context.Mapper.Map<ClientDto>(obj.Client);
                     }))
                .ForMember(c => c.PaymentWay, opt => opt.MapFrom((obj, dto, i, context) =>
                  {
                      return context.Mapper.Map<NameAndIdDto>(obj.PaymentWay);
                  }));

        }
    }
}
