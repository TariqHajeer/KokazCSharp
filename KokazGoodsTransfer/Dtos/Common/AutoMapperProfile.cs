using AutoMapper;
using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.Dtos.Clients;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Dtos.DiscountDtos;
using KokazGoodsTransfer.Dtos.EditRequestDtos;
using KokazGoodsTransfer.Dtos.IncomesDtos;
using KokazGoodsTransfer.Dtos.IncomeTypes;
using KokazGoodsTransfer.Dtos.NotifcationDtos;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Dtos.OrdersTypes;
using KokazGoodsTransfer.Dtos.OutComeDtos;
using KokazGoodsTransfer.Dtos.OutComeTypeDtos;
using KokazGoodsTransfer.Dtos.PayemntRequestDtos;
using KokazGoodsTransfer.Dtos.PointSettingsDtos;
using KokazGoodsTransfer.Dtos.ReceiptDtos;
using KokazGoodsTransfer.Dtos.Regions;
using KokazGoodsTransfer.Dtos.Users;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Static;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KokazGoodsTransfer.Dtos.Common
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {

            CreateMap<IndexEntity, NameAndIdDto>();
            CreateMap<ClientPhone, PhoneDto>();
            CreateMap<UserPhone, PhoneDto>();

            #region  user
            CreateMap<User, UserDto>()
                .ForMember(c => c.Password, opt => opt.Ignore())
                .ForMember(d => d.Phones, opt => opt.MapFrom((user, dto, i, context) =>
                {
                    return context.Mapper.Map<PhoneDto[]>(user.UserPhones);
                }))
                .ForMember(c => c.GroupsId, opt => opt.MapFrom(src => src.UserGroups.Select(c => c.GroupId)))
                .ForMember(c => c.Countries, opt => opt.MapFrom((user, dto, i, context) =>
                  {
                      if (user.AgentCountrs == null)
                      {
                          return null;
                      }
                      else
                      {

                          user.AgentCountrs.ToList().ForEach(c =>
                          {
                              if (c.Country != null)
                                  c.Country.AgentCountrs = null;
                          });
                      }
                      return context.Mapper.Map<CountryDto[]>(user.AgentCountrs.Select(c => c.Country));
                  }));
            CreateMap<CreateUserDto, User>()
            .ForMember(des => des.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(des => des.Password, opt => opt.MapFrom(src => MD5Hash.GetMd5Hash(src.Password)))
            .ForMember(des => des.AgentCountrs, opt => opt.MapFrom((dto, obk, i, context) =>
            {
                var agentCountr = new List<AgentCountr>();
                if (dto.Countries != null && dto.Countries.Any())
                {
                    dto.Countries.Distinct().ToList().ForEach(countryId =>
                    {
                        agentCountr.Add(new AgentCountr()
                        {
                            CountryId = countryId
                        });
                    });
                }
                return agentCountr;
            }))
            .ForMember(des => des.UserPhones, opt => opt.MapFrom((dto, obj, i, context) =>
            {
                var userPhones = new List<UserPhone>();
                if (dto.Phones != null && dto.Phones.Any())
                {
                    dto.Phones.Distinct().ToList().ForEach(phone =>
                    {
                        userPhones.Add(new UserPhone()
                        {
                            Phone = phone
                        });
                    });

                }
                return userPhones;
            }))
            .ForMember(des => des.UserGroups, opt => opt.MapFrom((dto, obj, i, context) =>
                     {
                         var userGroup = new List<UserGroup>();
                         if (dto.GroupsId != null && dto.GroupsId.Any())
                         {
                             dto.GroupsId.ToList().ForEach(group =>
                             {
                                 userGroup.Add(new UserGroup()
                                 {
                                     GroupId = group
                                 });
                             });
                         }
                         return userGroup;
                     }));
            CreateMap<UpdateUserDto, User>()
            .ForMember(c => c.Password, opt => opt.MapFrom(src => String.IsNullOrEmpty(src.Password) ? String.Empty : MD5Hash.GetMd5Hash(src.Password)))
            .ForMember(des => des.Salary, opt => opt.MapFrom(src => src.CanWorkAsAgent == true ? src.Salary : (decimal?)null))
            .ForMember(c => c.UserGroups, opt => opt.MapFrom((dto, obj, i, context) =>
            {
                if (dto.CanWorkAsAgent)
                {
                    return null;
                }
                return obj.UserGroups;
            }))
            .ForMember(src => src.AgentCountrs, opt => opt.MapFrom((dto, obj, i, context) =>
            {
                if (!dto.CanWorkAsAgent)
                    return null;
                var agentCountries = new List<AgentCountr>();
                foreach (var item in dto.Countries)
                {
                    agentCountries.Add(new AgentCountr()
                    {
                        AgentId = obj.Id,
                        CountryId = item
                    });
                }
                return agentCountries;
            }));
            #endregion
            CreateMap<CreateClientDto, Client>()
                .ForMember(c => c.Password, opt => opt.MapFrom(src => MD5Hash.GetMd5Hash(src.Password)))
                .ForMember(c => c.Points, opt => opt.MapFrom(src => 0))
                .ForMember(c => c.ClientPhones, opt => opt.MapFrom((dto, obj, i, context) =>
                     {
                         var clientPhones = new List<ClientPhone>();
                         dto.Phones.Distinct().ToList().ForEach(c =>
                         {
                             clientPhones.Add(new ClientPhone()
                             {
                                 Phone = c
                             });
                         });
                         return clientPhones;
                     }));
            CreateMap<Privilege, UserPrivilegeDto>();
            CreateMap<User, AuthenticatedUserDto>()
                .ForMember(c => c.Privileges, opt => opt.MapFrom((user, authDto, i, context) =>
                     {
                         return context.Mapper.Map<UserPrivilegeDto[]>(user.UserGroups.SelectMany(c => c.Group.GroupPrivileges.Select(c => c.Privileg).Distinct()));
                     }));
            CreateMap<Client, ClientDto>()
                .ForMember(c => c.Total, opt => opt.MapFrom(src => src.Receipts.Where(c => c.PrintId == null).Sum(c => c.Amount)))
                .ForMember(d => d.Country, opt => opt.MapFrom((client, clientDto, i, context) =>
                {
                    return context.Mapper.Map<CountryDto>(client.Country);
                }))
                .ForMember(d => d.Phones, opt => opt.MapFrom((client, clientDto, i, context) =>
                     {
                         return context.Mapper.Map<PhoneDto[]>(client.ClientPhones);
                     }));

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
                .ForMember(c => c.Password, opt => opt.MapFrom((dto, obj, i, context) =>
                {
                    if (String.IsNullOrEmpty(dto.Password))
                        return obj.Password;
                    return MD5Hash.GetMd5Hash(dto.Password);
                }));
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
            CreateMap<AgentPrint, PrintOrdersDto>()
                .ForMember(c => c.PrintNmber, opt => opt.MapFrom(c => c.Id))
                .ForMember(c => c.Receipts, opt => opt.Ignore())
                .ForMember(c => c.Discount, opt => opt.Ignore());


            CreateMap<AgentPrintDetail, AgentPrintDetailDto>();

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
                }))
                .ForMember(c => c.Discount, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return context.Mapper.Map<DiscountDto>(obj.Discounts.FirstOrDefault());
                }))
                ;
            CreateMap<Discount, DiscountDto>();
            CreateMap<AgnetPrint, PrintDto>();
            CreateMap<ClientPrint, PrintDto>()
                .ForMember(c => c.Orderplaced, opt => opt.MapFrom((order, dto, i, context) =>
                {
                    return context.Mapper.Map<NameAndIdDto>(order.OrderPlaced);
                }));

            CreateMap<Receipt, ReceiptDto>()
                .ForMember(c => c.ClientName, opt => opt.MapFrom(c => c.Client.Name))
                .ForMember(c => c.PrintNumber, opt => opt.MapFrom(c => c.Print.PrintNmber));
            CreateMap<EditRequest, EditRequestDto>()
                .ForMember(c => c.Client, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return context.Mapper.Map<ClientDto>(obj.Client);
                }));
            CreateMap<Notfication, NotficationDto>()
                .ForMember(c => c.MoneyPlaced, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return context.Mapper.Map<NameAndIdDto>(obj.MoneyPlaced);
                }))
                .ForMember(c => c.OrderPlaced, opt => opt.MapFrom((obj, dto, i, context) =>
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

            CreateMap<CreatePointSetting, PointsSetting>();
            CreateMap<PointsSetting, PointSettingsDto>();
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
