using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.ClientDtos;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CStaticsController : AbstractClientPolicyController
    {
        public CStaticsController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
        [HttpGet]
        public IActionResult Get()
        {
            var  orders= this.Context.Orders.Where(c => c.ClientId == AuthoticateUserId()); 

            StaticsDto staticsDto = new StaticsDto();
            staticsDto.TotalOrder = orders.Count();
            staticsDto.OrderWithClient = orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Client).Count();

            return Ok(staticsDto);
        }
    }
}