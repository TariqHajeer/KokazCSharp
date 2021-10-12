using AutoMapper;
using KokazGoodsTransfer.Dtos.AgentDtos;
using KokazGoodsTransfer.Dtos.Common;
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
            var orders = this.Context.Orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Store && c.AgentId == AuthoticateUserId())
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
        [HttpGet("OrderSuspended")]
        public IActionResult OrderSuspended()
        {
            var orders = this.Context.Orders.Where(c => c.OrderStateId == (int)OrderStateEnum.Processing && (c.OrderplacedId >= (int)OrderplacedEnum.Way || c.OrderplacedId == (int)OrderplacedEnum.Delayed) && c.AgentId == AuthoticateUserId())
                 .ToList();
            return Ok(mapper.Map<OrderDto[]>(orders));
        }
        [HttpGet("Print")]
        public IActionResult GetPrint([FromQuery] PagingDto pagingDto, PrintFilterDto printFilterDto)
        {
            var printeds = this.Context.Printeds.Where(c => c.Type == PrintType.Agent);
            if (printFilterDto.Date != null)
            {
                printeds = printeds.Where(c => c.Date == printFilterDto.Date);
            }
            if (printFilterDto.Number != null)
            {
                printeds = printeds.Where(c => c.PrintNmber == printFilterDto.Number);
            }
            printeds = printeds.Where(c => c.OrderPrints.Any(c => c.Order.AgentId == AuthoticateUserId()));
            var total = printeds.Count();
            var list = printeds.Skip(pagingDto.Page - 1).Take(pagingDto.RowCount * pagingDto.Page).ToList();
            return Ok(new { total, Data = mapper.Map<PrintDto[]>(list) });
        }
    }
}
