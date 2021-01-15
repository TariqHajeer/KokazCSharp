using AutoMapper;
using KokazGoodsTransfer.Dtos.Clients;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Dtos.Currencies;
using KokazGoodsTransfer.Dtos.DepartmentDtos;
using KokazGoodsTransfer.Dtos.IncomesDtos;
using KokazGoodsTransfer.Dtos.IncomeTypes;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Dtos.OutComeDtos;
using KokazGoodsTransfer.Dtos.OutComeTypeDtos;
using KokazGoodsTransfer.Dtos.Regions;
using KokazGoodsTransfer.Dtos.Users;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
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
                .ForMember(c => c.CanDelete, opt => opt.MapFrom(src => src.Regions.Count() == 0 && src.Users.Count() == 0))
                .ForMember(c => c.CanDeleteWithRegion, opt => opt.MapFrom(src => src.Users.Count() == 0 && src.Regions.All(c => c.CanDelete)))
                .ForMember(c => c.Regions, src => src.MapFrom((country, countryDto, i, context) =>
                {
                    return context.Mapper.Map<RegionDto[]>(country.Regions);
                }))
                .MaxDepth(2);
            CreateMap<Department, DepartmentDto>()
                .ForMember(d => d.UserCount, opt => opt.MapFrom(src => src.Users.Count()));
            CreateMap<User, UserDto>()
                .ForMember(d => d.Phones, opt => opt.MapFrom((user, dto, i, context) =>
                {
                    return context.Mapper.Map<PhoneDto[]>(user.UserPhones);
                }))
                .ForMember(d => d.Department, opt => opt.MapFrom((user, userDto, i, context) =>
                     {
                         return context.Mapper.Map<DepartmentDto>(user.Department);
                     }))
                .ForMember(c => c.GroupsId, opt => opt.MapFrom(src => src.UserGroups.Select(c => c.GroupId)));
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

                .ForMember(d => d.Region, opt => opt.MapFrom((client, clientDto, i, context) =>
                {
                    return context.Mapper.Map<RegionDto>(client.Region);
                }))
                .ForMember(d => d.Phones, opt => opt.MapFrom((client, clientDto, i, context) =>
                     {
                         return context.Mapper.Map<PhoneDto[]>(client.ClientPhones);
                     }));
            CreateMap<ClientPhone, PhoneDto>();
            CreateMap<UserPhone, PhoneDto>();
            CreateMap<Client, AuthClient>()
                .ForMember(d => d.Region, opt => opt.MapFrom((client, authclient, i, context) =>
                {
                    return context.Mapper.Map<RegionDto>(client.Region);
                }))
                .ForMember(d => d.Phones, opt => opt.MapFrom((client, auth, i, context) =>
                {
                    return context.Mapper.Map<PhoneDto[]>(client.ClientPhones);
                }));
            CreateMap<CreateOutComeDto, OutCome>();
            CreateMap<OutCome, OutComeDto>()
                .ForMember(c => c.CreatedBy, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(c => c.Currency, opt => opt.MapFrom((outcome, dto, i, context) =>
                     {
                         return context.Mapper.Map<CurrencyDto>(outcome.Currency);
                     }))
                .ForMember(c => c.OutComeType, opt => opt.MapFrom((outcome, dto, i, context) =>
                {

                    return context.Mapper.Map<OutComeDto>(outcome);
                }));
            CreateMap<UpdateClientDto, Client>()
                .ForMember(c => c.Password, opt => opt.MapFrom(src => MD5Hash.GetMd5Hash(src.Password)));
            CreateMap<CUpdateClientDto, Client>()
                .ForMember(c => c.Password, opt => opt.MapFrom(src =>src.Password==null?"":MD5Hash.GetMd5Hash(src.Password)));
            CreateMap<CreateIncomeDto, Income>();
            CreateMap<Income, IncomeDto>()
                .ForMember(c => c.IncomeType, opt => opt.MapFrom((income, incomeDto, i, context) =>
                {
                    return context.Mapper.Map<IncomeTypeDto>(income.IncomeType);
                }))
                .ForMember(c => c.CreatedBy, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(c => c.Currency, opt => opt.MapFrom((income, incomeDto, i, context) =>
                     {
                         return context.Mapper.Map<CurrencyDto>(income.Currency);
                     }));
            CreateMap<Currency, CurrencyDto>()
            .ForMember(c => c.CanDelete, opt => opt.MapFrom(src => src.Incomes.Count() == 0 && src.OutComes.Count() == 0));
            CreateMap<OutComeType, OutComeTypeDto>()
                .ForMember(d => d.CanDelete, opt => opt.MapFrom(src => src.OutComes.Count() == 0));
            CreateMap<OutCome, OutComeDto>()
                .ForMember(d => d.OutComeType, opt => opt.MapFrom((outcome, dto, i, context) =>
                {
                    return context.Mapper.Map<OutComeTypeDto>(outcome.OutComeType);
                }))
                .ForMember(c => c.Currency, opt => opt.MapFrom((outcome, dto, i, context) =>
                {
                    return context.Mapper.Map<CurrencyDto>(outcome.Currency);
                }))
                .ForMember(c => c.CreatedBy, opt => opt.MapFrom(src => src.User.Name));
            CreateMap<OrderPlaced, NameAndIdDto>();
            CreateMap<MoenyPlaced, NameAndIdDto>();
            CreateMap<CreateOrderFromClient, Order>()
                .ForMember(s => s.RecipientPhones, opt => opt.MapFrom(src => String.Join(", ", src.RecipientPhones)));
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
                }));
            CreateMap<Order, OrderTypeResponseClientDto>()
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

        }
    }
}
