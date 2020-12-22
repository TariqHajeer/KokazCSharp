using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KokazGoodsTransfer.Dtos.OrdersTypes;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderTypeController : ControllerBase
    {
        KokazContext Context;
        public OrderTypeController(KokazContext context)
        {
            this.Context = context;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var orderTypes = Context.OrderTypes
                .Include(c=>c.ClientOrderTypes)
                .ToList();
            List<OrderTypeDto> orderTypeDtos = new List<OrderTypeDto>();
            foreach (var item in orderTypes)
            {
                orderTypeDtos.Add(new OrderTypeDto()
                {
                    Id = item.Id,
                    Name = item.Name,
                    CanDelete = item.ClientOrderTypes.ToList().Count == 0
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
    }
}