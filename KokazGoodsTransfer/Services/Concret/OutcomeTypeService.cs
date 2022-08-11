using AutoMapper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.OutComeTypeDtos;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Helper;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Concret
{
    public class OutcomeTypeService : IndexCURDService<OutComeType, OutComeTypeDto, CreateOutComeTypeDto, UpdateOutComeTypeDto>, IOutcomeTypeService
    {
        private readonly IRepository<OutCome> _outcomeRepository;
        public OutcomeTypeService(IRepository<OutComeType> repository, IRepository<OutCome> outcomeRepository, IMapper mapper, Logging logging, IHttpContextAccessor httpContextAccessor) : base(repository, mapper, logging,httpContextAccessor)
        {
            _outcomeRepository = outcomeRepository;
        }
        public override async Task<ErrorRepsonse<OutComeTypeDto>> Delete(int id)
        {
            var enrity = await _repository.GetById(id);
            var response = new ErrorRepsonse<OutComeTypeDto>();
            if (enrity == null)
            {
                response.NotFound = true;
                response.Errors.Add("Not.Found");
            }
            else
            {
                if (await _outcomeRepository.Any(c => c.OutComeTypeId == id))
                {
                    response.CantDelete = true;
                    response.Errors.Add("Cant.Delete");
                }
                else
                {
                    await _repository.Delete(enrity);
                    response.Data = _mapper.Map<OutComeTypeDto>(enrity);
                }
            }
            return response;
        }
    }
}
