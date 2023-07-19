using AutoMapper;
using Quqaz.Web.DAL.Infrastructure.Interfaces;
using Quqaz.Web.Dtos.OutComeTypeDtos;
using Quqaz.Web.Helpers;
using Quqaz.Web.Models;
using Quqaz.Web.Services.Helper;
using Quqaz.Web.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Quqaz.Web.Services.Concret
{
    public class OutcomeTypeService : IndexCURDService<OutComeType, OutComeTypeDto, CreateOutComeTypeDto, UpdateOutComeTypeDto>, IOutcomeTypeService
    {
        private readonly IRepository<OutCome> _outcomeRepository;
        public OutcomeTypeService(IRepository<OutComeType> repository, IRepository<OutCome> outcomeRepository, IMapper mapper, Logging logging, IHttpContextAccessorService httpContextAccessorService)
            : base(repository, mapper, logging, httpContextAccessorService)
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
