﻿using AutoMapper;
using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.TreasuryDtos;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Helper;
using KokazGoodsTransfer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using KokazGoodsTransfer.Models.Static;
using System.Linq.Expressions;

namespace KokazGoodsTransfer.Services.Concret
{
    public class TreasuryService : ITreasuryService
    {
        private readonly IMapper _mapper;
        private readonly IUintOfWork _uintOfWork;
        private readonly IUserService _userService;
        private readonly IRepository<Treasury> _repository;
        private readonly IRepository<TreasuryHistory> _historyRepositroy;
        public TreasuryService(IUintOfWork uintOfWork, IRepository<Treasury> repository, IRepository<TreasuryHistory> historyRepositroy, IUserService userService, IMapper mapper)
        {
            _mapper = mapper;
            _uintOfWork = uintOfWork;
            _userService = userService;
            _repository = repository;
            _historyRepositroy = historyRepositroy;
        }
        public async Task<ErrorRepsonse<TreasuryDto>> Create(CreateTreasuryDto createTreasuryDto)
        {
            if (await _uintOfWork.Repository<Treasury>().Any(c => c.Id == createTreasuryDto.UserId))
            {
                return new ErrorRepsonse<TreasuryDto>("يوجد صندوق لهذا المتسخدم ");
            }
            var treaury = _mapper.Map<Treasury>(createTreasuryDto);
            await _uintOfWork.BegeinTransaction();
            try
            {
                await _uintOfWork.Add(treaury);
                if (createTreasuryDto.Amount != 0)
                {
                    var cashMovment = _mapper.Map<CashMovment>(createTreasuryDto);
                    cashMovment.CreatedBy = _userService.AuthoticateUserName();
                    var history = _mapper.Map<TreasuryHistory>(cashMovment);
                    cashMovment.TreasuryHistories.Add(history);
                    await _uintOfWork.Add(cashMovment);
                    history.CashMovment = cashMovment;
                    treaury.TreasuryHistories.Add(history);
                }
                await _uintOfWork.Commit();

                var dto = _mapper.Map<TreasuryDto>(treaury);
                dto.History = new PagingResualt<IEnumerable<TreasuryHistoryDto>>()
                {
                    Total = treaury.TreasuryHistories.Count,
                    Data = _mapper.Map<TreasuryHistoryDto[]>(treaury.TreasuryHistories)
                };
                return new ErrorRepsonse<TreasuryDto>(dto);
            }
            catch (Exception ex)
            {
                await _uintOfWork.Rollback();
                return new ErrorRepsonse<TreasuryDto>("حصل خطأ ما ")
                {

                };
            }
        }

        public async Task<TreasuryDto> GetById(int id)
        {
            var treasury = await _repository.GetById(id);
            if (treasury == null)
                return null;
            var history = await _historyRepositroy.GetAsync(new Paging() { Page = 1, RowCount = 10 }, c => c.TreasuryId == id, new string[] { "Receipt" });
            var treasuryDto = _mapper.Map<TreasuryDto>(treasury);
            treasuryDto.History = new PagingResualt<IEnumerable<TreasuryHistoryDto>>()
            {
                Total = history.Total,
                Data = _mapper.Map<TreasuryHistoryDto[]>(history.Data)
            };
            return treasuryDto;
        }
        public async Task<PagingResualt<IEnumerable<TreasuryHistoryDto>>> GetTreasuryHistory(int id, PagingDto pagingDto)
        {
            var hisotres = await _historyRepositroy.GetAsync(pagingDto, c => c.TreasuryId == id);
            return new PagingResualt<IEnumerable<TreasuryHistoryDto>>()
            {
                Total = hisotres.Total,
                Data = _mapper.Map<TreasuryHistoryDto[]>(hisotres.Data)
            };
        }

