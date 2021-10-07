using AutoMapper;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("Order")]
        public IActionResult GetOrder()
        {
            var orders = this.Context.Orders.Where(c => c.AgentId == AuthoticateUserId()).ToList();
            return Ok(mapper.Map<OrderDto[]>(orders));
        }
    }
}
