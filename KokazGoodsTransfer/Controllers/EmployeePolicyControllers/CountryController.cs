using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : AbstractEmployeePolicyController
    {
        private readonly ICountryCashedRepository _countryCashedRepository;
        private readonly IAgentCashRepository _agentCashRepository;
        private readonly IClientCahedRepository _clientCahedRepository;
        public CountryController(KokazContext context, IMapper mapper, Logging logging, ICountryCashedRepository countryCashedRepository, IAgentCashRepository agentCashRepository, IClientCahedRepository clientCahedRepository) : base(context, mapper, logging)
        {
            _countryCashedRepository = countryCashedRepository;
            _agentCashRepository = agentCashRepository;
            _clientCahedRepository = clientCahedRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var countries = await _countryCashedRepository.GetAll();
            return Ok(_mapper.Map<CountryDto[]>(countries));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCountryDto createCountryDto)
        {
            var similer = _context.Countries.Where(c => c.Name == createCountryDto.Name).FirstOrDefault();
            if (similer != null)
                return Conflict();
            var country = new Country()
            {
                Name = createCountryDto.Name,
                DeliveryCost = createCountryDto.DeliveryCost,
                MediatorId = createCountryDto.MediatorId,
                IsMain = false,
                Points = createCountryDto.Points
            };
            if (this._context.Countries.Count() == 0)
            {
                country.IsMain = true;
            }

            if (createCountryDto.Regions != null)
                foreach (var item in createCountryDto.Regions)
                {
                    country.Regions.Add(new Region()
                    {
                        Name = item
                    });
                }
            await _countryCashedRepository.AddAsync(country);

            return Ok(_mapper.Map<CountryDto>(country));
        }
        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateCountryDto updateCountryDto)
        {
            var country = this._context.Countries.Find(updateCountryDto.Id);
            var similarCountry = this._context.Countries.Where(c => c.Name == updateCountryDto.Name && c.Id != updateCountryDto.Id).Any();
            if (similarCountry)
                return Conflict();

            country.Name = updateCountryDto.Name;
            country.DeliveryCost = updateCountryDto.DeliveryCost;
            country.MediatorId = updateCountryDto.MediatorId;
            country.Points = updateCountryDto.Points;
            await _countryCashedRepository.Update(country);
            await _countryCashedRepository.RefreshCash();
            await _agentCashRepository.RefreshCash();
            await _clientCahedRepository.RefreshCash();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var country = this._context.Countries
                    .Include(c => c.Clients)
                    .Include(c => c.Regions)
                    .Include(c => c.AgentCountrs)
                    .Where(c => c.Id == id)
                    .SingleOrDefault();

                if (country == null)
                    return NotFound();
                if (country.AgentCountrs.Any())
                    return Conflict();
                if (country.Clients.Count() > 0)
                {
                    return Conflict();
                }
                foreach (var item in country.Regions)
                {
                    this._context.Regions.Remove(item);
                }
                await _countryCashedRepository.Delete(country);
                await _countryCashedRepository.RefreshCash();
                await _agentCashRepository.RefreshCash();
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
            var country = this._context.Countries.Find(id);
            var mainCountry = this._context.Countries.Where(c => c.IsMain == true).ToList();
            mainCountry.ForEach(c =>
            {
                c.IsMain = false;
            });
            country.IsMain = true;
            mainCountry.Add(country);
            await _countryCashedRepository.Update(mainCountry);
            await _countryCashedRepository.RefreshCash();
            await _agentCashRepository.RefreshCash();
            return Ok();
        }
    }
}