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
    public class OrderTypeController : OldAbstractEmployeePolicyController
    {
        public OrderTypeController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var orderTypes = _context.OrderTypes
                .Include(c => c.OrderItems)
                .ToList();
            return Ok(_mapper.Map<OrderTypeDto[]>(orderTypes));
        }
        [HttpPost]
        public IActionResult Create([FromBody] CreateOrderType orderTypeDto)
        {
            var similerOrderType = this._context.OrderTypes.Where(c => c.Name.Equals(orderTypeDto.Name)).FirstOrDefault();
            if (similerOrderType != null)
                return Conflict();

            OrderType orderType = new OrderType()
            {
                Name = orderTypeDto.Name
            };

            _context.Add(orderType);
            _context.SaveChanges();
            OrderTypeDto response = new OrderTypeDto()
            {
                Id = orderType.Id,
                Name = orderType.Name,
                CanDelete = true
            };
            return Ok(response);
        }
        [HttpPatch]
        public IActionResult Update([FromBody] UpdateOrderTypeDto updateOrderTypeDto)
        {
            var orderType = this._context.OrderTypes.Find(updateOrderTypeDto.Id);
            if (orderType == null)
                return NotFound();
            if (this._context.OrderTypes.Where(c => c.Id != updateOrderTypeDto.Id && c.Name == updateOrderTypeDto.Name).Any())
                return Conflict();
            orderType.Name = updateOrderTypeDto.Name;
            this._context.Update(orderType);
            this._context.SaveChanges();
            return Ok();

        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var orderType = this._context.OrderTypes
                .Include(c => c.OrderItems)
                .Where(c => c.Id == id).SingleOrDefault();
            if (orderType == null)
                return NotFound();
            this._context.Entry(orderType).Collection(c => c.OrderItems).Load();
            if (orderType.OrderItems.Count() != 0)
                return Conflict();
            this._context.Remove(orderType);
            this._context.SaveChanges();
            return Ok();


        }

    }
}