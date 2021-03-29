using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.Currencies;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : AbstractEmployeePolicyController
    {
        public CurrencyController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }

        [HttpGet]
        //public IActionResult GetALl([FromQuery]PagingDto pagingDto)
        public IActionResult GetALl()
        {
            //var total = 1000000;
            //var data = mapper.Map<CurrencyDto[]>(this.Context.Currencies.Include(c => c.Incomes).Include(c => c.OutComes).Skip((pagingDto.Page - 1) * pagingDto.RowCount).Take(pagingDto.RowCount));

            //return Ok(new { total, data = data });
            //return Ok(mapper.Map<CurrencyDto[]>(this.Context.Currencies.Include(c => c.Incomes).Include(c => c.OutComes).ToList()));
            return Ok();
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
        [HttpPatch]
        public IActionResult Update([FromBody]UpdateCurrency updateCurrency)
        {
            try
            {
                var currency = this.Context.Currencies.Find(updateCurrency.Id);
                var similer = this.Context.Currencies.Where(c => c.Name == updateCurrency.Name && c.Id != updateCurrency.Id).Any();
                if (similer)
                    return Conflict();
                currency.Name = updateCurrency.Name;
                this.Context.Update(currency);
                this.Context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var currency = this.Context.Currencies.Find(id);
            this.Context.Remove(currency);
            this.Context.SaveChanges();
            return Ok();
        }

    }
}