﻿using AutoMapper;
using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.AgentDtos;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using LinqKit;
using KokazGoodsTransfer.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq.Expressions;

namespace KokazGoodsTransfer.Services.Concret
{
    public class AgentPrintService : IAgentPrintService
    {
        private readonly IRepository<AgentPrint> _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected int AuthoticateUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.Claims.ToList().Where(c => c.Type == "UserID").Single();
            return Convert.ToInt32(userIdClaim.Value);
        }
        public AgentPrintService(IRepository<AgentPrint> repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PagingResualt<IEnumerable<PrintOrdersDto>>> GetPrint(PagingDto pagingDto, PrintFilterDto printFilterDto)
        {
            var predicte = PredicateBuilder.New<AgentPrint>(true);
            if (printFilterDto.Date != null)
            {
                predicte = predicte.And(c => c.Date == printFilterDto.Date);
            }
            if (printFilterDto.Number != null)
            {
                predicte = predicte.And(c => c.Id == printFilterDto.Number);
            }
            predicte = predicte.And(c => c.AgentOrderPrints.Any(c => c.Order.AgentId == AuthoticateUserId()));
            var result = await _repository.GetAsync(pagingDto, predicte);
            return new PagingResualt<IEnumerable<PrintOrdersDto>>()
            {
                Data = _mapper.Map<IEnumerable<PrintOrdersDto>>(result.Data),
                Total = result.Total
            };
        }

        public async Task<PrintOrdersDto> GetPrintById(int printNumber)
        {
            var printed = await _repository.FirstOrDefualt(c => c.Id == printNumber, c => c.AgentPrintDetails);
            return _mapper.Map<PrintOrdersDto>(printed);
        }

        public async Task<int> Count(Expression<Func<AgentPrint, bool>> filter = null)
        {
            return await _repository.Count(filter);
        }
    }
}
