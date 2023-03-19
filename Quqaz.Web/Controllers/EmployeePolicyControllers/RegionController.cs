using System.Linq;
using System.Threading.Tasks;
using Quqaz.Web.Dtos.Regions;
using Quqaz.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Quqaz.Web.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : AbstractEmployeePolicyController
    {

        private readonly IRegionCashedService _regionCashedService;
        private readonly ICountryCashedService _countryCashedService;
        private readonly IUserCashedService _userCashedService;
        public RegionController(IRegionCashedService regionCashedService, ICountryCashedService countryCashedService, IUserCashedService userCashedService)
        {
            _regionCashedService = regionCashedService;
            _countryCashedService = countryCashedService;
            _userCashedService = userCashedService;
        }
        private void RemoveRelatedCash()
        {
            _userCashedService.RemoveCash();
            _countryCashedService.RemoveCash();
        }
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _regionCashedService.GetCashed());
        [HttpPost]
        public async Task<IActionResult> Create(CreateRegionDto createRegionDto)
        {
            var result = await _regionCashedService.AddAsync(createRegionDto);
            RemoveRelatedCash();
            return Ok(result.Data);


        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            var result = await _regionCashedService.Delete(id);
            if (result.Errors.Any())
                return Conflict();
            RemoveRelatedCash();
            return Ok();

        }
        [HttpPatch]
        public async Task<IActionResult> UpdateRegion([FromBody] UpdateRegionDto updateRegion)
        {

            var result = await _regionCashedService.Update(updateRegion);
            if (result.Errors.Any())
            {
                return Conflict();
            }
            RemoveRelatedCash();
            return Ok();

        }
    }
}