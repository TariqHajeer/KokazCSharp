using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.IncomesDtos;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeController : AbstractEmployeePolicyController
    {
        [HttpGet]
        public IActionResult Get([FromQuery]IncomeFiltering filtering)
        {
            var incomeIQ = (IQueryable<Income>)this.Context.Incomes
                   .Include(c => c.User)
                   .Include(c => c.IncomeType);
            if (filtering.MaxAmount != null)
                incomeIQ = incomeIQ.Where(c => c.Amount <= filtering.MaxAmount);
            if (filtering.MinAmount != null)
                incomeIQ = incomeIQ.Where(c => c.Amount >= filtering.MaxAmount);
            if (filtering.IncomeTypeId != null)
                incomeIQ = incomeIQ.Where(c => c.IncomeTypeId == filtering.IncomeTypeId);
            if (filtering.UserId != null)
                incomeIQ = incomeIQ.Where(c => c.UserId == filtering.UserId);
            if (filtering.FromDate != null)
                incomeIQ = incomeIQ.Where(c => c.Date >= filtering.FromDate);
            if (filtering.ToDate != null)
                incomeIQ = incomeIQ.Where(c => c.Date <= filtering.ToDate);
            return Ok(mapper.Map<IncomeDto[]>(incomeIQ.ToList()));
        }
        public IncomeController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
        [HttpPost]
        public IActionResult Create([FromBody] CreateIncomeDto creatrIncomeDto)
        {
            try
            {
                var income = mapper.Map<Income>(creatrIncomeDto);
                income.UserId = AuthoticateUserId();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPost("AddMultiple")]
        public IActionResult Create([FromBody]IList<CreateIncomeDto> createIncomeDtos)
        {
            try
            {
                var userId = AuthoticateUserId();
                foreach (var item in createIncomeDtos)
                {
                    var incmoe = mapper.Map<Income>(item);
                    incmoe.UserId = userId;
                    this.Context.Add(incmoe);
                }
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