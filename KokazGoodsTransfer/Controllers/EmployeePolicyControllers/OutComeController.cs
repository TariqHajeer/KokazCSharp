using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.OutComeDtos;
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
    public class OutComeController : AbstractEmployeePolicyController
    {
        private readonly IOutcomeService _outcomeService;
        public OutComeController(KokazContext context, IMapper mapper, IOutcomeService outcomeService) : base(context, mapper)
        {
            _outcomeService = outcomeService;
        }
        [HttpGet]
        public IActionResult Get([FromQuery] Filtering filtering, [FromQuery] PagingDto pagingDto)
        {
            var outComeIQ = (IQueryable<OutCome>)this._context.OutComes
                .Include(c => c.User)
                .Include(c => c.OutComeType);
            if (filtering.MaxAmount != null)
                outComeIQ = outComeIQ.Where(c => c.Amount <= filtering.MaxAmount);
            if (filtering.MinAmount != null)
                outComeIQ = outComeIQ.Where(c => c.Amount >= filtering.MaxAmount);
            if (filtering.Type != null)
                outComeIQ = outComeIQ.Where(c => c.OutComeTypeId == filtering.Type);
            if (filtering.UserId != null)
                outComeIQ = outComeIQ.Where(c => c.UserId == filtering.UserId);
            if (filtering.FromDate != null)
                outComeIQ = outComeIQ.Where(c => c.Date >= filtering.FromDate);
            if (filtering.ToDate != null)
                outComeIQ = outComeIQ.Where(c => c.Date <= filtering.ToDate);
            var total = outComeIQ.Count();
            var outComes = outComeIQ.Skip((pagingDto.Page - 1) * pagingDto.RowCount).Take(pagingDto.RowCount).ToList();

            return Ok(new { data = _mapper.Map<OutComeDto[]>(outComes), total });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _outcomeService.GetById(id));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="createOutComeDto"></param>
        /// <returns></returns>
        /// <example>
        // {
        //   "amount": 10000,
        //   "currencyId":2004 ,
        //   "date": "2021-01-10T12:47:58.194Z",
        //   "reason": "string",
        //   "note": "string",
        //   "outComeTypeId": 2055
        //  }
        /// </example>

        [HttpPost]
        public async Task<ActionResult<OutComeDto>> Create([FromBody] CreateOutComeDto createOutComeDto)
        {

            var result = await _outcomeService.AddAsync(createOutComeDto);
            if (result.Sucess)
                return Ok(result.Data);
            return Conflict();

        }
        [HttpPost("CreateMulitpleOutCome")]
        public async Task<IActionResult> CreateMultiple([FromBody] IList<CreateOutComeDto> createOutComeDtos)
        {

            await _outcomeService.AddRangeAsync(createOutComeDtos);
            return Ok();

        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var outCome = this._context.OutComes.Find(id);
            this._context.Remove(outCome);
            this._context.SaveChanges();
            return Ok();
        }
        [HttpPatch]
        public IActionResult Update([FromBody] UpdateOuteComeDto dto)
        {
            var outcome = this._context.OutComes.Find(dto.Id);
            outcome = _mapper.Map<UpdateOuteComeDto, OutCome>(dto, outcome);
            this._context.Update(outcome);
            this._context.SaveChanges();
            return Ok(outcome);
        }
    }
}