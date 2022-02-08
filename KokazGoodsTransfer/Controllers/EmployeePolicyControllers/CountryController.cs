using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
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
        public CountryController(KokazContext context, IMapper mapper, Logging logging, ICountryCashedService countryCashedService, IRegionCashedService regionCashedService) : base(context, mapper, logging)
        {
            _countryCashedService = countryCashedService;
            _regionCashedService = regionCashedService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetAll() => Ok(await _countryCashedService.GetCashed());


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCountryDto createCountryDto)
        {
            try
            {
                var result = await _countryCashedService.AddAsync(createCountryDto);
                if (result.Errors.Any())
                    return Conflict();
                await _regionCashedService.RefreshCash();
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateCountryDto updateCountryDto)
        {
            try
            {
                var result = await _countryCashedService.Update(updateCountryDto);
                if (result.Errors.Any())
                    return Conflict();
                await _regionCashedService.RefreshCash();
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _countryCashedService.Delete(id);
                if (result.Errors.Any())
                    return Conflict();
                await _regionCashedService.RefreshCash();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPut("SetMain/{id}")]
        public async Task<IActionResult> SetIsMain(int id)
        {
            try
            {
                await _countryCashedService.SetMainCountry(id);
                await _regionCashedService.RefreshCash();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}