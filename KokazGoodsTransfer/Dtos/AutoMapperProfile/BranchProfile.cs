﻿using AutoMapper;
using KokazGoodsTransfer.Dtos.BranchDtos;
using KokazGoodsTransfer.Models;

namespace KokazGoodsTransfer.Dtos.AutoMapperProfile
{
    public class BranchProfile:Profile
    {
        public BranchProfile()
        {
            CreateMap<Branch, BranchDto>();
        }
    }
}