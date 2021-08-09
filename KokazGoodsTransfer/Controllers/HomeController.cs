using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Dtos.MarketDtos;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            foreach (var item in this.Context.Markets.Where(c => c.IsActive == true).ToList())
            {
                var temp = new MarketDto()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    IsActive = (bool)item.IsActive,
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
        [HttpGet("TrackOrder")]
        public IActionResult TrackingOrder([FromQuery] string code, string phone)
        {
            var orders = this.Context.Orders
                .Include(c => c.Orderplaced)
                .Where(c => c.Code == code);
            if (!String.IsNullOrEmpty(phone))
            {
                orders = orders.Where(c => c.RecipientPhones.Contains(phone));
            }
            return Ok(mapper.Map<OrderDto[]>(orders));
        }
        //List<Country> GetPath(Country country, List<Country> countries =null)
        //{
        //    if (country.MediatorId != null)
        //    {
        //        var mid = this.Context.Countries.Find(country.MediatorId);
        //        GetPath(mid, countries);
        //    }
        //    countries.Add(country);
        //}
    }
}