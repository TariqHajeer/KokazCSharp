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
            try
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

                if (createOrdersFromEmployee.OrderTypeDtos != null)
                {
                    foreach (var item in createOrdersFromEmployee.OrderTypeDtos)
                    {
                        OrderItem orderItem = new OrderItem()
                        {
                            OrderId = order.Id,
                            Count = item.Count,
                            OrderTpyeId = (int)item.OrderTypeId
                        };
                        this.Context.Add(orderItem);
                    }
                }
                this.Context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult Get([FromQuery] PagingDto pagingDto, [FromQuery]OrderFilter orderFilter)
        {
            var orderIQ = this.Context.Orders.AsQueryable();
            if (orderFilter.CountryId != null)
            {
                orderIQ = orderIQ.Where(c => c.CountryId == orderFilter.CountryId);
            }
            if (orderFilter.Code != string.Empty && orderFilter.Code != null)
            {
                orderIQ = orderIQ.Where(c => c.Code.StartsWith(orderFilter.Code));
            }
            if (orderFilter.ClientId != null)
            {
                orderIQ = orderIQ.Where(c => c.ClientId == orderFilter.ClientId);
            }
            if (orderFilter.RegionId != null)
            {
                orderIQ = orderIQ.Where(c => c.RegionId == orderFilter.RegionId);
            }
            if (orderFilter.RecipientName != string.Empty && orderFilter.RecipientName != null)
            {
                orderIQ = orderIQ.Where(c => c.RecipientName.StartsWith(orderFilter.RecipientName));
            }
            if (orderFilter.MonePlacedId != null)
            {
                orderIQ = orderIQ.Where(c => c.MoenyPlacedId == orderFilter.MonePlacedId);
            }
            if (orderFilter.OrderplacedId != null)
            {
                orderIQ = orderIQ.Where(c => c.OrderplacedId == orderFilter.OrderplacedId);
            }
            if (orderFilter.Phone != string.Empty && orderFilter.Phone != null)
            {
                orderIQ = orderIQ.Where(c => c.RecipientPhones.Contains(orderFilter.Phone));
            }
            var total = orderIQ.Count();
            var orders = orderIQ.Skip((pagingDto.Page - 1) * pagingDto.RowCount).Take(pagingDto.RowCount).ToList();
            return Ok(new { data = mapper.Map<OrderDto[]>(orders), total });
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
        [HttpGet("chekcCode")]
        public IActionResult CheckCode([FromQuery] string code, int clientid)
        {
            return Ok(this.Context.Orders.Where(c => c.ClientId == clientid && c.Code == code).Any());
        }

    }
}