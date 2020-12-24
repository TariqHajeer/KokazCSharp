
using System.Collections.Generic;
using System.Linq;
using KokazGoodsTransfer.Dtos.Currencies;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        KokazContext Context;
        public CurrencyController(KokazContext context)
        {
            Context = context;
        }
        [HttpPost]
        public IActionResult Create([FromBody] CreateCurrencyDto createCurrency)
        {
            var similer = Context.Countries.Where(c => c.Name == createCurrency.Name).FirstOrDefault();
            if (similer != null)
            {
                return Conflict();
            }
            Currency currency = new Currency()
            {
                Name = createCurrency.Name
            };
            this.Context.Add(currency);
            this.Context.SaveChanges();
            CurrencyDto currencyDto = new CurrencyDto()
            {
                Name = currency.Name,
                Id = currency.Id,
                CanDelete = true
            };
            return Ok(currencyDto);
        }
        [HttpGet]
        public IActionResult GetALl()
        {
            var currencies = this.Context.Currencies.ToList();
            List<CurrencyDto> currencyDtos = new List<CurrencyDto>();
            foreach (var item in currencies)
            {
                currencyDtos.Add(new CurrencyDto()
                {
                    Name = item.Name,
                    Id = item.Id,
                    CanDelete =true
                });
            }
            return Ok(currencyDtos);
        } 
    }
}