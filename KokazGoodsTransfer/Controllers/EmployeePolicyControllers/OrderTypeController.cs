using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using KokazGoodsTransfer.Dtos.OrdersTypes;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderTypeController : AbstractEmployeePolicyController
    {
        public OrderTypeController(KokazContext context, IMapper mapper, Logging logging) : base(context, mapper,logging)
        {
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var orderTypes = Context.OrderTypes
                .Include(c => c.OrderItems)
                .ToList();
            return Ok(mapper.Map<OrderTypeDto[]>(orderTypes));
        }
        [HttpPost]
        public IActionResult Create([FromBody] CreateOrderType orderTypeDto)
        {
            var similerOrderType = this.Context.OrderTypes.Where(c => c.Name.Equals(orderTypeDto.Name)).FirstOrDefault();
            if (similerOrderType != null)
                return Conflict();

            OrderType orderType = new OrderType()
            {
                Name = orderTypeDto.Name
            };

            Context.Add(orderType);
            Context.SaveChanges();
            OrderTypeDto response = new OrderTypeDto()
            {
                Id = orderType.Id,
                Name = orderType.Name,
                CanDelete = true
            };
            return Ok(response);
        }
        [HttpPatch]
        public IActionResult Update([FromBody]UpdateOrderTypeDto updateOrderTypeDto)
        {
            try
            {
                var orderType = this.Context.OrderTypes.Find(updateOrderTypeDto.Id);
                if (orderType == null)
                    return NotFound();
                if (this.Context.OrderTypes.Where(c => c.Id != updateOrderTypeDto.Id && c.Name == updateOrderTypeDto.Name).Any())
                    return Conflict();
                orderType.Name = updateOrderTypeDto.Name;
                this.Context.Update(orderType);
                this.Context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var orderType = this.Context.OrderTypes
                    .Include(c => c.OrderItems)
                    .Where(c => c.Id == id).SingleOrDefault();
                if (orderType == null)
                    return NotFound();
                this.Context.Entry(orderType).Collection(c => c.OrderItems).Load();
                if (orderType.OrderItems.Count() != 0)
                    return Conflict();
                this.Context.Remove(orderType);
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