using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.OrdersTypes;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderTypeController : AbstractController
    {
        public OrderTypeController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var orderTypes = Context.OrderTypes
                .ToList();
            List<OrderTypeDto> orderTypeDtos = new List<OrderTypeDto>();
            foreach (var item in orderTypes)
            {
                orderTypeDtos.Add(new OrderTypeDto()
                {
                    Id = item.Id,
                    Name = item.Name,
                    CanDelete = true
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