        public async Task<ErrorRepsonse<TreasuryHistoryDto>> IncreaseAmount(int id, decimal amount)
        {
            var treausry = await _uintOfWork.Repository<Treasury>().GetById(id);
            await _uintOfWork.BegeinTransaction();
            try
            {
                var cashMovment = new CashMovment()
                {
                    Amount = amount,
                    CreatedBy = _userService.AuthoticateUserName(),
                    CreatedOnUtc = DateTime.UtcNow,
                    TreasuryId = treausry.Id,
                };
                await _uintOfWork.Add(cashMovment);
                var history = new TreasuryHistory()
                {
                    CreatedOnUtc = cashMovment.CreatedOnUtc,
                    Amount = amount,
                    CashMovment = cashMovment,
                    CashMovmentId = cashMovment.Id,
                    TreasuryId = id,
                };
                await _uintOfWork.Add(history);
                treausry.Total += amount;
                await _uintOfWork.Update(treausry);
                await _uintOfWork.Commit();
                return new ErrorRepsonse<TreasuryHistoryDto>(_mapper.Map<TreasuryHistoryDto>(history));
            }
            catch (Exception ex)
            {
                await _uintOfWork.Rollback();
                return new ErrorRepsonse<TreasuryHistoryDto>("حدث خطأ ما ");
            }
        }
        public async Task<ErrorRepsonse<TreasuryHistoryDto>> DecreaseAmount(int id, decimal amount)
        {
            var treausry = await _uintOfWork.Repository<Treasury>().GetById(id);
            await _uintOfWork.BegeinTransaction();
            try
            {
                var cashMovment = new CashMovment()
                {
                    Amount = -amount,
                    CreatedBy = _userService.AuthoticateUserName(),
                    CreatedOnUtc = DateTime.UtcNow,
                    TreasuryId = treausry.Id,
                };
                await _uintOfWork.Add(cashMovment);
                var history = new TreasuryHistory()
                {
                    CreatedOnUtc = cashMovment.CreatedOnUtc,
                    Amount = -amount,
                    CashMovment = cashMovment,
                    CashMovmentId = cashMovment.Id,
                    TreasuryId = id,
                };
                await _uintOfWork.Add(history);
                treausry.Total -= amount;
                await _uintOfWork.Update(treausry);
                await _uintOfWork.Commit();
                return new ErrorRepsonse<TreasuryHistoryDto>(_mapper.Map<TreasuryHistoryDto>(history));
            }
            catch (Exception ex)
            {
                await _uintOfWork.Rollback();
                return new ErrorRepsonse<TreasuryHistoryDto>("حدث خطأ ما ");
            }
        }

        public async Task DisActive(int id)
        {
            var treausry = await _repository.GetById(id);
            treausry.IsActive = false;
            await _repository.Update(treausry);
        }

        public async Task Active(int id)
        {
            var treausry = await _repository.GetById(id);
            treausry.IsActive = true;
            await _repository.Update(treausry);
        }

        public async Task IncreaseAmountByOrderFromAgent(ReceiptOfTheOrderStatus receiptOfTheOrderStatus)
        {
            var receiptOfTheOrderStatusDetalis = receiptOfTheOrderStatus.ReceiptOfTheOrderStatusDetalis.Where(c => c.MoneyPlacedId != (int)MoneyPalcedEnum.WithAgent && c.MoneyPlacedId != (int)MoneyPalcedEnum.OutSideCompany).ToList();
            if (receiptOfTheOrderStatusDetalis.Any())
            {
                var treaury = await _repository.GetById(_userService.AuthoticateUserId());
                var total = receiptOfTheOrderStatusDetalis.Sum(c => c.Cost - c.AgentCost);
                treaury.Total += total;
                await _uintOfWork.Update(treaury);
                var history = new TreasuryHistory()
                {
                    Amount = total,
                    ReceiptOfTheOrderStatusId = receiptOfTheOrderStatus.Id,
                    TreasuryId = treaury.Id,
                    CreatedOnUtc = DateTime.UtcNow,
                };
                await _uintOfWork.Add(history);
            }
        }

        public async Task<bool> Any(Expression<Func<Treasury, bool>> expression)
        {
            return await _repository.Any(expression);
        }
    }
}