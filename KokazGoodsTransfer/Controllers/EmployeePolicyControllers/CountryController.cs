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
                .Include(c => c.Users)
                .Include(c => c.Regions)
                    .ThenInclude(c => c.Clients)
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
                Name = createCountryDto.Name
            };

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
            country.Name = updateCountryDto.Name;
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
                    .Include(c => c.Regions)
                    .ThenInclude(c => c.Clients)
                    .Where(c => c.Id == id)
                    .SingleOrDefault();

                if (country == null)
                    return NotFound();
                if (country.Users.Any())
                    return Conflict();
                if (country.Regions.Any(c => c.Clients.Any()))
                {
                    return Conflict();
                }
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

    }
}