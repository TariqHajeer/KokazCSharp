using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Countries;
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
        public CountryController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var countries = Context.Countries
                .Include(c=>c.Clients)
                .Include(c => c.Regions)
                .Include(c=>c.AgentCountrs)
                    .ThenInclude(c=>c.Agent)
                .ToList();
            return Ok(mapper.Map<CountryDto[]>(countries));
        }

        [HttpPost]
        public IActionResult Create([FromBody]CreateCountryDto createCountryDto)
        {
            var similer = Context.Countries.Where(c => c.Name == createCountryDto.Name).FirstOrDefault();
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
            if (this.Context.Countries.Count() == 0)
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
            Context.Add(country);
            Context.SaveChanges();
            return Ok(mapper.Map<CountryDto>(country));
        }
        [HttpPatch]
        public IActionResult Update([FromBody] UpdateCountryDto updateCountryDto)
        {
            var country = this.Context.Countries.Find(updateCountryDto.Id);
            var similarCountry = this.Context.Countries.Where(c => c.Name == updateCountryDto.Name && c.Id != updateCountryDto.Id).Any();
            if (similarCountry)
                return Conflict();
            country.Name = updateCountryDto.Name;
            country.DeliveryCost = updateCountryDto.DeliveryCost;
            country.MediatorId = updateCountryDto.MediatorId;
            country.Points = updateCountryDto.Points;
            this.Context.Update(country);   
            this.Context.SaveChanges();
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                //test find instade of that
                var country = this.Context.Countries
                    .Include(c=>c.Clients)
                    .Include(c => c.Regions)
                    .Include(c=>c.AgentCountrs)
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
                //if (country.Regions.Any(c => c.Clients.Any()))
                //{
                //    return Conflict();
                //}
                foreach (var item in country.Regions)
                {
                    this.Context.Regions.Remove(item);
                }
                this.Context.Countries.Remove(country);
                this.Context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPut("SetMain/{id}")]
        public IActionResult SetIsMain(int id )
        {
            var country = this.Context.Countries.Find(id);
            var mainCountry = this.Context.Countries.Where(c => c.IsMain == true).ToList();
            mainCountry.ForEach(c =>
            {
                c.IsMain = false;
            });
            country.IsMain = true;
            this.Context.SaveChanges();
            return Ok();
        }
    }
}