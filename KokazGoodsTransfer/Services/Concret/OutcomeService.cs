using AutoMapper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.OutComeDtos;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Helper;
using KokazGoodsTransfer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Concret
{
    public class OutcomeService : CRUDService<OutCome, OutComeDto, CreateOutComeDto, UpdateOuteComeDto>, IOutcomeService
    {
        private readonly IUserService _userService;
        private readonly IUintOfWork _uintOfWork;
        public OutcomeService(IRepository<OutCome> repository, IMapper mapper,
            IUserService userService, IUintOfWork uintOfWork) : base(repository, mapper)
        {
            _userService = userService;
            _uintOfWork = uintOfWork;
        }
        public override async Task<ErrorRepsonse<OutComeDto>> AddAsync(CreateOutComeDto createDto)
        {
            var outcome = _mapper.Map<OutCome>(createDto);
            outcome.UserId = _userService.AuthoticateUserId();
            var treasury = await _uintOfWork.Repository<Treasury>().GetById(_userService.AuthoticateUserId());
            treasury.Total -= outcome.Amount;
            var treauseryHistory = new TreasuryHistory()
            {
                TreasuryId = treasury.Id,
                Amount = -outcome.Amount,
                CreatedOnUtc = DateTime.UtcNow,

            };
            await _uintOfWork.BegeinTransaction();
            try
            {
                await _uintOfWork.Add(outcome);
                treauseryHistory.IncomeId = outcome.Id;
                await _uintOfWork.Add(treauseryHistory);
                await _uintOfWork.Update(treasury);
                await _uintOfWork.Commit();
            }
            catch (Exception ex)
            {
                await _uintOfWork.Rollback();
                throw ex;
            }
            await _uintOfWork.Repository<OutCome>().LoadRefernces(outcome, c => c.User);
            await _uintOfWork.Repository<OutCome>().LoadRefernces(outcome, c => c.OutComeType);
            return new ErrorRepsonse<OutComeDto>(_mapper.Map<OutComeDto>(outcome));
        }
        public override async Task<IEnumerable<OutComeDto>> AddRangeAsync(IEnumerable<CreateOutComeDto> entities)
        {
            var outcomes = _mapper.Map<List<OutCome>>(entities);
            var userId = _userService.AuthoticateUserId();
            outcomes.ForEach(c => c.UserId = userId);
            var treasury = await _uintOfWork.Repository<Treasury>().GetById(userId);
            treasury.Total -= outcomes.Sum(c => c.Amount);
            await _uintOfWork.BegeinTransaction();
            try
            {
                await _uintOfWork.AddRange(outcomes);
                var hisotries = outcomes.Select(c => new TreasuryHistory()
                {
                    Amount = -c.Amount,
                    CreatedOnUtc = DateTime.UtcNow,
                    OutcomeId = c.Id,
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
        public override async Task<OutComeDto> GetById(int id)
        {
            var entity = await _repository.GetById(id);
            await _repository.LoadRefernces(entity, c => c.User);
            await _repository.LoadRefernces(entity,c=>c.OutComeType);
            if (entity == null)
                return null;
            return _mapper.Map<OutComeDto>(entity);
        }
    }
}
