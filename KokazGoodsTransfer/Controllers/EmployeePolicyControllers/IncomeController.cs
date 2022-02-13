using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.IncomesDtos;
using KokazGoodsTransfer.Helpers;
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
        public IncomeController(KokazContext context, IMapper mapper, Logging logging) : base(context, mapper,logging)
        {
        }
        [HttpGet]
        public IActionResult Get([FromQuery]Filtering filtering, [FromQuery]PagingDto pagingDto)
        {
            try
            {
                var incomeIQ = (IQueryable<Income>)this._context.Incomes
                       .Include(c => c.User)
                       .Include(c => c.IncomeType);
                if (filtering.MaxAmount != null)
                    incomeIQ = incomeIQ.Where(c => c.Amount <= filtering.MaxAmount);
                if (filtering.MinAmount != null)
                    incomeIQ = incomeIQ.Where(c => c.Amount >= filtering.MinAmount);
                if (filtering.Type != null)
                    incomeIQ = incomeIQ.Where(c => c.IncomeTypeId == filtering.Type);
                if (filtering.UserId != null)
                    incomeIQ = incomeIQ.Where(c => c.UserId == filtering.UserId);
                if (filtering.FromDate != null)
                    incomeIQ = incomeIQ.Where(c => c.Date >= filtering.FromDate);
                if (filtering.ToDate != null)
                    incomeIQ = incomeIQ.Where(c => c.Date <= filtering.ToDate);
                var totla = incomeIQ.Count();
                var incomes = incomeIQ.Skip((pagingDto.Page - 1) * pagingDto.RowCount).Take(pagingDto.RowCount).ToList();
                return Ok(new { data = _mapper.Map<IncomeDto[]>(incomeIQ.ToList()), totla });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateIncomeDto creatrIncomeDto)
        {
            try
            {
                var income = _mapper.Map<Income>(creatrIncomeDto);
                income.UserId = AuthoticateUserId();
                this._context.Add(income);
                this._context.SaveChanges();
                this._context.Entry(income).Reference(c => c.User).Load();
                this._context.Entry(income).Reference(c => c.IncomeType).Load();
                return Ok(_mapper.Map<IncomeDto>(income));
            }
            catch (Exception ex)
            {
                _logging.WriteExption(ex);
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
                    var incmoe = _mapper.Map<Income>(item);
                    incmoe.UserId = userId;
                    this._context.Add(incmoe);
                }
                this._context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                _logging.WriteExption(ex);
                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var income = this._context.Incomes.Find(id);
            this._context.Remove(income);
            this._context.SaveChanges();
            return Ok();
        }
        [HttpPatch]
        public IActionResult UpdateIncome([FromBody]UpdateIncomeDto dto)
        {
            var income = this._context.Incomes.Find(dto.Id);
            income = _mapper.Map<UpdateIncomeDto, Income>(dto, income);
            this._context.Update(income);
            this._context.SaveChanges();
            return Ok(income);
        }
    }
}