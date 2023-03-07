using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : AbstractEmployeePolicyController
    {

        private readonly ICountryCashedService _countryCashedService;
        private readonly IRegionCashedService _regionCashedService;
        private readonly IUserCashedService _userCashedService;
        private readonly IClientCashedService _clientCashedService;
        public CountryController(ICountryCashedService countryCashedService, IRegionCashedService regionCashedService, IUserCashedService userCashedService, IClientCashedService clientCashedService)
        {
            _countryCashedService = countryCashedService;
            _regionCashedService = regionCashedService;
            _userCashedService = userCashedService;
            _clientCashedService = clientCashedService;
        }
        private void RemoveRelatedCash()
        {
            _regionCashedService.RemoveCash();
            _userCashedService.RemoveCash();
            _clientCashedService.RemoveCash();
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetAll() => Ok(await _countryCashedService.GetCashed());



        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCountryDto createCountryDto)
        {

            var result = await _countryCashedService.AddAsync(createCountryDto);
            if (result.Errors.Any())
                return Conflict();
            RemoveRelatedCash();
            return Ok(result.Data);

        }
        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateCountryDto updateCountryDto)
        {
            var result = await _countryCashedService.Update(updateCountryDto);
            if (result.Errors.Any())
                return Conflict();
            RemoveRelatedCash();
            return Ok();

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            var result = await _countryCashedService.Delete(id);
            if (result.Errors.Any())
                return Conflict();
            RemoveRelatedCash();
            return Ok();

        }
    }
}