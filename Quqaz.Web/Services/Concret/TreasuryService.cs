using AutoMapper;
using Quqaz.Web.DAL.Helper;
using Quqaz.Web.DAL.Infrastructure.Interfaces;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.TreasuryDtos;
using Quqaz.Web.Models;
using Quqaz.Web.Services.Helper;
using Quqaz.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Quqaz.Web.Models.Static;
using System.Linq.Expressions;
using Quqaz.Web.Helpers;

namespace Quqaz.Web.Services.Concret
{
    public class TreasuryService : ITreasuryService
    {
        private readonly IMapper _mapper;
        private readonly IUintOfWork _uintOfWork;
        private readonly IUserService _userService;
        private readonly IRepository<Treasury> _repository;
        private readonly IRepository<TreasuryHistory> _historyRepositroy;
        private readonly IRepository<CashMovment> _cashMovmentRepositroy;
        private readonly Logging _logging;
        public TreasuryService(IUintOfWork uintOfWork, IRepository<Treasury> repository, IRepository<TreasuryHistory> historyRepositroy, IUserService userService, IMapper mapper, Logging logging, IRepository<CashMovment> cashMovmentRepositroy)
        {
            _mapper = mapper;
            _uintOfWork = uintOfWork;
            _userService = userService;
            _repository = repository;
            _historyRepositroy = historyRepositroy;
            _logging = logging;
            _cashMovmentRepositroy = cashMovmentRepositroy;
        }
        public async Task<IEnumerable<TreasuryDto>> GetAll()
        {
            var Treasuries = await _repository.GetAll(c => c.IdNavigation);
            return _mapper.Map<IEnumerable<TreasuryDto>>(Treasuries);
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
                _logging.WriteExption(ex);
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
            var history = await _historyRepositroy.GetAsync(new PagingDto() { Page = 1, RowCount = 10 }, c => c.TreasuryId == id, new string[] { "Receipt" }, orderBy: c => c.OrderByDescending(h => h.Id));
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
            var hisotres = await _historyRepositroy.GetAsync(pagingDto, c => c.TreasuryId == id, new string[] { "Receipt" }, c => c.OrderByDescending(h => h.Id));
            return new PagingResualt<IEnumerable<TreasuryHistoryDto>>()
            {
                Total = hisotres.Total,
                Data = _mapper.Map<TreasuryHistoryDto[]>(hisotres.Data)
            };
        }

        public async Task<ErrorRepsonse<TreasuryHistoryDto>> IncreaseAmount(int id, CreateCashMovmentDto createCashMovment)
        {
            var treausry = await _uintOfWork.Repository<Treasury>().GetById(id);
            await _uintOfWork.BegeinTransaction();
            try
            {
                var cashMovment = new CashMovment()
                {
                    Amount = createCashMovment.Amount,
                    Note = createCashMovment.Note,
                    CreatedBy = _userService.AuthoticateUserName(),
                    CreatedOnUtc = DateTime.UtcNow,
                    TreasuryId = treausry.Id,
                };
                await _uintOfWork.Add(cashMovment);
                var history = new TreasuryHistory()
                {
                    CreatedOnUtc = cashMovment.CreatedOnUtc,
                    Amount = createCashMovment.Amount,
                    CashMovment = cashMovment,
                    CashMovmentId = cashMovment.Id,
                    TreasuryId = id,
                };
                await _uintOfWork.Add(history);
                treausry.Total += createCashMovment.Amount;
                await _uintOfWork.Update(treausry);
                await _uintOfWork.Commit();
                return new ErrorRepsonse<TreasuryHistoryDto>(_mapper.Map<TreasuryHistoryDto>(history));
            }
            catch (Exception ex)
            {
                _logging.WriteExption(ex);
                await _uintOfWork.Rollback();
                return new ErrorRepsonse<TreasuryHistoryDto>("حدث خطأ ما ");
            }
        }
        public async Task<ErrorRepsonse<TreasuryHistoryDto>> DecreaseAmount(int id, CreateCashMovmentDto createCashMovment)
        {
            var treausry = await _uintOfWork.Repository<Treasury>().GetById(id);
            await _uintOfWork.BegeinTransaction();
            try
            {
                var cashMovment = new CashMovment()
                {
                    Amount = -createCashMovment.Amount,
                    Note = createCashMovment.Note,
                    CreatedBy = _userService.AuthoticateUserName(),
                    CreatedOnUtc = DateTime.UtcNow,
                    TreasuryId = treausry.Id,
                };
                await _uintOfWork.Add(cashMovment);
                var history = new TreasuryHistory()
                {
                    CreatedOnUtc = cashMovment.CreatedOnUtc,
                    Amount = -createCashMovment.Amount,
                    CashMovment = cashMovment,
                    CashMovmentId = cashMovment.Id,
                    TreasuryId = id,
                };
                await _uintOfWork.Add(history);
                treausry.Total -= createCashMovment.Amount;
                await _uintOfWork.Update(treausry);
                await _uintOfWork.Commit();
                return new ErrorRepsonse<TreasuryHistoryDto>(_mapper.Map<TreasuryHistoryDto>(history));
            }
            catch (Exception ex)
            {
                _logging.WriteExption(ex);
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
            var receiptOfTheOrderStatusDetalis = receiptOfTheOrderStatus.ReceiptOfTheOrderStatusDetalis.Where(c => c.MoneyPalce != MoneyPalce.WithAgent && c.MoneyPalce != MoneyPalce.OutSideCompany).ToList();
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
        public async Task<PagingResualt<IEnumerable<CashMovmentDto>>> GetCashMovment(PagingDto paging, int? treasueryId)
        {
            PagingResualt<IEnumerable<CashMovment>> pagingResult;
            if (treasueryId != null)
            {
                pagingResult = await _cashMovmentRepositroy.GetAsync(paging, c => c.TreasuryId == treasueryId, c => c.Treasury.IdNavigation);
            }
            else
            {
                pagingResult = await _cashMovmentRepositroy.GetAsync(paging, null, c => c.Treasury.IdNavigation);
            }
            return new PagingResualt<IEnumerable<CashMovmentDto>>()
            {
                Total = pagingResult.Total,
                Data = _mapper.Map<CashMovmentDto[]>(pagingResult.Data)
            };

        }
        public async Task<CashMovmentDto> GetCashMovmentById(int id)
        {
            var cashMovment = await _cashMovmentRepositroy.FirstOrDefualt(c => c.Id == id, c => c.Treasury.IdNavigation);
            return _mapper.Map<CashMovmentDto>(cashMovment);
        }


    }
}
