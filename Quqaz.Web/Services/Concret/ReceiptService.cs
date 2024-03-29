﻿using AutoMapper;
using Quqaz.Web.CustomException;
using Quqaz.Web.DAL.Helper;
using Quqaz.Web.DAL.Infrastructure.Interfaces;
using Quqaz.Web.Dtos.Clients;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.ReceiptDtos;
using Quqaz.Web.Models;
using Quqaz.Web.Services.Interfaces;
using LinqKit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quqaz.Web.Services.Concret
{
    public class ReceiptService : IReceiptService
    {
        private readonly IRepository<Receipt> _repository;
        private readonly IMapper _mapper;
        public ReceiptService(IRepository<Receipt> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task Delete(int id)
        {
            var recipt = await _repository.FirstOrDefualt(c => c.Id == id);
            if (recipt.ClientPaymentId != null)
                throw new ConflictException("");
            await _repository.Delete(recipt);
        }

        public async Task<PagingResualt<IEnumerable<ReceiptDto>>> Get(PagingDto pagingDto, AccountFilterDto accountFilterDto)
        {
            var predicate = PredicateBuilder.New<Receipt>(true);

            if (accountFilterDto.ClientId != null)
            {
                predicate = predicate.And(c => c.ClientId == accountFilterDto.ClientId);
            }
            if (accountFilterDto.IsPay != null)
            {
                predicate = predicate.And(c => c.IsPay == (bool)accountFilterDto.IsPay);
            }
            if (accountFilterDto.Date != null)
            {
                predicate = predicate.And(c => c.Date == accountFilterDto.Date);
            }
            var pagingResualt = await _repository.GetAsync(pagingDto, predicate, c => c.ClientPayment, c => c.Client);
            return new PagingResualt<IEnumerable<ReceiptDto>>()
            {
                Total = pagingResualt.Total,
                Data = _mapper.Map<IEnumerable<ReceiptDto>>(pagingResualt.Data)
            };
        }

        public async Task<ReceiptDto> GetById(int id)
        {
            var recipt = await _repository.FirstOrDefualt(c => c.Id == id, c => c.Client);
            return _mapper.Map<ReceiptDto>(recipt);
        }

        public async Task<IEnumerable<ReceiptDto>> UnPaidRecipt(int clientId)
        {
            var reipts = await _repository.GetAsync(c => c.ClientId == clientId && c.ClientPaymentId == null);
            return _mapper.Map<IEnumerable<ReceiptDto>>(reipts);
        }
    }
}
