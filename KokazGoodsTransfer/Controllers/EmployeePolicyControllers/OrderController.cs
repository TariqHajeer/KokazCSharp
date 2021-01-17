using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : AbstractEmployeePolicyController
    {
        public OrderController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
        [HttpPost]
        public IActionResult Create([FromBody] CreateOrdersFromEmployee createOrdersFromEmployee)
        {
            var order = mapper.Map<CreateOrdersFromEmployee, Order>(createOrdersFromEmployee);
            if (createOrdersFromEmployee.RegionId == null)
            {
                var region = new Region()
                {
                    Name = createOrdersFromEmployee.RecipientName,
                    CountryId = createOrdersFromEmployee.CountryId
                };
                this.Context.Add(region);
                order.RegionId = region.Id;
            }
            this.Context.Add(order);
            this.Context.SaveChanges();
            return Ok();
        }
        [HttpGet]
        public IActionResult Get([FromQuery] PagingDto pagingDto,[FromQuery]OrderFilter orderFilter)
        {
            var order = this.Context.Orders.AsQueryable();
            if (orderFilter.CountryId != null)
            {
                order = order.Where(c => c.CountryId == orderFilter.CountryId);
            }
            if (orderFilter.Code !=string.Empty)
            {
                order = order.Where(c => c.Code.StartsWith(orderFilter.Code));
            }
            if (orderFilter.ClientId != null)
            {
                order = order.Where(c => c.ClientId==orderFilter.ClientId);
            }
            if (orderFilter.RegionId != null)
            {
                order = order.Where(c => c.RegionId == orderFilter.RegionId);
            }
            if (orderFilter.RecipientName != string.Empty)
            {
                order = order.Where(c => c.RecipientName.StartsWith(orderFilter.RecipientName));
            }
            
            return Ok();
        }
        //[HttpPost]
        //public IActionResult Creat()
        //{
        //    return Ok();
        //}
        [HttpGet("orderPlace")]
        public IActionResult GetOrderPalce()
        {
            return Ok(mapper.Map<NameAndIdDto[]>(this.Context.OrderPlaceds.ToList()));
        }
        [HttpGet("MoenyPlaced")]
        public IActionResult GetMoenyPlaced()
        {
            return Ok(mapper.Map<NameAndIdDto[]>(this.Context.MoenyPlaceds.ToList()));
        }

    }
}