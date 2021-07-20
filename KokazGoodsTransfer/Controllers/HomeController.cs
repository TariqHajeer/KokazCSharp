using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Dtos.MarketDtos;
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
        [HttpGet("Market")]
        public IActionResult GetMarket()
        {
            List<MarketDto> markets = new List<MarketDto>();
            foreach (var item in this.Context.Markets.Where(c=>c.IsActive==true).ToList())
            {
                var temp = new MarketDto()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    IsActive = item.IsActive,
                    MarketUrl = item.MarketUrl,
                    LogoPath = "MarketLogo/" + item.LogoPath
                };
                if (item.ClientId != null)
                {
                    var client = this.Context.Clients.Find(item.ClientId);
                    temp.Client = new Dtos.Clients.ClientDto()
                    {
                        Name = client.Name,
                        Id = client.Id
                    };
                }
                markets.Add(temp);

            }
            return Ok(markets);
        }
    }
}