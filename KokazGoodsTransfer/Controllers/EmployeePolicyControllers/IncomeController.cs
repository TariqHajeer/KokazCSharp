using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.IncomesDtos;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeController : AbstractEmployeePolicyController
    {
        private readonly IIncomeService _IncomeService;
        public IncomeController(KokazContext context, IMapper mapper, Logging logging, IIncomeService incomeService) : base(context, mapper, logging)
        {
            _IncomeService = incomeService;
        }
        [HttpGet]
        public IActionResult Get([FromQuery] Filtering filtering, [FromQuery] PagingDto pagingDto)
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _IncomeService.GetById(id));
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateIncomeDto creatrIncomeDto)
        {
            try
            {
                var result = await _IncomeService.AddAsync(creatrIncomeDto);
                if (result.Sucess)
                return Ok(result.Data);
                return Conflict();
            }
            catch (Exception ex)
            {
                _logging.WriteExption(ex);
                return BadRequest();
            }
        }
        [HttpPost("AddMultiple")]
        public async Task<IActionResult> Create([FromBody] IList<CreateIncomeDto> createIncomeDtos)
        {
            try
            {
                await _IncomeService.AddRangeAsync(createIncomeDtos);
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
        public IActionResult UpdateIncome([FromBody] UpdateIncomeDto dto)
        {
            var income = this._context.Incomes.Find(dto.Id);
            income = _mapper.Map<UpdateIncomeDto, Income>(dto, income);
            this._context.Update(income);
            this._context.SaveChanges();
            return Ok(income);
        }
    }
}