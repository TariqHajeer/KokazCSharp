using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Dtos.MarketDtos;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : OldAbstractController
    {
        private readonly ICountryCashedService _countryCashedService;
        public HomeController(KokazContext context, IMapper mapper, ICountryCashedService countryCashedService) : base(context, mapper)
        {
            _countryCashedService = countryCashedService;
        }
        [HttpGet("Country")]
        public async Task<IActionResult> GetCountry() => Ok(await _countryCashedService.GetCashed());
        [HttpGet("Market")]
        public IActionResult GetMarket()
        {
            List<MarketDto> markets = new List<MarketDto>();
            foreach (var item in this._context.Markets.Where(c => c.IsActive == true).ToList())
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
                    var client = this._context.Clients.Find(item.ClientId);
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
            var orders = this._context.Orders
                .Include(c => c.Orderplaced)
                .Include(c => c.Country)
                .Include(c => c.Client)
                .ThenInclude(c => c.ClientPhones)
                .Where(c => c.Code == code);
            if (!String.IsNullOrEmpty(phone))
            {
                orders = orders.Where(c => c.RecipientPhones.Contains(phone));
            }
            var dto = _mapper.Map<OrderDto[]>(orders).ToList();
            dto.ForEach(c =>
            {

                var country = this._context.Countries.Find(c.Country.Id);
                c.Path = _mapper.Map<CountryDto[]>(GetPath(country, null)).ToList();
            });
            return Ok(dto);
        }

        List<Country> GetPath(Country country, List<Country> countries = null)
        {
            if (country.MediatorId != null)
            {
                var mid = this._context.Countries.Find(country.MediatorId);
                countries = GetPath(mid, countries);

            }
            if (countries == null)
                countries = new List<Country>();
            countries.Add(country);
            return countries;
        }

    }
}