using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace KokazGoodsTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : AbstractController
    {
        public CountryController(KokazContext context) : base(context)
        {
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var countries = Context.Countries
                .Include(c => c.Users)
                .Include(c => c.Regions)
                .ToList();
            List<CountryDto> countryDtos = new List<CountryDto>();
            foreach (var item in countries)
            {
                countryDtos.Add(new CountryDto()
                {
                    Id = item.Id,
                    Name = item.Name,
                    CanDelete = item.Regions.Count() == 0 && item.Users.Count() == 0,
                    Regions = item.Regions.Select(c => c.Name).ToList()
                });
            }
            return Ok(countryDtos);
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
            Context.Add(country);

            if (createCountryDto.Regions != null)
                foreach (var item in createCountryDto.Regions)
                {
                    var region = new Region()
                    {
                        Name = item,
                        CountryId = country.Id,
                    };
                    Context.Add(region);
                }
            Context.SaveChanges();
            var countryDto = new CountryDto()
            {
                Id = country.Id,
                Name = country.Name,
                CanDelete = true
            };
            return Ok(countryDto);
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
        //[HttpDelete("{id}"]
        //public IActionResult Delte(int id)
        //{
        //    var country = this.Context.Countries.Find(id);
        //    if (country == null)
        //        return NotFound();
        //    if(country.Regions.Count()>0||country.Users.Count())
        //    return Ok();
        //}
    }
}