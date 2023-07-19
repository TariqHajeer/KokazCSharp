using AutoMapper;
using Quqaz.Web.Dtos.Clients;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.EditRequestDtos;
using Quqaz.Web.Helpers;
using Quqaz.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quqaz.Web.Dtos.AutoMapperProfile
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<ClientPhone, PhoneDto>();
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

            CreateMap<Client, ClientDto>()
                .ForMember(c => c.Total, opt => opt.MapFrom(src => src.Receipts.Where(c => c.ClientPaymentId == null).Sum(c => c.Amount)))
                .ForMember(d => d.Country, opt => opt.MapFrom((client, clientDto, i, context) =>
                {
                    return context.Mapper.Map<NameAndIdDto>(client.Country);
                }))
                .ForMember(d => d.Phones, opt => opt.MapFrom((client, clientDto, i, context) =>
                {
                    return context.Mapper.Map<PhoneDto[]>(client.ClientPhones);
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

            CreateMap<Client, AuthClient>()
                .ForMember(d => d.Country, opt => opt.MapFrom((client, authclient, i, context) =>
                {
                    return context.Mapper.Map<NameAndIdDto>(client.Country);
                })) 
                .ForMember(d => d.Phones, opt => opt.MapFrom((client, auth, i, context) =>
                {
                    return context.Mapper.Map<PhoneDto[]>(client.ClientPhones);
                }));

            CreateMap<EditRequest, EditRequestDto>()
                .ForMember(c => c.Client, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return context.Mapper.Map<ClientDto>(obj.Client);
                }));
        }

    }
}
