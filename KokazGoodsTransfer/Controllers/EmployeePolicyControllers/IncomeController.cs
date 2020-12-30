using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.IncomesDtos;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeController : AbstractEmployeePolicyController
    {
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
        //[HttpGet]
        //public IActionResult Get()
        //{

        //}
    }
}