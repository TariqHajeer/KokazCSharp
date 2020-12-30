using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.OutComeDtos;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutComeController : AbstractEmployeePolicyController
    {
        public OutComeController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
        [HttpPost]
        public IActionResult Create([FromBody] CreateOutComeDto createOutComeDto)
        {
            try
            {
                var outCome = mapper.Map<OutCome>(createOutComeDto);
                outCome.UserId = AuthoticateUserId();
                this.Context.Add(outCome);
                this.Context.SaveChanges();
                return Ok();
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

        //[HttpGet]
        //public IActionResult Get([FromQuery] )
        //{
        //    return Ok();
        //}
    }
}