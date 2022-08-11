using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public CountryController(KokazContext context, IMapper mapper, ICountryCashedService countryCashedService, IRegionCashedService regionCashedService, IUserCashedService userCashedService, IClientCashedService clientCashedService) : base(context, mapper)
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
        [HttpPut("SetMain/{id}")]
        public async Task<IActionResult> SetIsMain(int id)
        {

            await _countryCashedService.SetMainCountry(id);
            RemoveRelatedCash();
            return Ok();

        }
    }
}