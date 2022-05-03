using AutoMapper;
using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.TreasuryDtos;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Helper;
using KokazGoodsTransfer.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            return new ErrorRepsonse<TreasuryDto>();
        }

        public async Task<TreasuryDto> GetById(int id)
        {
            var treasury = await _repository.GetById(id);
            var history = await _historyRepositroy.GetAsync(new Paging() { Page = 1, RowCount = 10 }, c => c.TreasuryId == id);
            var treasuryDto = _mapper.Map<TreasuryDto>(treasury);
            treasuryDto.History = new PagingResualt<IEnumerable<TreasuryHistoryDto>>()
            {
                Total = history.Total,
                Data = _mapper.Map<TreasuryHistoryDto[]>(history.Data)
            };
            return treasuryDto;
        }
    }
}
