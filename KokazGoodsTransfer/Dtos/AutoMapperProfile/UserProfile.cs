﻿using AutoMapper;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Dtos.Users;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KokazGoodsTransfer.Dtos.AutoMapperProfile
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<UserPhone, PhoneDto>();
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
            CreateMap<Privilege, UserPrivilegeDto>();
            CreateMap<User, AuthenticatedUserDto>()
                .ForMember(c => c.Privileges, opt => opt.MapFrom((user, authDto, i, context) =>
                {
                    return context.Mapper.Map<UserPrivilegeDto[]>(user.UserGroups.SelectMany(c => c.Group.GroupPrivileges.Select(c => c.Privileg).Distinct()));
                }));
        }
    }
}