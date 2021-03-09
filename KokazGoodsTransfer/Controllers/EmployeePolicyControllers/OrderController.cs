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
                    order.AgentCost = this.Context.Users.Find(order.AgentId).Salary ?? 0;
                }
                order.DeliveryCost = country.DeliveryCost;
                if (order.MoenyPlacedId == (int)MoneyPalcedEnum.Delivered)
                {
                    order.IsClientDiliverdMoney = true;
                }
                else
                {
                    order.IsClientDiliverdMoney = false;
                }
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
                order.IsClientDiliverdMoney = false;
                order.OrderStateId = (int)OrderStateEnum.Processing;
                order.AgentCost = this.Context.Users.Find(order.AgentId).Salary ?? 0;
                order.Date = DateTime.Now;
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
            if (orderFilter.AgentId != null)
            {
                orderIQ = orderIQ.Where(c => c.AgentId == orderFilter.AgentId);
            }
            if (orderFilter.IsClientDiliverdMoney != null)
            {
                orderIQ = orderIQ.Where(c => c.IsClientDiliverdMoney == orderFilter.IsClientDiliverdMoney);
            }
            var total = orderIQ.Count();
            var orders = orderIQ.Skip((pagingDto.Page - 1) * pagingDto.RowCount).Take(pagingDto.RowCount)
                .Include(c => c.Client)
                    .ThenInclude(c => c.ClientPhones)
                .Include(c => c.Agent)
                    .ThenInclude(c => c.UserPhones)
                .Include(c => c.Region)
                .Include(c => c.Country)
                .Include(c => c.Orderplaced)
                .Include(c => c.MoenyPlaced)
                .Include(c => c.OrderItems)
                    .ThenInclude(c => c.OrderTpye)
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
        [HttpPut("MakeOrderInWay")]
        public IActionResult MakeOrderInWay(int[] ids)
        {
            var orders = this.Context.Orders.Where(c => ids.Contains(c.Id)).ToList();
            if (orders.Any(c => c.OrderplacedId != (int)OrderplacedEnum.Store))
            {
                return Conflict();
            }
            var printNumber = this.Context.Orders.Max(c => c.AgentPrintNumber) ?? 0;
            ++printNumber;
            foreach (var item in orders)
            {
                item.OrderplacedId = (int)OrderplacedEnum.Way;
                item.AgentPrintNumber = printNumber;
                this.Context.Update(item);
            }

            this.Context.SaveChanges();
            return Ok(new { printNumber });
        }
        [HttpPut("UpdateOrdersStatusFromAgent")]
        public IActionResult UpdateOrdersStatusFromAgent(List<OrderStateDto> orderStates)
        {

            foreach (var item in orderStates)
            {
                var order = this.Context.Orders.Find(item.Id);
                order.OrderplacedId = item.OrderplacedId;
                order.MoenyPlacedId = item.MoenyPlacedId;
                if (order.IsClientDiliverdMoney == true)
                {
                    switch (order.OrderplacedId)
                    {
                        case (int)OrderplacedEnum.Delivered:
                            {
                                order.OrderStateId = (int)OrderStateEnum.Finished;
                            }
                            break;
                        case (int)OrderplacedEnum.CompletelyReturned:
                            {
                                order.OldCost = order.Cost;
                                order.Cost = 0;
                                order.AgentCost = 0;
                                order.OrderStateId = (int)OrderStateEnum.ShortageOfCash;
                            }
                            break;
                        case (int)OrderplacedEnum.Unacceptable:
                            {
                                order.OldCost = order.Cost;
                                order.Cost = 0;
                                order.OrderStateId = (int)OrderStateEnum.ShortageOfCash;
                            }
                            break;
                        case (int)OrderplacedEnum.PartialReturned:
                            {
                                if (order.OldCost == null)
                                    order.OldCost = order.Cost;
                                order.Cost = item.Cost;
                                order.OrderStateId = (int)OrderStateEnum.ShortageOfCash;
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (order.OrderplacedId)
                    {

                        case (int)OrderplacedEnum.CompletelyReturned:
                            {
                                if (order.OldCost != null)
                                    order.OldCost = order.Cost;
                                order.Cost = 0;
                                order.AgentCost = 0;
                            }
                            break;
                        case (int)OrderplacedEnum.Unacceptable:
                            {
                                if (order.OldCost != null)
                                    order.OldCost = order.Cost;
                                order.Cost = 0;
                            }
                            break;
                        case (int)OrderplacedEnum.PartialReturned:
                            {
                                if (order.OldCost == null)
                                    order.OldCost = order.Cost;
                                order.Cost = item.Cost;
                            }
                            break;
                        default:
                            break;
                    }
                }
                this.Context.Update(order);
            }
            this.Context.SaveChanges();
            return Ok();
        }
        [HttpPut("DeleiverMoneyForClient")]
        public IActionResult DeleiverMoneyForClient(int[] ids)
        {

            var orders = this.Context.Orders.Where(c => ids.Contains(c.Id));
            var printNumber = this.Context.Orders.Max(c => c.ClientPrintNumber) ?? 0;
            ++printNumber;
            foreach (var item in orders)
            {
                item.IsClientDiliverdMoney = true;
                item.ClientPrintNumber = printNumber;
                if (item.MoenyPlacedId == (int)MoneyPalcedEnum.InsideCompany||item.OrderplacedId>(int)OrderplacedEnum.Way)
                {
                    item.OrderStateId = (int)OrderStateEnum.Finished;
                }
                this.Context.Update(item);
            }
            this.Context.SaveChanges();
            return Ok(new { printNumber });
        }
        [HttpGet("GetOrderByAgent/{agentId}/{orderCode}")]
        public IActionResult GetOrderByAgent(int agentId, string orderCode)
        {
            var order = this.Context.Orders.Where(c => c.AgentId == agentId && c.Code == orderCode).SingleOrDefault();
            if (order == null)
                return Conflict(new { message = "الشحنة غير موجودة" });
            if ((order.MoenyPlacedId > (int)MoneyPalcedEnum.WithAgent))
            {
                return Conflict(new { message = "تم إستلام الشحنة مسبقاً" });
            }
            if (order.OrderplacedId == (int)OrderplacedEnum.CompletelyReturned || order.OrderplacedId == (int)OrderplacedEnum.Unacceptable)
            {
                return Conflict(new { message = "تم إستلام الشحنة مسبقاً" });
            }
            this.Context.Entry(order).Reference(c => c.Orderplaced).Load();
            this.Context.Entry(order).Reference(c => c.MoenyPlaced).Load();
            this.Context.Entry(order).Reference(c => c.Region).Load();
            this.Context.Entry(order).Reference(c => c.Country).Load();
            return Ok(mapper.Map<OrderDto>(order));
        }
        [HttpGet("GetEarnings")]
        public IActionResult GetEarnings([FromQuery] PagingDto pagingDto, [FromQuery] DateFiter dateFiter)
        {
            var ordersQuery = this.Context.Orders.Where(c => c.OrderStateId == (int)OrderStateEnum.Finished && c.OrderplacedId != (int)OrderplacedEnum.CompletelyReturned);
            if (dateFiter.FromDate != null)
                ordersQuery = ordersQuery.Where(c => c.Date >= dateFiter.FromDate);
            if (dateFiter.ToDate != null)
                ordersQuery = ordersQuery.Where(c => c.Date <= dateFiter.ToDate);
            var totalRecord = ordersQuery.Count();
            var totalEarinig = ordersQuery.Sum(c => c.DeliveryCost - c.AgentCost);
            var orders = ordersQuery.Skip((pagingDto.Page - 1) * pagingDto.RowCount).Take(pagingDto.RowCount).ToList();
            return Ok(new { data = new { orders = mapper.Map<OrderDto[]>(orders), totalEarinig }, total = totalRecord });
        }
        [HttpGet("ShipmentsNotReimbursedToTheClient/{clientId}")]
        public IActionResult ShipmentsNotReimbursedToTheClient(int clientId)
        {
            var orders = this.Context.Orders.Where(c => c.ClientId == clientId && c.IsClientDiliverdMoney == false && c.OrderplacedId >= (int)OrderplacedEnum.Way)
                 .Include(c => c.Agent)
                     .ThenInclude(c => c.UserPhones)
                 .Include(c => c.Region)
                 .Include(c => c.Country)
                 .Include(c => c.Orderplaced)
                 .Include(c => c.MoenyPlaced).ToList();
            return Ok(mapper.Map<OrderDto[]>(orders));
        }
        [HttpGet("ShortageOfCash")]
        public IActionResult GetShortageOfCash([FromQuery] int clientId)
        {
            var orders = this.Context.Orders.Where(c => c.OrderStateId == (int)OrderStateEnum.ShortageOfCash && c.ClientId == clientId)
                .Include(c => c.Client)
                    .ThenInclude(c => c.ClientPhones)
                .Include(c => c.Agent)
                    .ThenInclude(c => c.UserPhones)
                .Include(c => c.Region)
                .Include(c => c.Country)
                .Include(c => c.Orderplaced)
                .Include(c => c.MoenyPlaced)
                .Include(c => c.OrderItems)
                    .ThenInclude(c => c.OrderTpye)
                .ToList();
            return Ok(mapper.Map<OrderDto[]>(orders));
        }
        [HttpPut("ReiveMoneyFromClient")]
        public IActionResult ReiveMoneyFromClient([FromBody] int[] ids)
        {
            var orders = this.Context.Orders.Where(c => ids.Contains(c.Id)).ToList();
            orders.ForEach(c => c.OrderStateId = (int)OrderStateEnum.Finished);
            this.Context.SaveChanges();
            return Ok();
        }
        [HttpGet("GetOrderByAgnetPrintNumber")]
        public IActionResult GetOrderByAgnetPrintNumber([FromQuery] int printNumber)
        {
           var orders= this.Context.Orders.Where(c=>c.AgentPrintNumber==printNumber)
                .Include(c => c.Client)
                    .ThenInclude(c => c.ClientPhones)
                .Include(c => c.Agent)
                    .ThenInclude(c => c.UserPhones)
                .Include(c => c.Region)
                .Include(c => c.Country)
                .Include(c => c.Orderplaced)
                .Include(c => c.MoenyPlaced)
                .Include(c => c.OrderItems)
                    .ThenInclude(c => c.OrderTpye)
                .ToList();
            return Ok( mapper.Map<OrderDto[]>(orders));
        }
        [HttpGet("GetOrderByClientPrintNumber")]
        public IActionResult GetOrderByClientPrintNumber([FromQuery] int printNumber)
        {
            var orders = this.Context.Orders.Where(c => c.ClientPrintNumber== printNumber)
                .Include(c => c.Client)
                    .ThenInclude(c => c.ClientPhones)
                .Include(c => c.Agent)
                    .ThenInclude(c => c.UserPhones)
                .Include(c => c.Region)
                .Include(c => c.Country)
                .Include(c => c.Orderplaced)
                .Include(c => c.MoenyPlaced)
                .Include(c => c.OrderItems)
                    .ThenInclude(c => c.OrderTpye)
                .ToList();
            return Ok(mapper.Map<OrderDto[]>(orders));
        }
        #region oldPrintNumber
        //[HttpGet("GetClientPrintNumber")]
        //public IActionResult GetClientPrintNumber()
        //{
        //    var x = this.Context.Orders.Max(c => c.ClientPrintNumber) ?? 0;
        //    if (x == 0)
        //        return Ok(1);
        //    return Ok(x + 1);
        //}
        //[HttpGet("GetAgentPrintNumber")]
        //public IActionResult GetAgnetPrintNumber()
        //{
        //    var x = this.Context.Orders.Max(c => c.AgentPrintNumber) ?? 0;
        //    if (x == 0)
        //        return Ok(1);
        //    return Ok(x + 1);
        //}
        //[HttpPut("SetClientPrintNumber")]
        //public IActionResult SetClientPrintNumber([FromBody]PrintNumberOrder printNumberOrder)
        //{
        //    var orders = this.Context.Orders.Where(c => printNumberOrder.OrderId.Contains(c.Id));
        //    foreach (var item in orders)
        //    {
        //        item.ClientPrintNumber = printNumberOrder.PrintNumber;
        //    }
        //    this.Context.SaveChanges();
        //    return Ok();
        //}
        //[HttpPut("SetAgentPrintNumber")]
        //public IActionResult SetAgentPrintNumber([FromBody]PrintNumberOrder printNumberOrder)
        //{
        //    var orders = this.Context.Orders.Where(c => printNumberOrder.OrderId.Contains(c.Id));
        //    foreach (var item in orders)
        //    {
        //        item.AgentPrintNumber = printNumberOrder.PrintNumber;
        //    }
        //    this.Context.SaveChanges();
        //    return Ok();
        //}
        #endregion

    }
}