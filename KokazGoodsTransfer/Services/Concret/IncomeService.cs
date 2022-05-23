using AutoMapper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.IncomesDtos;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Helper;
using KokazGoodsTransfer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Concret
{
    public class IncomeService : CRUDService<Income, IncomeDto, CreateIncomeDto, UpdateIncomeDto>, IIncomeService
    {
        private readonly IUserService _userService;
        private readonly IUintOfWork _uintOfWork;
        public IncomeService(IUintOfWork uintOfWork, IRepository<Income> repository, IMapper mapper, IUserService userService) : base(repository, mapper)
        {
            _userService = userService;
            _uintOfWork = uintOfWork;

        }
        public override async Task<ErrorRepsonse<IncomeDto>> AddAsync(CreateIncomeDto createDto)
        {
            var income = _mapper.Map<Income>(createDto);
            income.UserId = _userService.AuthoticateUserId();
            var treasury = await _uintOfWork.Repository<Treasury>().GetById(_userService.AuthoticateUserId());
            treasury.Total += income.Amount;
            var treauseryHistory = new TreasuryHistory()
            {
                TreasuryId = treasury.Id,
                Amount = income.Amount,
                CreatedOnUtc = DateTime.UtcNow,

            };
            await _uintOfWork.BegeinTransaction();
            try
            {
                await _uintOfWork.Add(income);
                treauseryHistory.IncomeId = income.Id;
                await _uintOfWork.Add(treauseryHistory);
                await _uintOfWork.Update(treasury);
                await _uintOfWork.Commit();
            }
            catch (Exception ex)
            {
                await _uintOfWork.Rollback();
                throw ex;
            }
            await _uintOfWork.Repository<Income>().LoadRefernces(income, c => c.User);
            await _uintOfWork.Repository<Income>().LoadRefernces(income, c => c.IncomeType);
            return new ErrorRepsonse<IncomeDto>(_mapper.Map<IncomeDto>(income));
        }
        public override async Task<IEnumerable<IncomeDto>> AddRangeAsync(IEnumerable<CreateIncomeDto> entities)
        {
            var incomes = _mapper.Map<List<Income>>(entities);
            var userId = _userService.AuthoticateUserId();
            incomes.ForEach(c => c.UserId = userId);
            var treasury = await _uintOfWork.Repository<Treasury>().GetById(userId);
            treasury.Total += incomes.Sum(c => c.Amount);
            await _uintOfWork.BegeinTransaction();
            try
            {
                await _uintOfWork.AddRange(incomes);
                var hisotries = incomes.Select(c => new TreasuryHistory()
                {
                    Amount = c.Amount,
                    CreatedOnUtc = DateTime.UtcNow,
                    IncomeId = c.Id,
                    TreasuryId = treasury.Id
                });
                await _uintOfWork.AddRange(hisotries);
                await _uintOfWork.Update(treasury);
                await _uintOfWork.Commit();
            }
            catch (Exception ex)
            {
                await _uintOfWork.Rollback();
            }
            return null;
        }
        public override async Task<IncomeDto> GetById(int id)
        {
            var entity = await _repository.GetById(id);
            if (entity == null)
                return null;
            await _repository.LoadRefernces(entity, c => c.User);
            await _repository.LoadRefernces(entity, c => c.IncomeType);
            return _mapper.Map<IncomeDto>(entity);
        }
    }
}
