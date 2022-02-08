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

        private readonly IAgentCashRepository _agentCashRepository;
        private readonly IClientCahedRepository _clientCahedRepository;
        private readonly ICountryCashedService _countryCashedService;
        public CountryController(KokazContext context, IMapper mapper, Logging logging, IAgentCashRepository agentCashRepository, IClientCahedRepository clientCahedRepository, ICountryCashedService countryCashedService) : base(context, mapper, logging)
        {
            _agentCashRepository = agentCashRepository;
            _clientCahedRepository = clientCahedRepository;
            _countryCashedService = countryCashedService;
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
                return Ok(result.Data);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
        //[HttpPatch]
        //public async Task<IActionResult> Update([FromBody] UpdateCountryDto updateCountryDto)
        //{
        //    var country = this._context.Countries.Find(updateCountryDto.Id);
        //    var similarCountry = this._context.Countries.Where(c => c.Name == updateCountryDto.Name && c.Id != updateCountryDto.Id).Any();
        //    if (similarCountry)
        //        return Conflict();

        //    country.Name = updateCountryDto.Name;
        //    country.DeliveryCost = updateCountryDto.DeliveryCost;
        //    country.MediatorId = updateCountryDto.MediatorId;
        //    country.Points = updateCountryDto.Points;
        //    await _countryCashedRepository.Update(country);
        //    await _countryCashedRepository.RefreshCash();
        //    await _agentCashRepository.RefreshCash();
        //    await _clientCahedRepository.RefreshCash();
        //    return Ok();
        //}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _countryCashedService.Delete(id);
                if (result.Errors.Any())
                    return Conflict();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        //[HttpPut("SetMain/{id}")]
        //public async Task<IActionResult> SetIsMain(int id)
        //{
        //    var country = this._context.Countries.Find(id);
        //    var mainCountry = this._context.Countries.Where(c => c.IsMain == true).ToList();
        //    mainCountry.ForEach(c =>
        //    {
        //        c.IsMain = false;
        //    });
        //    country.IsMain = true;
        //    mainCountry.Add(country);
        //    await _countryCashedRepository.Update(mainCountry);
        //    await _countryCashedRepository.RefreshCash();
        //    await _agentCashRepository.RefreshCash();
        //    return Ok();
        //}
    }
}