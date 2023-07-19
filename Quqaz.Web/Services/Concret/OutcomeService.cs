using AutoMapper;
using Quqaz.Web.DAL.Helper;
using Quqaz.Web.DAL.Infrastructure.Interfaces;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.OutComeDtos;
using Quqaz.Web.Helpers;
using Quqaz.Web.Models;
using Quqaz.Web.Services.Helper;
using Quqaz.Web.Services.Interfaces;
using LinqKit;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Services.Concret
{
    public class OutcomeService : CRUDService<OutCome, OutComeDto, CreateOutComeDto, UpdateOuteComeDto>, IOutcomeService
    {
        private readonly IUserService _userService;
        private readonly IUintOfWork _uintOfWork;
        public OutcomeService(IRepository<OutCome> repository, IMapper mapper,
            IUserService userService, IUintOfWork uintOfWork, Logging logging, IHttpContextAccessorService httpContextAccessorService) : base(repository, mapper, logging, httpContextAccessorService)
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
                _logging.WriteExption(ex);
                await _uintOfWork.Rollback();
            }
            return null;
        }

        public async Task<PagingResualt<IEnumerable<OutComeDto>>> GetAsync(Filtering filtering, PagingDto pagingDto)
        {
            var predicate = PredicateBuilder.New<OutCome>(true);
            if (filtering.MaxAmount != null)
                predicate = predicate.And(c => c.Amount <= filtering.MaxAmount);
            if (filtering.MinAmount != null)
                predicate = predicate.And(c => c.Amount >= filtering.MaxAmount);
            if (filtering.Type != null)
                predicate = predicate.And(c => c.OutComeTypeId == filtering.Type);
            if (filtering.UserId != null)
                predicate = predicate.And(c => c.UserId == filtering.UserId);
            if (filtering.FromDate != null)
                predicate = predicate.And(c => c.Date >= filtering.FromDate);
            if (filtering.ToDate != null)
                predicate = predicate.And(c => c.Date <= filtering.ToDate);
            var pagingReuslt = await _repository.GetAsync(pagingDto, predicate, c => c.User, c => c.OutComeType);
            return new PagingResualt<IEnumerable<OutComeDto>>()
            {
                Total = pagingReuslt.Total,
                Data = _mapper.Map<IEnumerable<OutComeDto>>(pagingReuslt.Data)
            };
        }

        public override async Task<OutComeDto> GetById(int id)
        {
            var entity = await _repository.GetById(id);
            if (entity == null)
                return null;
            await _repository.LoadRefernces(entity, c => c.User);
            await _repository.LoadRefernces(entity, c => c.OutComeType);
            return _mapper.Map<OutComeDto>(entity);
        }
    }
}
