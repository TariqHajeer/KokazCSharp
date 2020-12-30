using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using KokazGoodsTransfer.Dtos.OrdersTypes;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderTypeController : AbstractEmployeePolicyController
    {
        public OrderTypeController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var orderTypes = Context.OrderTypes
                .Include(c=>c.OrderOrderTypes)
                .ToList();
            List<OrderTypeDto> orderTypeDtos = new List<OrderTypeDto>();
            foreach (var item in orderTypes)
            {
                orderTypeDtos.Add(new OrderTypeDto()
                {
                    Id = item.Id,
                    Name = item.Name,
                    CanDelete = item.OrderOrderTypes.Count()==0
                });

            }
            return Ok(orderTypeDtos);
        }
        [HttpPost]
        public IActionResult Create(CreateOrderType orderTypeDto)
        {
            var similerOrderType = this.Context.OrderTypes.Where(c => c.Name.Equals(orderTypeDto.Name));
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
        //[HttpPatch]
        //public IActionResult 
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var orderType = this.Context.OrderTypes
                    .Include(c => c.OrderOrderTypes)
                    .Where(c => c.Id == id).SingleOrDefault();
                if (orderType == null)
                    return NotFound();
                this.Context.Remove(orderType);
                this.Context.SaveChanges();
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
            
        }

    }
}