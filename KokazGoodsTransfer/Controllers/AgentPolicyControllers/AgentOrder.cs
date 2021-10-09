using AutoMapper;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Controllers.AgentPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentOrderController : AbstractAgentController
    {
        public AgentOrderController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
        //[HttpGet("Order")]
        //public IActionResult GetOrder()
        //{
        //    var orders = this.Context.Orders.Where(c => c.AgentId == AuthoticateUserId()).ToList();
        //    return Ok(mapper.Map<OrderDto[]>(orders));
        //}
        [HttpGet("InStock")]
        public IActionResult GetInStock()
        {
           var orders= this.Context.Orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Store && c.AgentId == AuthoticateUserId())
                .ToList();
            return Ok(mapper.Map<OrderDto[]>(orders));
        }
        [HttpGet("InWay")]
        public IActionResult GetInWay()
        {
            var orders = this.Context.Orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Way && c.AgentId == AuthoticateUserId())
                 .ToList();
            return Ok(mapper.Map<OrderDto[]>(orders));
        }
    }
}
