﻿using AutoMapper;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Dtos.TreasuryDtos;
using System;

namespace KokazGoodsTransfer.Dtos.AutoMapperProfile
{
    public class TreasuryProfile : Profile
    {
        public TreasuryProfile()
        {
            CreateMap<Treasury, TreasuryDto>()
                .ForMember(c => c.History, opt => opt.MapFrom((obj, dto, i, context) =>
                {
                    return context.Mapper.Map<TreasuryHistoryDto[]>(dto.History);
                }));
            CreateMap<TreasuryHistory, TreasuryHistoryDto>();
            CreateMap<CreateTreasuryDto, Treasury>()
                .ForMember(c => c.CreateOnUtc, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(c => c.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(c => c.Total, opt => opt.MapFrom(src => src.Amount))
                .ForMember(c => c.Id, opt => opt.MapFrom(src => src.UserId));
            CreateMap<CreateTreasuryDto, CashMovment>()
                .ForMember(c => c.TreasuryId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(c => c.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(c => c.CreatedOnUtc, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<CashMovment, TreasuryHistory>()
                .ForMember(c => c.TreasuryId, opt => opt.MapFrom(src => src.TreasuryId))
                .ForMember(c => c.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(c => c.CreatedOnUtc, opt => opt.MapFrom(src => DateTime.UtcNow));



        }
    }
}
