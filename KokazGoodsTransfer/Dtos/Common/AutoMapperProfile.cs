using AutoMapper;
using KokazGoodsTransfer.Dtos.Clients;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Dtos.DepartmentDtos;
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

            CreateMap<Country, CountryDto>()
                .ForMember(c => c.CanDelete, opt => opt.MapFrom(src => src.Regions.Count() == 0 && src.Users.Count() == 0))
                .ForMember(c => c.Regions, src => src.MapFrom((country, countryDto, i, context) =>
                {
                    return context.Mapper.Map<RegionDto[]>(country.Regions);
                }))
                .MaxDepth(2);
            CreateMap<Department, DepartmentDto>()
                .ForMember(d => d.UserCount, opt => opt.MapFrom(src => src.Users.Count()));
            CreateMap<User, UserDto>()
                .ForMember(d => d.Phones, opt => opt.MapFrom(src => src.UserPhones.Select(c => c.Phone).ToList()))
                .ForMember(d => d.Department, opt => opt.MapFrom((user, userDto, i, context) =>
                     {
                         return context.Mapper.Map<DepartmentDto>(user.Department);
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
                
                .ForMember(d => d.Region, opt => opt.MapFrom((client, clientDto, i, context) =>
                {
                    return context.Mapper.Map<RegionDto>(client.Region);
                }));
        }
    }
}
