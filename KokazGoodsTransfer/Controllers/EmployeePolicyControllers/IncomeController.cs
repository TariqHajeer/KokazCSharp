using System.Collections.Generic;
using System.Threading.Tasks;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.IncomesDtos;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Interfaces;
using LinqKit;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeController : AbstractEmployeePolicyController
    {
        private readonly IIncomeService _IncomeService;
        public IncomeController(IIncomeService incomeService)
        {
            _IncomeService = incomeService;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Filtering filtering, [FromQuery] PagingDto pagingDto)
        {
            var incomePredicate = PredicateBuilder.New<Income>(true);
            if (filtering.MaxAmount != null)
                incomePredicate = incomePredicate.And(c => c.Amount <= filtering.MaxAmount);
            if (filtering.MinAmount != null)
                incomePredicate = incomePredicate.And(c => c.Amount >= filtering.MinAmount);
            if (filtering.Type != null)
                incomePredicate = incomePredicate.And(c => c.IncomeTypeId == filtering.Type);
            if (filtering.UserId != null)
                incomePredicate = incomePredicate.And(c => c.UserId == filtering.UserId);
            if (filtering.FromDate != null)
                incomePredicate = incomePredicate.And(c => c.Date >= filtering.FromDate);
            if (filtering.ToDate != null)
                incomePredicate = incomePredicate.And(c => c.Date <= filtering.ToDate);
            var pagingResualt = await _IncomeService.GetAsync(pagingDto, incomePredicate);
            return Ok(pagingResualt);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _IncomeService.GetById(id));
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateIncomeDto creatrIncomeDto)
        {
            var result = await _IncomeService.AddAsync(creatrIncomeDto);
            if (result.Sucess)
                return Ok(result.Data);
            return Conflict();

        }
        [HttpPost("AddMultiple")]
        public async Task<IActionResult> Create([FromBody] IList<CreateIncomeDto> createIncomeDtos)
        {
            await _IncomeService.AddRangeAsync(createIncomeDtos);
            return Ok();

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _IncomeService.Delete(id);
            return Ok();
        }
        [HttpPatch]
        public async Task<IActionResult> UpdateIncome([FromBody] UpdateIncomeDto dto)
        {
            var income = (await _IncomeService.Update(dto)).Data;
            return Ok(income);
        }
    }
}