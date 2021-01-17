using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.OutComeDtos;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutComeController : AbstractEmployeePolicyController
    {
        public OutComeController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
        [HttpGet]
        public IActionResult Get([FromQuery] Filtering filtering, [FromQuery]PagingDto pagingDto)
        {
            var outComeIQ = (IQueryable<OutCome>)this.Context.OutComes
                .Include(c => c.User)
                .Include(c => c.OutComeType)
                .Include(c => c.Currency);
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
            if (filtering.CurrencyId != null)
                outComeIQ = outComeIQ.Where(c => c.CurrencyId == filtering.CurrencyId);
            var total = outComeIQ.Count();
            var outComes = outComeIQ.Skip((pagingDto.Page - 1) * pagingDto.RowCount).Take(pagingDto.RowCount).ToList();

            return Ok(new { data = mapper.Map<OutComeDto[]>(outComes), total });
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
        public IActionResult Create([FromBody] CreateOutComeDto createOutComeDto)
        {
            try
            {
                var outCome = mapper.Map<OutCome>(createOutComeDto);
                outCome.UserId = AuthoticateUserId();
                this.Context.Add(outCome);
                this.Context.SaveChanges();
                this.Context.Entry(outCome).Reference(c => c.User).Load();
                this.Context.Entry(outCome).Reference(c => c.Currency).Load();
                this.Context.Entry(outCome).Reference(c => c.OutComeType).Load();
                return Ok(mapper.Map<OutComeDto>(outCome));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPost("CreateMulitpleOutCome")]
        public ActionResult CreateMultiple([FromBody]IList<CreateOutComeDto> createOutComeDtos)
        {
            try
            {
                var userId = AuthoticateUserId();
                foreach (var item in createOutComeDtos)
                {
                    var outCome = mapper.Map<OutCome>(item);
                    outCome.UserId = userId;
                    this.Context.Add(outCome);
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