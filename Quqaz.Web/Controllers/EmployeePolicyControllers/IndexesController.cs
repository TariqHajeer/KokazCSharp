using Microsoft.AspNetCore.Mvc;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndexesController : AbstractEmployeePolicyController
    {
        private readonly ICountryCashedService _countryCashedService;
        private readonly IClientCashedService _clientCashedService;
        private readonly IBranchService _branchService;

        public IndexesController(ICountryCashedService countryCashedService, IClientCashedService clientCashedService, IBranchService branchService)
        {
            _countryCashedService = countryCashedService;
            _clientCashedService = clientCashedService;
            _branchService = branchService;
        }

        [HttpGet]
        public async Task<ActionResult<IndexListDto>> Get([FromQuery] IEnumerable<IndexesType> indexesTypes)
        {
            var result = new IndexListDto();
            if (indexesTypes.Contains(IndexesType.Countries))
            {
                result.Countries = await _countryCashedService.GetCashed();
            }
            if (indexesTypes.Contains(IndexesType.Clients))
            {
                result.Clients= await _clientCashedService.GetCashed();
            }
            if (indexesTypes.Contains(IndexesType.Branches))
            {
                result.Benaches = await _branchService.GetLite();
            }
            return Ok(result);
        }
    }
}
