using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var country = this.Context.Countries.Find(createOrdersFromEmployee.CountryId);
            var dbContextTransaction = this.Context.Database.BeginTransaction();
            try
            {
                var order = mapper.Map<CreateOrdersFromEmployee, Order>(createOrdersFromEmployee);
                if (createOrdersFromEmployee.RegionId == null)
                {
                    var region = new Region()
                    {
                        Name = createOrdersFromEmployee.RegionName,
                        CountryId = createOrdersFromEmployee.CountryId
                    };
                    this.Context.Add(region);
                    this.Context.SaveChanges();
                    order.RegionId = region.Id;
                    order.Seen = true;
                }
                order.DeliveryCost = country.DeliveryCost;
                this.Context.Add(order);
                this.Context.SaveChanges();

                if (createOrdersFromEmployee.OrderTypeDtos != null)
                {

                    foreach (var item in createOrdersFromEmployee.OrderTypeDtos)
                    {
                        int orderId;
                        if (item.OrderTypeId != null)
                        {
                            orderId = (int)item.OrderTypeId;
                        }
                        else
                        {
                            OrderType orderType = new OrderType()
                            {
                                Name = item.OrderTypeName
                            };
                            this.Context.Add(orderType);
                            this.Context.SaveChanges();
                            orderId = orderType.Id;
                        }
                        OrderItem orderItem = new OrderItem()
                        {
                            OrderId = order.Id,
                            Count = item.Count,
                            OrderTpyeId = orderId
                        };
                        this.Context.Add(orderItem);
                        this.Context.SaveChanges();
                    }
                }

                dbContextTransaction.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                dbContextTransaction.Rollback();
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("createMultiple")]
        public IActionResult Create([FromBody]List<CreateMultipleOrder> createMultipleOrders)
        {

            foreach (var item in createMultipleOrders)
            {
                var order = mapper.Map<Order>(item);
                var country = this.Context.Countries.Find(order.CountryId);
                order.Seen = true;
                order.MoenyPlacedId = (int)MoneyPalcedEnum.OutSideCompany;
                order.DeliveryCost = country.DeliveryCost;
                this.Context.Add(order);
            }
            this.Context.SaveChanges();
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var order = this.Context.Orders.Find(id);
                this.Context.Remove(order);
                this.Context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public IActionResult Get([FromQuery] PagingDto pagingDto, [FromQuery]OrderFilter orderFilter)
        {
            var orderIQ = this.Context.Orders
                .AsQueryable();
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
                orderIQ = orderIQ.Where(c => c.RecipientName.StarسtsWith(orderFilter.RecipientName));
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
            var orders = orderIQ.Skip((pagingDto.Page - 1) * pagingDto.RowCount).Take(pagingDto.RowCount)
                .Include(c => c.Client)
                .Include(c => c.Agent)
                .Include(c => c.Region)
                .Include(c => c.Country)
                .Include(c => c.Orderplaced)
                .Include(c => c.MoenyPlaced)
                .ToList();
            return Ok(new { data = mapper.Map<OrderDto[]>(orders), total });
        }
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
        [HttpGet("NewOrders")]
        public IActionResult GetNewOrders()
        {
            var orders = this.Context.Orders
                .Include(c => c.Client)
                .Include(c => c.Agent)
                .Include(c => c.Region)
                .Include(c => c.Country)
                .Include(c => c.Orderplaced)
                .Include(c => c.MoenyPlaced)
                .Where(c => c.Seen == null)
                .ToList();
            return Ok(mapper.Map<OrderDto[]>(orders));
        }
        [HttpPut("Accept/{id}")]
        public IActionResult Accept(int id)
        {
            var order = this.Context.Orders.Find(id);
            order.Seen = true;
            this.Context.Update(order);
            this.Context.SaveChanges();
            return Ok();
        }
        [HttpPut("DisAccept/{id}")]
        public IActionResult DisAccept(int id)
        {
            var order = this.Context.Orders.Find(id);
            order.Seen = false;
            this.Context.Update(order);
            this.Context.SaveChanges();
            return Ok();
        }
        [HttpPut]
        public IActionResult MakeOrderInWay(int[] ids)
        {
            var orders = this.Context.Orders.Where(c => ids.Contains(c.Id)).ToList();
            if (orders.Any(c => c.OrderplacedId != (int)OrderplacedEnum.Store))
            {
                return Conflict();
            }
            foreach (var item in orders)
            {
                item.OrderplacedId = (int)OrderplacedEnum.Way;
                this.Context.Update(orders);
            }
            this.Context.SaveChanges();
            return Ok();
        }

    }
}