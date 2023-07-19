using AutoMapper;
using Quqaz.Web.DAL.Infrastructure.Interfaces;
using Quqaz.Web.Dtos.IncomeTypes;
using Quqaz.Web.Helpers;
using Quqaz.Web.Models;
using Quqaz.Web.Services.Helper;
using Quqaz.Web.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Quqaz.Web.Services.Concret
{
    public class IncomeTypeSerivce : IndexCURDService<IncomeType, IncomeTypeDto, CreateIncomeTypeDto, UpdateIncomeTypeDto>, IIncomeTypeSerive
    {
        private readonly IRepository<Income> _incomeReposiory;
        public IncomeTypeSerivce(IRepository<IncomeType> repository, IRepository<Income> incomeReposiory ,IMapper mapper,Logging logging, IHttpContextAccessorService httpContextAccessorService) : base(repository, mapper, logging, httpContextAccessorService)
        {
            _incomeReposiory = incomeReposiory;
        }
        public override async Task<ErrorRepsonse<IncomeTypeDto>> Delete(int id)
        {
            var enrity = await _repository.GetById(id);
            var response = new ErrorRepsonse<IncomeTypeDto>();
            if (enrity == null)
            {
                response.NotFound = true;
                response.Errors.Add("Not.Found");
            }
            else
            {
                if (await _incomeReposiory.Any(c => c.IncomeTypeId == id))
                {
                    response.CantDelete = true;
                    response.Errors.Add("Cant.Delete");
                }
                else
                {
                    await _repository.Delete(enrity);
                    response.Data = _mapper.Map<IncomeTypeDto>(enrity);
                }
            }
            return response;
        }
    }
}
