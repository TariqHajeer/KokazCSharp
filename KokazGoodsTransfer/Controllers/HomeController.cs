using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : AbstractController
    {
        public HomeController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
        [HttpGet("Country")]
        public IActionResult GetCountry()
        {
            var countries = Context.Countries
                .ToList();
            return Ok(mapper.Map<CountryDto[]>(countries));
        }
    }
}