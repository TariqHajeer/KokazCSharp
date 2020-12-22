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
    public class CountryController : ControllerBase
    {
        KokazContext Context;
        public CountryController(KokazContext context)
        {
            this.Context = context;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var countries = Context.Countries
                .Include(c => c.Users)
                .Include(c => c.Clients)
                .ToList();
            List<CountryDto> countryDtos = new List<CountryDto>();
            foreach (var item in countries)
            {
                countryDtos.Add(new CountryDto()
                {
                    Id = item.Id,
                    Name = item.Name,
                    CanDelete = item.Clients.Count() == 0&&item.Users.Count()==0
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
            Context.SaveChanges();
            var countryDto = new CountryDto()
            {
                Id = country.Id,
                Name = country.Name,
                CanDelete = true
            };
            return Ok(countryDto);
        }

    }
}