﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Dtos.NotifcationDtos;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.HubsConfig;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Static;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using KokazGoodsTransfer.Services.Interfaces;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : AbstractEmployeePolicyController
    {
        private readonly IIndexService<MoenyPlaced> _moneyPlacedIndexService;
        private readonly IIndexService<OrderPlaced> _orderPlacedIndexService;
        ErrorMessage err;
        NotificationHub notificationHub;
        static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        public OrderController(KokazContext context, IMapper mapper, NotificationHub notificationHub, Logging logging, IIndexService<MoenyPlaced> moneyPlacedIndexService, IIndexService<OrderPlaced> orderPlacedIndexService) : base(context, mapper, logging)
        {
            this.err = new ErrorMessage();
            this.err.Controller = "Order";
            this.notificationHub = notificationHub;
            _moneyPlacedIndexService = moneyPlacedIndexService;
            _orderPlacedIndexService = orderPlacedIndexService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagingDto pagingDto, [FromQuery] OrderFilter orderFilter)
        {
            try
            {

                var orderIQ = this._context.Orders
                    .Include(c => c.Client)
                    .Include(c => c.Agent)
                    .Include(c => c.Region)
                    .Include(c => c.Country)
                    .Include(c => c.Orderplaced)
                    .Include(c => c.MoenyPlaced)
                    .Include(c => c.OrderPrints)
                        .ThenInclude(c => c.Print)
                    .Include(c => c.AgentOrderPrints)
                        .ThenInclude(c => c.AgentPrint)
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
                if (orderFilter.ClientPrintNumber != null)
                {
                    orderIQ = orderIQ.Where(c => c.OrderPrints.Any(op => op.Print.PrintNmber == orderFilter.ClientPrintNumber && op.Print.Type == PrintType.Client));
                }
                if (orderFilter.AgentPrintNumber != null)
                {
                    orderIQ = orderIQ.Where(c => c.AgentOrderPrints.Any(c => c.AgentPrint.Id == orderFilter.AgentPrintNumber));
                }
                if (orderFilter.CreatedDate != null)
                {
                    orderIQ = orderIQ.Where(c => c.Date == orderFilter.CreatedDate);
                }
                if (orderFilter.Note != "" && orderFilter.Note != null)
                {
                    orderIQ = orderIQ.Where(c => c.Note.Contains(orderFilter.Note));
                }
                if (orderFilter.AgentPrintStartDate != null)
                {
                    ///TODO :
                    ///chould check this query 
                    orderIQ = orderIQ.Where(c => c.AgentOrderPrints.Select(c => c.AgentPrint).OrderBy(c => c.Id).LastOrDefault().Date >= orderFilter.AgentPrintStartDate);

                }
                if (orderFilter.AgentPrintEndDate != null)
                {
                    ///TODO :
                    ///chould check this query 
                    orderIQ = orderIQ.Where(c => c.AgentOrderPrints.Select(c => c.AgentPrint).OrderBy(c => c.Id).LastOrDefault().Date <= orderFilter.AgentPrintEndDate);
                }
                var total = await orderIQ.CountAsync();
                var orders = await orderIQ.Skip((pagingDto.Page - 1) * pagingDto.RowCount).Take(pagingDto.RowCount)
                    .ToListAsync();
                return Ok(new { data = _mapper.Map<OrderDto[]>(orders), total });
            }
            catch (Exception ex)
            {
                _logging.WriteExption(ex);
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult Create([FromBody] CreateOrdersFromEmployee createOrdersFromEmployee)
        {
            var country = this._context.Countries.Find(createOrdersFromEmployee.CountryId);
            var dbContextTransaction = this._context.Database.BeginTransaction();
            try
            {
                var order = _mapper.Map<CreateOrdersFromEmployee, Order>(createOrdersFromEmployee);
                order.CurrentCountry = this._context.Countries.Where(c => c.IsMain == true).FirstOrDefault().Id;
                order.CreatedBy = AuthoticateUserName();
                if (this._context.Orders.Where(c => c.Code == order.Code && c.ClientId == order.ClientId).Any())
                {
                    this.err.Messges.Add($"الكود{order.Code} مكرر");
                    return Conflict(err);
                }

                if (createOrdersFromEmployee.RegionId == null)
                {
                    var region = new Region()
                    {
                        Name = createOrdersFromEmployee.RegionName,
                        CountryId = createOrdersFromEmployee.CountryId
                    };
                    this._context.Add(region);
                    this._context.SaveChanges();
                    order.RegionId = region.Id;
                    order.Seen = true;

                    order.AgentCost = this._context.Users.Find(order.AgentId).Salary ?? 0;
                }
                order.OrderStateId = (int)OrderStateEnum.Processing;
                order.DeliveryCost = country.DeliveryCost;
                if (order.MoenyPlacedId == (int)MoneyPalcedEnum.Delivered)
                {
                    order.IsClientDiliverdMoney = true;

                }
                else
                {
                    order.IsClientDiliverdMoney = false;
                }

                this._context.Add(order);
                this._context.SaveChanges();

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
                            this._context.Add(orderType);
                            this._context.SaveChanges();
                            orderId = orderType.Id;
                        }
                        OrderItem orderItem = new OrderItem()
                        {
                            OrderId = order.Id,
                            Count = item.Count,
                            OrderTpyeId = orderId
                        };
                        this._context.Add(orderItem);
                        this._context.SaveChanges();
                    }
                }

                dbContextTransaction.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                dbContextTransaction.Rollback();
                _logging.WriteExption(ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        public IActionResult Edit([FromBody] UpdateOrder updateOrder)
        {

            try
            {
                var order = this._context.Orders.Find(updateOrder.Id);
                OrderLog log = order;
                this._context.Add(log);
                if (order.Code != updateOrder.Code)
                {
                    if (this._context.Orders.Any(c => c.ClientId == order.ClientId && c.Code == updateOrder.Code))
                    {
                        this.err.Messges.Add($"الكود{order.Code} مكرر");
                        return Conflict(err);
                    }
                }
                order.Code = updateOrder.Code;

                if (order.AgentId != updateOrder.AgentId)
                {
                    order.OrderStateId = (int)OrderStateEnum.Processing;
                    order.MoenyPlacedId = (int)MoneyPalcedEnum.OutSideCompany;
                    order.OrderplacedId = (int)OrderplacedEnum.Store;
                }
                if (order.ClientId != updateOrder.ClientId)
                {
                    if (order.IsClientDiliverdMoney)
                    {
                        order.IsClientDiliverdMoney = false;
                        Receipt receipt = new Receipt()
                        {
                            IsPay = true,
                            ClientId = order.ClientId,
                            Amount = ((order.Cost - order.DeliveryCost) * -1),
                            CreatedBy = "النظام",
                            Manager = "",
                            Date = DateTime.Now,
                            About = "",
                            Note = " بعد تعديل طلب بكود " + order.Code,
                        };
                        this._context.Add(receipt);
                    }
                }
                order.DeliveryCost = updateOrder.DeliveryCost;
                order.Cost = updateOrder.Cost;
                order.ClientId = updateOrder.ClientId;
                order.AgentId = updateOrder.AgentId;
                order.CountryId = updateOrder.CountryId;
                order.RegionId = updateOrder.RegionId;
                order.Address = updateOrder.Address;
                order.RecipientName = updateOrder.RecipientName;
                order.RecipientPhones = String.Join(",", updateOrder.RecipientPhones);
                order.Note = updateOrder.Note;
                this._context.Update(order);
                this._context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                _logging.WriteExption(ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("createMultiple")]
        public async Task<IActionResult> Create([FromBody] List<CreateMultipleOrder> createMultipleOrders)
        {
            var transaction = this._context.Database.BeginTransaction();
            try
            {
                foreach (var item in createMultipleOrders)
                {
                    var isExisit = await this._context.Orders.Where(c => c.Code == item.Code && c.ClientId == item.ClientId).AnyAsync();
                    if (isExisit)
                    {
                        transaction.Rollback();

                        this.err.Messges.Add($"الكود{item.Code} مكرر");
                        return Conflict(this.err);
                    }
                    var order = _mapper.Map<Order>(item);
                    var country = await this._context.Countries.FindAsync(order.CountryId);
                    order.Seen = true;
                    order.MoenyPlacedId = (int)MoneyPalcedEnum.OutSideCompany;
                    order.IsClientDiliverdMoney = false;
                    order.OrderStateId = (int)OrderStateEnum.Processing;
                    order.AgentCost = this._context.Users.Find(order.AgentId).Salary ?? 0;
                    order.Date = item.Date;
                    order.OrderplacedId = (int)OrderplacedEnum.Store;
                    order.CurrentCountry = this._context.Countries.Where(c => c.IsMain == true).FirstOrDefault().Id;
                    order.CreatedBy = AuthoticateUserName();
                    this._context.Add(order);
                    await this._context.SaveChangesAsync();
                }
                await transaction.CommitAsync();
                return Ok();
            }
            catch (Exception ex)
            {

                transaction.Rollback();
                _logging.WriteExption(ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var order = this._context.Orders.Find(id);
                this._context.Remove(order);
                this._context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                _logging.WriteExption(ex);
                return BadRequest();
            }
        }
        [HttpGet("WithoutPaging")]
        public async Task<IActionResult> Get([FromQuery] OrderFilter orderFilter)
        {
            try
            {
                var orderIQ = this._context.Orders
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
                    .Include(c => c.OrderPrints)
                        .ThenInclude(c => c.Print)
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
                if (orderFilter.ClientPrintNumber != null)
                {
                    orderIQ = orderIQ.Where(c => c.OrderPrints.Any(op => op.Print.PrintNmber == orderFilter.ClientPrintNumber && op.Print.Type == PrintType.Client));
                }
                if (orderFilter.AgentPrintNumber != null)
                {
                    orderIQ = orderIQ.Where(c => c.AgentOrderPrints.Any(c => c.AgentPrint.Id == orderFilter.AgentPrintNumber));
                }
                if (orderFilter.CreatedDate != null)
                {
                    orderIQ = orderIQ.Where(c => c.Date.Value.Date == orderFilter.CreatedDate.Value.Date);
                }
                if (orderFilter.AgentPrintStartDate != null)
                {
                    ///TODO :
                    ///chould check this query 
                    orderIQ = orderIQ.Where(c => c.AgentOrderPrints.Select(c => c.AgentPrint).OrderBy(c => c.Id).LastOrDefault().Date >= orderFilter.AgentPrintStartDate);
                }
                if (orderFilter.AgentPrintEndDate != null)
                {
                    ///TODO :
                    ///chould check this query 
                    orderIQ = orderIQ.Where(c => c.AgentOrderPrints.Select(c => c.AgentPrint).OrderBy(c => c.Id).LastOrDefault().Date <= orderFilter.AgentPrintEndDate);
                }
                var total = await orderIQ.CountAsync();
                var orders = await orderIQ
                    .ToListAsync();
                return Ok(new { data = _mapper.Map<OrderDto[]>(orders), total });
            }
            catch (Exception ex)
            {
                _logging.WriteExption(ex);
                return BadRequest();
            }
        }
        [HttpGet("TrakingOrder")]
        public IActionResult Get([FromQuery] int agentId, int? nextCountry)
        {
            var orders = this._context.Orders
                .Include(c => c.Country)
                .Include(c => c.Client)
                .Where(c => c.AgentId == agentId && c.OrderplacedId == (int)OrderplacedEnum.Way && c.Country.MediatorId != null).AsQueryable();
            if (nextCountry != null)
            {
                orders = orders.Where(c => c.CurrentCountry != nextCountry);
            }
            var dto = _mapper.Map<OrderDto[]>(orders).ToList();

            if (nextCountry != null)
            {
                Dictionary<int, List<Country>> paths = new Dictionary<int, List<Country>>();
                dto.ForEach(c =>
                {
                    if (!paths.ContainsKey(c.Country.Id))
                    {
                        paths.Add(c.Country.Id, GetPath(this._context.Countries.Find(c.Country.Id)));
                    }
                    var path = paths[c.Country.Id];
                    bool test = false;
                    foreach (var item in path)
                    {
                        if (test)
                        {

                            c.NextCountryDto = _mapper.Map<CountryDto>(item);
                            break;
                        }
                        if (item.Id == c.CurrentCountry)
                        {
                            test = true;
                        }
                    }
                });
            }
            return Ok(dto);
        }
        [HttpPut("MoveToNextStep")]
        public IActionResult MoveToNextStep([FromBody] int[] ids)
        {
            var orders = this._context.Orders
                .Include(c => c.Country)
                .Where(c => ids.Contains(c.Id)).ToList();
            Dictionary<int, List<Country>> paths = new Dictionary<int, List<Country>>();
            foreach (var item in orders)
            {
                if (!paths.ContainsKey(item.CountryId))
                {
                    paths.Add(item.CountryId, GetPath(item.Country));
                }
                var path = paths[item.Country.Id];
                bool test = false;
                foreach (var country in path)
                {
                    if (test)
                    {
                        item.CurrentCountry = country.Id;
                    }
                    if (country.Id == item.CurrentCountry)
                    {
                        test = true;
                    }
                }
            }
            this._context.SaveChanges();
            return Ok();
        }
        List<Country> GetPath(Country country, List<Country> countries = null)
        {
            if (country.MediatorId != null)
            {
                var mid = this._context.Countries.Find(country.MediatorId);
                countries = GetPath(mid, countries);

            }
            if (countries == null)
                countries = new List<Country>();
            countries.Add(country);
            return countries;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var order = this._context.Orders
               .Include(c => c.Client)
                       .ThenInclude(c => c.ClientPhones)
                       .Include(c => c.Client)
                       .ThenInclude(c => c.Country)
                   .Include(c => c.Agent)
                       .ThenInclude(c => c.UserPhones)
                   .Include(c => c.Region)
                   .Include(c => c.Country)
                   .Include(c => c.Orderplaced)
                   .Include(c => c.MoenyPlaced)
                   .Include(c => c.OrderItems)
                       .ThenInclude(c => c.OrderTpye)
                   .Include(c => c.OrderPrints)
                       .ThenInclude(c => c.Print)
                   .Include(c => c.OrderLogs)
                   .Include(c => c.AgentOrderPrints)
                   .ThenInclude(c => c.AgentPrint)
               .FirstOrDefault(c => c.Id == id);
            return Ok(_mapper.Map<OrderDto>(order));
        }
        [HttpGet("OrdersDontFinished")]
        public IActionResult Get([FromQuery] OrderDontFinishedFilter orderDontFinishedFilter)
        {
            List<Order> orders = new List<Order>();
            if (orderDontFinishedFilter.ClientDoNotDeleviredMoney)
            {
                var list = this._context.Orders.Where(c => c.IsClientDiliverdMoney == false && c.ClientId == orderDontFinishedFilter.ClientId && orderDontFinishedFilter.OrderPlacedId.Contains(c.OrderplacedId))
                   .Include(c => c.Region)
                   .Include(c => c.Country)
                   .Include(c => c.MoenyPlaced)
                   .Include(c => c.Orderplaced)
                   .Include(c => c.Agent)
                   .Include(c => c.OrderPrints)
                    .ThenInclude(c => c.Print)
                   .ToList();
                if (list != null && list.Count() > 0)
                {
                    orders.AddRange(list);
                }
            }
            if (orderDontFinishedFilter.IsClientDeleviredMoney)
            {

                var list = this._context.Orders.Where(c => c.OrderStateId == (int)OrderStateEnum.ShortageOfCash && c.ClientId == orderDontFinishedFilter.ClientId && orderDontFinishedFilter.OrderPlacedId.Contains(c.OrderplacedId))
               .Include(c => c.Region)
               .Include(c => c.Country)
               .Include(c => c.Orderplaced)
               .Include(c => c.MoenyPlaced)
               .Include(c => c.Agent)
               .Include(c => c.OrderPrints)
                    .ThenInclude(c => c.Print)
               .ToList();
                if (list != null && list.Count() > 0)
                {
                    orders.AddRange(list);
                }
            }
            var o = _mapper.Map<PayForClientDto[]>(orders);
            return Ok(o);
        }
        [HttpGet("orderPlace")]
        public async Task<IActionResult> GetOrderPalce() => Ok(await _orderPlacedIndexService.GetAllLite());
        [HttpGet("MoenyPlaced")]
        public async Task<IActionResult> GetMoenyPlaced() => Ok(await _moneyPlacedIndexService.GetAllLite());
        [HttpGet("chekcCode")]
        public IActionResult CheckCode([FromQuery] string code, int clientid)
        {
            return Ok(this._context.Orders.Where(c => c.ClientId == clientid && c.Code == code).Any());
        }
        [HttpPost("CheckMulieCode/{clientId}")]
        public IActionResult CheckMulieCode(int clientId, [FromBody] string[] codes)
        {
            List<CodeStatus> codeStatuses = new List<CodeStatus>();
            var nonAvilableCode = this._context.Orders.Where(c => c.ClientId == clientId && codes.Contains(c.Code)).Select(c => c.Code).ToArray();
            codeStatuses.AddRange(codes.Except(nonAvilableCode).Select(c => new CodeStatus()
            {
                Code = c,
                Avilabe = true
            }));
            codeStatuses.AddRange(nonAvilableCode.Select(c => new CodeStatus()
            {
                Avilabe = false,
                Code = c
            }));

            return Ok(codeStatuses);
        }


        [HttpGet("NewOrders")]
        public async Task<IActionResult> GetNewOrders()
        {
            var orders = await this._context.Orders
                .Include(c => c.Client)
                .ThenInclude(c => c.ClientPhones)
                .Include(c => c.Client)
                .ThenInclude(c => c.Country)
                .Include(c => c.Region)
                .Include(c => c.Country)
                    .ThenInclude(c => c.AgentCountrs)
                        .ThenInclude(c => c.Agent)
                .Include(c => c.OrderItems)
                    .ThenInclude(c => c.OrderTpye)
                .Where(c => c.IsSend == true && c.OrderplacedId == (int)OrderplacedEnum.Client)
                .ToListAsync();
            return Ok(_mapper.Map<OrderDto[]>(orders));
        }
        [HttpGet("NewOrderDontSned")]
        public async Task<IActionResult> NewOrderDontSned()
        {
            var orders = await this._context.Orders
                .Include(c => c.Client)
                .ThenInclude(c => c.ClientPhones)
                .Include(c => c.Client)
                .ThenInclude(c => c.Country)
                .Include(c => c.Region)
                .Include(c => c.Country)
                    .ThenInclude(c => c.AgentCountrs)
                        .ThenInclude(c => c.Agent)
                .Include(c => c.OrderItems)
                    .ThenInclude(c => c.OrderTpye)
                .Where(c => c.IsSend == false && c.OrderplacedId == (int)OrderplacedEnum.Client)
                .ToListAsync();
            return Ok(_mapper.Map<OrderDto[]>(orders));
        }
        [HttpGet("OrderAtClient")]
        public async Task<IActionResult> OrderAtClient([FromQuery] OrderFilter orderFilter)
        {
            var orderIQ = this._context.Orders
                    .Include(c => c.Client)
                    .Include(c => c.Country)
                    .Include(c => c.Client)
                .ThenInclude(c => c.ClientPhones)
                .Include(c => c.Client)
                .ThenInclude(c => c.Country)
                .Include(c => c.Region)
                .Include(c => c.Country)
                    .ThenInclude(c => c.AgentCountrs)
                        .ThenInclude(c => c.Agent)
                .Include(c => c.OrderItems)
                    .ThenInclude(c => c.OrderTpye)
                .Where(c => c.IsSend == false && c.OrderplacedId == (int)OrderplacedEnum.Client)
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
            if (orderFilter.ClientPrintNumber != null)
            {
                orderIQ = orderIQ.Where(c => c.OrderPrints.Any(op => op.Print.PrintNmber == orderFilter.ClientPrintNumber && op.Print.Type == PrintType.Client));
            }
            if (orderFilter.AgentPrintNumber != null)
            {
                orderIQ = orderIQ.Where(c => c.AgentOrderPrints.Any(c => c.AgentPrint.Id == orderFilter.AgentPrintNumber));
            }
            if (orderFilter.CreatedDate != null)
            {
                orderIQ = orderIQ.Where(c => c.Date == orderFilter.CreatedDate);
            }
            if (orderFilter.Note != "" && orderFilter.Note != null)
            {
                orderIQ = orderIQ.Where(c => c.Note.Contains(orderFilter.Note));
            }
            if (orderFilter.AgentPrintStartDate != null)
            {
                ///TODO :
                ///chould check this query 
                orderIQ = orderIQ.Where(c => c.AgentOrderPrints.Select(c => c.AgentPrint).OrderBy(c => c.Id).LastOrDefault().Date >= orderFilter.AgentPrintStartDate);
            }
            if (orderFilter.AgentPrintEndDate != null)
            {
                ///TODO :
                ///chould check this query 
                orderIQ = orderIQ.Where(c => c.AgentOrderPrints.Select(c => c.AgentPrint).OrderBy(c => c.Id).LastOrDefault().Date <= orderFilter.AgentPrintEndDate);
            }
            return Ok(_mapper.Map<OrderDto[]>(await orderIQ.ToArrayAsync()));
        }

        [HttpPut("Accept")]
        public IActionResult Accept([FromBody] IdsDto idsDto)
        {
            var order = this._context.Orders.Find(idsDto.OrderId);
            var agnetCountries = this._context.AgentCountrs.Where(c => c.AgentId == idsDto.AgentId);
            if (!agnetCountries.Any(c => c.CountryId == order.CountryId))
            {
                return Conflict();
            }
            order.AgentId = idsDto.AgentId;
            order.AgentCost = (decimal)this._context.Users.Find(idsDto.AgentId).Salary;
            order.OrderplacedId = (int)OrderplacedEnum.Store;
            order.IsSend = true;
            this._context.Update(order);
            this._context.SaveChanges();
            return Ok();
        }
        [HttpPut("Acceptmultiple")]
        public async Task<IActionResult> AcceptMultiple([FromBody] List<IdsDto> idsDto)
        {
            //get data 
            var orders = await this._context.Orders.Where(c => idsDto.Select(dto => dto.OrderId).Contains(c.Id)).ToListAsync();
            var agentsContries = await this._context.AgentCountrs.Where(c => idsDto.Select(dto => dto.AgentId).Contains(c.AgentId)).ToListAsync();

            //validation 
            if (idsDto.Select(c => c.OrderId).Except(orders.Select(c => c.Id)).Any())
                return Conflict();

            if (idsDto.Select(c => c.AgentId).Except(agentsContries.Select(c => c.AgentId)).Any())
                return Conflict();

            foreach (var item in idsDto)
            {
                var order = orders.Find(c => c.Id == item.OrderId);
                var agentCountries = agentsContries.Where(c => c.AgentId == item.AgentId).ToList();
                if (!agentsContries.Any(c => c.CountryId != order.CountryId))
                {
                    return Conflict();
                }
                order.AgentId = item.AgentId;
                order.AgentCost = (decimal)this._context.Users.Find(item.AgentId).Salary;
                order.OrderplacedId = (int)OrderplacedEnum.Store;
                order.IsSend = true;
                this._context.Update(order);
            }
            this._context.SaveChanges();
            return Ok();
        }
        [HttpPut("DisAccept")]
        public IActionResult DisAccept([FromBody] DateWithId<int> dateWithId)
        {
            var order = this._context.Orders.Find(dateWithId.Ids);
            DisAcceptOrder disAcceptOrder = new DisAcceptOrder()
            {
                Code = order.Code,
                CountryId = order.CountryId,
                Cost = order.Cost,
                ClientNote = order.ClientNote,
                CreatedBy = order.CreatedBy,
                Date = order.Date,
                Address = order.Address,
                ClientId = order.ClientId,
                DeliveryCost = order.DeliveryCost,
                IsDollar = order.IsDollar,
                RecipientName = order.RecipientName,
                RecipientPhones = order.RecipientPhones,
                RegionId = order.RegionId,
                UpdatedBy = AuthoticateUserName(),
                UpdatedDate = dateWithId.Date
            };
            this._context.Orders.Remove(order);
            this._context.Add(disAcceptOrder);
            this._context.SaveChanges();
            return Ok();
        }
        [HttpPut("DisAcceptmultiple")]
        public async Task<IActionResult> DisAcceptMultiple([FromBody] DateWithId<List<int>> dateWithIds)
        {
            var ids = dateWithIds.Ids;
            var orders = await this._context.Orders.Where(c => ids.Contains(c.Id)).ToListAsync();
            if (ids.Except(orders.Select(c => c.Id)).Any())
            {
                return Conflict();
            }
            foreach (var order in orders)
            {
                DisAcceptOrder disAcceptOrder = new DisAcceptOrder()
                {
                    Code = order.Code,
                    CountryId = order.CountryId,
                    Cost = order.Cost,
                    ClientNote = order.ClientNote,
                    CreatedBy = order.CreatedBy,
                    Date = order.Date,
                    Address = order.Address,
                    ClientId = order.ClientId,
                    DeliveryCost = order.DeliveryCost,
                    IsDollar = order.IsDollar,
                    RecipientName = order.RecipientName,
                    RecipientPhones = order.RecipientPhones,
                    RegionId = order.RegionId,
                    UpdatedBy = AuthoticateUserName(),
                    UpdatedDate = dateWithIds.Date
                };
                this._context.Orders.Remove(order);
                this._context.Add(disAcceptOrder);
            }
            this._context.SaveChanges();
            return Ok();

        }
        [HttpGet("DisAccept")]
        public IActionResult DisAccpted([FromQuery] PagingDto pagingDto, [FromQuery] OrderFilter orderFilter)
        {
            var query = this._context.DisAcceptOrders.
                AsQueryable();
            if (orderFilter.CountryId != null)
            {
                query = query.Where(c => c.CountryId == orderFilter.CountryId);
            }
            if (orderFilter.Code != string.Empty && orderFilter.Code != null)
            {
                query = query.Where(c => c.Code.StartsWith(orderFilter.Code));
            }
            if (orderFilter.ClientId != null)
            {
                query = query.Where(c => c.ClientId == orderFilter.ClientId);
            }
            if (orderFilter.RegionId != null)
            {
                query = query.Where(c => c.RegionId == orderFilter.RegionId);
            }
            if (orderFilter.RecipientName != string.Empty && orderFilter.RecipientName != null)
            {
                query = query.Where(c => c.RecipientName.StartsWith(orderFilter.RecipientName));
            }
            if (orderFilter.Phone != string.Empty && orderFilter.Phone != null)
            {
                query = query.Where(c => c.RecipientPhones.Contains(orderFilter.Phone));
            }
            if (orderFilter.CreatedDate != null)
            {
                query = query.Where(c => c.Date == orderFilter.CreatedDate);
            }
            var total = query.Count();
            var orders = query.Skip((pagingDto.Page - 1) * pagingDto.RowCount).Take(pagingDto.RowCount)
                .Include(c => c.Client)
                .Include(c => c.Region)
                .Include(c => c.Country)
                .ToList();
            return Ok(new { data = _mapper.Map<OrderDto[]>(orders), total });
        }
        [HttpPut("MakeOrderInWay")]
        public IActionResult MakeOrderInWay([FromBody] DateWithId<int[]> dateWithId)
        {
            var ids = dateWithId.Ids;
            var orders = this._context.Orders
                .Include(c => c.Agent)
                .ThenInclude(c => c.UserPhones)
                .Include(c => c.Client)
                .Include(c => c.Country)
                .Where(c => ids.Contains(c.Id)).ToList();

            if (orders.FirstOrDefault(c => c.OrderplacedId != (int)OrderplacedEnum.Store) != null)
            {
                this.err.Messges.Add($"الشحنة رقم{orders.FirstOrDefault(c => c.OrderplacedId != (int)OrderplacedEnum.Store).Code} ليست في المخزن");
                return Conflict(err);
            }


            var agent = orders.FirstOrDefault().Agent;
            var agnetPrint = new AgentPrint()
            {
                Date = dateWithId.Date,
                PrinterName = this.AuthoticateUserName(),
                DestinationName = agent.Name,
                DestinationPhone = agent.UserPhones.FirstOrDefault()?.Phone ?? ""
            };
            var transaction = this._context.Database.BeginTransaction();
            try
            {
                this._context.AgentPrints.Add(agnetPrint);
                this._context.SaveChanges();
                foreach (var item in orders)
                {


                    item.OrderplacedId = (int)OrderplacedEnum.Way;
                    this._context.Update(item);
                    this._context.Entry(item).Reference(c => c.Region).Load();
                    var agnetOrderPrint = new AgentOrderPrint()
                    {
                        OrderId = item.Id,
                        AgentPrintId = agnetPrint.Id
                    };
                    var agentPrintDetials = new AgentPrintDetail()
                    {
                        Code = item.Code,
                        ClientName = item.Client.Name,
                        Note = item.Note,
                        Total = item.Cost,
                        Country = item.Country.Name,
                        AgentPrintId = agnetPrint.Id,
                        Phone = item.RecipientPhones,
                        Region = item.Region?.Name
                    };
                    this._context.Add(agentPrintDetials);
                    this._context.Add(agnetOrderPrint);
                }
                this._context.SaveChanges();
                transaction.Commit();
                return Ok(new { printNumber = agnetPrint.Id });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logging.WriteExption(ex);
                return BadRequest();
            }

            //var ids = dateWithId.Ids;
            //var orders = this._context.Orders
            //    .Include(c => c.Agent)
            //    .ThenInclude(c => c.UserPhones)
            //    .Include(c => c.Client)
            //    .Include(c => c.Country)
            //    .Where(c => ids.Contains(c.Id)).ToList();

            //if (orders.FirstOrDefault(c => c.OrderplacedId != (int)OrderplacedEnum.Store) != null)
            //{
            //    this.err.Messges.Add($"الشحنة رقم{orders.FirstOrDefault(c => c.OrderplacedId != (int)OrderplacedEnum.Store).Code} ليست في المخزن");
            //    return Conflict(err);
            //}

            //var oldPrint = this._context.Printeds.Where(c => c.Type == PrintType.Agent && c.PrintNmber == this._context.Printeds.Where(c => c.Type == PrintType.Agent).Max(c => c.PrintNmber)).FirstOrDefault();
            //var printNumber = oldPrint?.PrintNmber ?? 0;
            //++printNumber;
            //var agent = orders.FirstOrDefault().Agent;
            //var newPrint = new Printed()
            //{
            //    PrintNmber = printNumber,
            //    Date = dateWithId.Date,
            //    Type = PrintType.Agent,
            //    PrinterName = this.AuthoticateUserName(),
            //    DestinationName = agent.Name,
            //    DestinationPhone = agent.UserPhones.FirstOrDefault()?.Phone ?? "",
            //};
            //var transaction = this._context.Database.BeginTransaction();
            //try
            //{
            //    this._context.Printeds.Add(newPrint);
            //    this._context.SaveChanges();
            //    foreach (var item in orders)
            //    {


            //        item.OrderplacedId = (int)OrderplacedEnum.Way;
            //        this._context.Update(item);
            //        this._context.Entry(item).Reference(c => c.Region).Load();
            //        var orderPrint = new OrderPrint()
            //        {
            //            PrintId = newPrint.Id,
            //            OrderId = item.Id
            //        };
            //        var AgentPrint = new AgnetPrint()
            //        {
            //            Code = item.Code,
            //            ClientName = item.Client.Name,
            //            Note = item.Note,
            //            Total = item.Cost,
            //            Country = item.Country.Name,
            //            PrintId = newPrint.Id,
            //            Phone = item.RecipientPhones,
            //            Region = item.Region?.Name
            //        };
            //        this._context.Add(orderPrint);
            //        this._context.Add(AgentPrint);
            //    }
            //    this._context.SaveChanges();
            //    transaction.Commit();
            //    return Ok(new { printNumber });
            //}
            //catch (Exception ex)
            //{
            //    transaction.Rollback();
            //    _logging.WriteExption(ex);
            //    return BadRequest();
            //}
        }
        /// <summary>
        /// <!--استلام حالة شحنة-->
        /// </summary>
        /// <param name="orderStates"></param>
        /// 
        /// <returns></returns>
        [HttpPut("UpdateOrdersStatusFromAgent")]
        public async Task<IActionResult> UpdateOrdersStatusFromAgent(List<OrderStateDto> orderStates)
        {
            try
            {
                var outSideCompny = (await _moneyPlacedIndexService.GetAllLite()).Where(c => c.Id == (int)MoneyPalcedEnum.OutSideCompany).First().Name;
                List<Notfication> notfications = new List<Notfication>();
                List<Notfication> addednotfications = new List<Notfication>();
                var orders = await this._context.Orders.Where(c => orderStates.Select(c => c.Id).Contains(c.Id)).ToListAsync();
                foreach (var item in orderStates)
                {
                    var order = orders.Find(c => c.Id == item.Id);


                    OrderLog log = order;
                    this._context.Add(log);
                    order.OrderplacedId = item.OrderplacedId;
                    order.MoenyPlacedId = item.MoenyPlacedId;
                    this._context.Entry(order).Reference(c => c.MoenyPlaced).Load();
                    this._context.Entry(order).Reference(c => c.Orderplaced).Load();
                    order.Note = item.Note;

                    if (order.DeliveryCost != item.DeliveryCost)
                        if (order.OldDeliveryCost == null)
                            order.OldDeliveryCost = order.DeliveryCost;
                    order.DeliveryCost = item.DeliveryCost;
                    order.AgentCost = item.AgentCost;
                    order.SystemNote = "UpdateOrdersStatusFromAgent";
                    if (order.IsClientDiliverdMoney)
                    {
                        switch (order.OrderplacedId)
                        {
                            case (int)OrderplacedEnum.Delivered:
                                {
                                    if (Decimal.Compare(order.Cost, item.Cost) != 0)
                                    {
                                        if (order.OldCost == null)
                                            order.OldCost = order.Cost;
                                        order.Cost = item.Cost;
                                    }
                                    var payForClient = order.ShouldToPay();


                                    if (Decimal.Compare(payForClient, (order.ClientPaied ?? 0)) != 0)
                                    {
                                        order.OrderStateId = (int)OrderStateEnum.ShortageOfCash;
                                        if (order.MoenyPlacedId == (int)MoneyPalcedEnum.Delivered)
                                        {
                                            order.MoenyPlacedId = (int)MoneyPalcedEnum.InsideCompany;
                                        }
                                    }
                                    else
                                    {
                                        order.OrderStateId = (int)OrderStateEnum.Finished;
                                    }

                                }
                                break;
                            case (int)OrderplacedEnum.CompletelyReturned:
                                {
                                    if (order.OldCost == null)
                                        order.OldCost = order.Cost;
                                    order.Cost = 0;
                                    order.AgentCost = 0;
                                    order.OrderStateId = (int)OrderStateEnum.ShortageOfCash;
                                }
                                break;
                            case (int)OrderplacedEnum.Unacceptable:
                                {
                                    if (order.OldCost == null)
                                    {
                                        order.OldCost = order.Cost;
                                    }
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
                        }
                    }
                    else
                    {
                        switch (order.OrderplacedId)
                        {
                            case (int)OrderplacedEnum.PartialReturned:
                            case (int)OrderplacedEnum.Delivered:
                                {
                                    if (order.Cost != item.Cost)
                                    {
                                        if (order.OldCost == null)
                                            order.OldCost = order.Cost;
                                        order.Cost = item.Cost;
                                    }
                                }
                                break;
                            case (int)OrderplacedEnum.CompletelyReturned:
                                {
                                    if (order.OldCost == null)
                                    {
                                        order.OldCost = order.Cost;
                                    }
                                    order.Cost = 0;
                                    if (order.OldDeliveryCost == null)
                                        order.OldDeliveryCost = order.DeliveryCost;
                                    order.DeliveryCost = 0;
                                    order.AgentCost = 0;
                                }
                                break;
                            case (int)OrderplacedEnum.Unacceptable:
                                {
                                    if (order.OldCost == null)
                                        order.OldCost = order.Cost;
                                    order.Cost = 0;
                                }
                                break;

                        }
                    }
                    if (order.MoenyPlacedId == (int)MoneyPalcedEnum.InsideCompany)
                    {
                        order.ApproveAgentEditOrderRequests.Clear();
                    }
                    this._context.Update(order);

                    if (order.OrderStateId != (int)OrderStateEnum.Finished && order.OrderplacedId != (int)OrderplacedEnum.Way)
                    {
                        var clientNotigaction = notfications.Where(c => c.ClientId == order.ClientId && c.OrderPlacedId == order.OrderplacedId && c.MoneyPlacedId == order.MoenyPlacedId).FirstOrDefault();
                        if (clientNotigaction == null)
                        {
                            int moenyPlacedId = item.MoenyPlacedId;
                            if (moenyPlacedId == (int)MoneyPalcedEnum.WithAgent)
                                moenyPlacedId = (int)MoneyPalcedEnum.OutSideCompany;
                            clientNotigaction = new Notfication()
                            {
                                ClientId = order.ClientId,
                                OrderPlacedId = moenyPlacedId,
                                MoneyPlacedId = item.MoenyPlacedId,
                                IsSeen = false,
                                OrderCount = 1
                            };
                            notfications.Add(clientNotigaction);
                        }
                        else
                        {
                            clientNotigaction.OrderCount++;
                        }
                    }
                    var moneyPlacedName = order.MoenyPlaced.Name;
                    if (order.MoenyPlacedId == (int)MoneyPalcedEnum.WithAgent)
                        moneyPlacedName = outSideCompny;
                    Notfication notfication = new Notfication()
                    {
                        Note = $"الطلب {order.Code} اصبح {order.Orderplaced.Name} و موقع المبلغ  {moneyPlacedName}",
                        ClientId = order.ClientId
                    };
                    this._context.Add(notfication);
                    addednotfications.Add(notfication);
                }
                foreach (var item in notfications)
                {
                    addednotfications.Add(item);
                    this._context.Add(item);
                }
                this._context.SaveChanges();
                {
                    var newnotifications = addednotfications.GroupBy(c => c.ClientId).ToList();
                    foreach (var item in newnotifications)
                    {
                        var key = item.Key;
                        List<NotficationDto> notficationDtos = new List<NotficationDto>();
                        foreach (var groupItem in item)
                        {
                            notficationDtos.Add(_mapper.Map<NotficationDto>(groupItem));
                        }
                        await notificationHub.AllNotification(key.ToString(), notficationDtos.ToArray());
                        ;
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logging.WriteExption(ex);
                return BadRequest();
            }
        }
        [HttpGet("GetClientprint")]
        public IActionResult GetClientprint([FromQuery] PagingDto pagingDto, [FromQuery] int? number, string clientName, string code)
        {
            var orderPrintIq = this._context.Printeds
                .Include(c => c.ClientPrints)
                .ThenInclude(c => c.OrderPlaced)
                .Where(c => c.Type == PrintType.Client);
            if (number != null)
            {
                orderPrintIq = orderPrintIq.Where(c => c.PrintNmber == number);
            }
            if (clientName != null)
            {
                orderPrintIq = orderPrintIq.Where(c => c.DestinationName == clientName);
            }
            if (!string.IsNullOrEmpty(code))
            {
                orderPrintIq = orderPrintIq.Where(c => c.ClientPrints.Any(c => c.Code.StartsWith(code)));
            }

            var total = orderPrintIq.Count();
            var orders = orderPrintIq.OrderByDescending(c => c.Date).Skip((pagingDto.Page - 1) * pagingDto.RowCount).Take(pagingDto.RowCount).ToList();

            return Ok(new { data = _mapper.Map<PrintOrdersDto[]>(orders), total });
        }
        [HttpGet("GetAgentPrint")]
        public IActionResult GetAgentPrint([FromQuery] PagingDto pagingDto, [FromQuery] int? number, string agnetName)
        {
            var ordersPrint = this._context.AgentPrints.AsQueryable();
            if (number != null)
            {
                ordersPrint = ordersPrint.Where(c => c.Id == number);
            }
            if (!String.IsNullOrWhiteSpace(agnetName))
            {
                ordersPrint = ordersPrint.Where(c => c.DestinationName == agnetName);
            }
            var total = ordersPrint.Count();
            var orders = ordersPrint.OrderByDescending(c => c.Id).Skip((pagingDto.Page - 1) * pagingDto.RowCount).Take(pagingDto.RowCount).ToList();
            return Ok(new { data = _mapper.Map<PrintOrdersDto[]>(orders), total });
        }
        /// <summary>
        /// تسديد العميل
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut("DeleiverMoneyForClient")]
        public IActionResult DeleiverMoneyForClient([FromBody] DeleiverMoneyForClientDto deleiverMoneyForClientDto)
        {
            //var orders = this._context.Orders
            //.Include(c => c.Client)
            //.ThenInclude(c => c.ClientPhones)
            //.Include(c => c.Country)
            //.Include(c => c.Orderplaced)
            //.Include(c => c.MoenyPlaced)
            //.Where(c => deleiverMoneyForClientDto.DateWithId.Ids.Contains(c.Id)).ToList();
            //var client = orders.FirstOrDefault().Client;
            //if (orders.Any(c => c.ClientId != client.Id))
            //{
            //    this.err.Messges.Add($"ليست جميع الشحنات لنفس العميل");
            //    return Conflict(err);
            //}
            //semaphore.Wait();
            //var clientPayment = new ClientPayment()
            //{
            //    Date = deleiverMoneyForClientDto.DateWithId.Date,
            //    PrinterName = User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value,
            //    DestinationName = client.Name,
            //    DestinationPhone = client.ClientPhones.FirstOrDefault()?.Phone ?? "",

            //};

            //var transaction = this._context.Database.BeginTransaction();
            //try
            //{
            //    this._context.ClientPayments.Add(clientPayment);
            //    this._context.SaveChanges();
            //    if (!orders.All(c => c.OrderplacedId == (int)OrderplacedEnum.CompletelyReturned || c.OrderplacedId == (int)OrderplacedEnum.Unacceptable))
            //    {
            //        var recepits = this._context.Receipts.Where(c => c.PrintId == null && c.ClientId == client.Id).ToList();
            //        recepits.ForEach(c =>
            //        {
            //            c.ClientPaymentId = clientPayment.Id;
            //            this._context.Update(c);
            //        });
            //        this._context.SaveChanges();
            //    }
            //    int totalPoints = 0;

            //    foreach (var item in orders)
            //    {

            //        if (!item.IsClientDiliverdMoney)
            //        {
            //            if (!(item.OrderplacedId == (int)OrderplacedEnum.CompletelyReturned || item.OrderplacedId == (int)OrderplacedEnum.Delayed))
            //            {
            //                totalPoints += item.Country.Points;
            //            }
            //        }
            //        else
            //        {
            //            if ((item.OrderplacedId == (int)OrderplacedEnum.CompletelyReturned || item.OrderplacedId == (int)OrderplacedEnum.Delayed))
            //            {
            //                totalPoints -= item.Country.Points;
            //            }
            //        }

            //        if (item.OrderplacedId > (int)OrderplacedEnum.Way)
            //        {
            //            item.OrderStateId = (int)OrderStateEnum.Finished;
            //            if (item.MoenyPlacedId == (int)MoneyPalcedEnum.InsideCompany)
            //            {
            //                item.MoenyPlacedId = (int)MoneyPalcedEnum.Delivered;
            //            }

            //        }

            //        item.IsClientDiliverdMoney = true;
            //        var currentPay = item.ShouldToPay() - (item.ClientPaied ?? 0);
            //        item.ClientPaied = item.ShouldToPay();
            //        this._context.Update(item);
            //        this._context.SaveChanges();
            //        var orderClientPaymnet = new OrderClientPaymnet()
            //        {
            //            OrderId = item.Id,
            //            ClientPaymentId = clientPayment.Id
            //        };

            //        var clientPaymentDetials = new ClientPaymentDetail()
            //        {
            //            Code = item.Code,
            //            Total = item.Cost,
            //            Country = item.Country.Name,
            //            ClientPaymentId = clientPayment.Id,
            //            Phone = item.RecipientPhones,
            //            DeliveryCost = item.DeliveryCost,
            //            LastTotal = item.OldCost,
            //            Note = item.Note,
            //            MoneyPlacedId = item.MoenyPlacedId,
            //            OrderPlacedId = item.OrderplacedId,
            //            PayForClient = currentPay
            //        };

            //        this._context.Add(orderClientPaymnet);
            //        this._context.Add(clientPaymentDetials);
            //        this._context.SaveChanges();
            //    }
            //    client.Points += totalPoints;
            //    this._context.Update(client);
            //    this._context.SaveChanges();
            //    if (deleiverMoneyForClientDto.PointsSettingId != null)
            //    {
            //        var pointSetting = this._context.PointsSettings.Find(deleiverMoneyForClientDto.PointsSettingId);

            //        Discount discount = new Discount()
            //        {
            //            Money = pointSetting.Money,
            //            Points = pointSetting.Points,
            //            ClientPaymentId= clientPayment.Id
            //        };
            //        this._context.Add(discount);
            //        this._context.SaveChanges();
            //    }
            //    this._context.Add(new Notfication()
            //    {
            //        Note = "تم تسديدك برقم " + clientPayment.Id,
            //        ClientId = client.Id
            //    });
            //    this._context.SaveChanges();
            //    transaction.Commit();
            //    semaphore.Release();
            //    return Ok(new { clientPayment.Id });
            //}
            //catch (Exception ex)
            //{

            //    semaphore.Release();
            //    transaction.Rollback();
            //    _logging.WriteExption(ex);
            //    return BadRequest();


            //}
            var orders = this._context.Orders
            .Include(c => c.Client)
            .ThenInclude(c => c.ClientPhones)
            .Include(c => c.Country)
            .Include(c => c.Orderplaced)
            .Include(c => c.MoenyPlaced)
            .Where(c => deleiverMoneyForClientDto.DateWithId.Ids.Contains(c.Id)).ToList();
            var client = orders.FirstOrDefault().Client;
            if (orders.Any(c => c.ClientId != client.Id))
            {
                this.err.Messges.Add($"ليست جميع الشحنات لنفس العميل");
                return Conflict(err);
            }
            semaphore.Wait();
            var oldPrint = this._context.Printeds.Where(c => c.Type == PrintType.Client && c.PrintNmber == this._context.Printeds.Where(c => c.Type == PrintType.Client).Max(c => c.PrintNmber)).FirstOrDefault();
            var printNumber = oldPrint?.PrintNmber ?? 0;

            ++printNumber;
            var newPrint = new Printed()
            {
                PrintNmber = printNumber,
                Date = deleiverMoneyForClientDto.DateWithId.Date,
                Type = PrintType.Client,
                PrinterName = User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value,
                DestinationName = client.Name,
                DestinationPhone = client.ClientPhones.FirstOrDefault()?.Phone ?? "",
            };
            var transaction = this._context.Database.BeginTransaction();
            try
            {
                this._context.Printeds.Add(newPrint);
                this._context.SaveChanges();
                if (!orders.All(c => c.OrderplacedId == (int)OrderplacedEnum.CompletelyReturned || c.OrderplacedId == (int)OrderplacedEnum.Unacceptable))
                {
                    var recepits = this._context.Receipts.Where(c => c.PrintId == null && c.ClientId == client.Id).ToList();
                    recepits.ForEach(c =>
                    {
                        c.PrintId = newPrint.Id;
                        this._context.Update(c);
                    });
                    this._context.SaveChanges();
                }
                int totalPoints = 0;

                foreach (var item in orders)
                {

                    if (!item.IsClientDiliverdMoney)
                    {
                        if (!(item.OrderplacedId == (int)OrderplacedEnum.CompletelyReturned || item.OrderplacedId == (int)OrderplacedEnum.Delayed))
                        {
                            totalPoints += item.Country.Points;
                        }
                    }
                    else
                    {
                        if ((item.OrderplacedId == (int)OrderplacedEnum.CompletelyReturned || item.OrderplacedId == (int)OrderplacedEnum.Delayed))
                        {
                            totalPoints -= item.Country.Points;
                        }
                    }

                    if (item.OrderplacedId > (int)OrderplacedEnum.Way)
                    {
                        item.OrderStateId = (int)OrderStateEnum.Finished;
                        if (item.MoenyPlacedId == (int)MoneyPalcedEnum.InsideCompany)
                        {
                            item.MoenyPlacedId = (int)MoneyPalcedEnum.Delivered;
                        }

                    }

                    item.IsClientDiliverdMoney = true;
                    var currentPay = item.ShouldToPay() - (item.ClientPaied ?? 0);
                    item.ClientPaied = item.ShouldToPay();
                    this._context.Update(item);
                    this._context.SaveChanges();
                    var orderPrint = new OrderPrint()
                    {
                        PrintId = newPrint.Id,
                        OrderId = item.Id
                    };

                    var clientPrint = new ClientPrint()
                    {
                        Code = item.Code,
                        Total = item.Cost,
                        Country = item.Country.Name,
                        PrintId = newPrint.Id,
                        Phone = item.RecipientPhones,
                        DeliveCost = item.DeliveryCost,
                        Date = item.Date,
                        LastTotal = item.OldCost,
                        Note = item.Note,
                        MoneyPlacedId = item.MoenyPlacedId,
                        OrderPlacedId = item.OrderplacedId,
                        PayForClient = currentPay
                    };
                    this._context.Add(orderPrint);
                    this._context.Add(clientPrint);
                    this._context.SaveChanges();
                }
                client.Points += totalPoints;
                this._context.Update(client);
                this._context.SaveChanges();
                if (deleiverMoneyForClientDto.PointsSettingId != null)
                {
                    var pointSetting = this._context.PointsSettings.Find(deleiverMoneyForClientDto.PointsSettingId);

                    Discount discount = new Discount()
                    {
                        Money = pointSetting.Money,
                        Points = pointSetting.Points,
                        PrintedId = newPrint.Id
                    };
                    this._context.Add(discount);
                    this._context.SaveChanges();
                }
                this._context.Add(new Notfication()
                {
                    Note = "تم تسديدك برقم " + printNumber,
                    ClientId = client.Id
                });
                this._context.SaveChanges();
                transaction.Commit();
                semaphore.Release();
                return Ok(new { printNumber });
            }
            catch (Exception ex)
            {

                semaphore.Release();
                transaction.Rollback();
                _logging.WriteExption(ex);
                return BadRequest();


            }
        }
        /// <summary>
        /// تسديد الشركات
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut("DeleiverMoneyForClientWithStatus")]
        public IActionResult DeleiverMoneyForClientWithStatus(DateWithId<int[]> idsAndDate)
        {
            var ids = idsAndDate.Ids;
            var orders = this._context.Orders
                .Include(c => c.Client)
                .ThenInclude(c => c.ClientPhones)
                .Include(c => c.Orderplaced)
                .Include(c => c.MoenyPlaced)
                .Include(c => c.Country)
                .Where(c => ids.Contains(c.Id)).ToList();
            var client = orders.FirstOrDefault().Client;
            if (orders.Any(c => c.ClientId != client.Id))
            {
                this.err.Messges.Add($"ليست جميع الشحنات لنفس العميل");
                return Conflict(err);
            }
            semaphore.Wait();
            var oldPrint = this._context.Printeds.Where(c => c.Type == PrintType.Client && c.PrintNmber == this._context.Printeds.Where(c => c.Type == PrintType.Client).Max(c => c.PrintNmber)).FirstOrDefault();
            var printNumber = oldPrint?.PrintNmber ?? 0;

            ++printNumber;
            var newPrint = new Printed()
            {
                PrintNmber = printNumber,
                Date = idsAndDate.Date,
                Type = PrintType.Client,
                PrinterName = User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value,
                DestinationName = client.Name,
                DestinationPhone = client.ClientPhones.FirstOrDefault()?.Phone ?? "",
            };
            var transaction = this._context.Database.BeginTransaction();
            try
            {
                this._context.Printeds.Add(newPrint);
                this._context.SaveChanges();
                foreach (var item in orders)
                {
                    if (item.OrderplacedId > (int)OrderplacedEnum.Way)
                    {
                        item.OrderStateId = (int)OrderStateEnum.Finished;
                        if (item.MoenyPlacedId == (int)MoneyPalcedEnum.InsideCompany)
                        {
                            item.MoenyPlacedId = (int)MoneyPalcedEnum.Delivered;
                        }

                    }
                    item.IsClientDiliverdMoney = true;
                    var cureentPay = item.ShouldToPay() - (item.ClientPaied ?? 0);
                    item.ClientPaied = item.ShouldToPay();

                    this._context.Update(item);
                    _context.SaveChanges();
                    var orderPrint = new OrderPrint()
                    {
                        PrintId = newPrint.Id,
                        OrderId = item.Id
                    };

                    var clientPrint = new ClientPrint()
                    {
                        Code = item.Code,
                        Total = item.Cost,
                        Country = item.Country.Name,
                        PrintId = newPrint.Id,
                        Phone = item.RecipientPhones,
                        DeliveCost = item.DeliveryCost,
                        MoneyPlacedId = item.MoenyPlacedId,
                        OrderPlacedId = item.OrderplacedId,
                        LastTotal = item.OldCost,
                        PayForClient = cureentPay,
                        Date = item.Date,
                        Note = item.Note,
                    };
                    this._context.Add(orderPrint);
                    this._context.Add(clientPrint);
                }
                this._context.SaveChanges();

                transaction.Commit();
                semaphore.Release();
                return Ok(new { printNumber });
            }
            catch (Exception ex)
            {
                semaphore.Release();
                transaction.Rollback();
                _logging.WriteExption(ex);
                return BadRequest();

            }
            //var ids = idsAndDate.Ids;
            //var orders = this._context.Orders
            //    .Include(c => c.Client)
            //    .ThenInclude(c => c.ClientPhones)
            //    .Include(c => c.Orderplaced)
            //    .Include(c => c.MoenyPlaced)
            //    .Include(c => c.Country)
            //    .Where(c => ids.Contains(c.Id)).ToList();
            //var client = orders.FirstOrDefault().Client;
            //if (orders.Any(c => c.ClientId != client.Id))
            //{
            //    this.err.Messges.Add($"ليست جميع الشحنات لنفس العميل");
            //    return Conflict(err);
            //}
            //semaphore.Wait();

            //var clientPaymnet = new ClientPayment()
            //{
            //    Date = idsAndDate.Date,
            //    PrinterName = User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value,
            //    DestinationName = client.Name,
            //    DestinationPhone = client.ClientPhones.FirstOrDefault()?.Phone ?? "",
            //};
            //var transaction = this._context.Database.BeginTransaction();
            //try
            //{
            //    this._context.Add(clientPaymnet);
            //    this._context.SaveChanges();
            //    foreach (var item in orders)
            //    {
            //        if (item.OrderplacedId > (int)OrderplacedEnum.Way)
            //        {
            //            item.OrderStateId = (int)OrderStateEnum.Finished;
            //            if (item.MoenyPlacedId == (int)MoneyPalcedEnum.InsideCompany)
            //            {
            //                item.MoenyPlacedId = (int)MoneyPalcedEnum.Delivered;
            //            }

            //        }
            //        item.IsClientDiliverdMoney = true;
            //        var cureentPay = item.ShouldToPay() - (item.ClientPaied ?? 0);
            //        item.ClientPaied = item.ShouldToPay();

            //        this._context.Update(item);
            //        _context.SaveChanges();
            //        var orderClientPayment = new OrderClientPaymnet()
            //        {
            //            OrderId = item.Id,
            //            ClientPaymentId = clientPaymnet.Id
            //        };
            //        var clientPaymnetDetial = new ClientPaymentDetail()
            //        {
            //            Code = item.Code,
            //            Total = item.Cost,
            //            Country = item.Country.Name,
            //            ClientPaymentId = clientPaymnet.Id,
            //            Phone = item.RecipientPhones,
            //            DeliveryCost = item.DeliveryCost,
            //            MoneyPlacedId = item.MoenyPlacedId,
            //            OrderPlacedId = item.OrderplacedId,
            //            LastTotal = item.OldCost,
            //            PayForClient = cureentPay,
            //            Date = item.Date,
            //            Note = item.Note,
            //        };
            //        this._cx`ontext.Add(orderClientPayment);
            //        this._context.Add(clientPaymnetDetial);
            //    }
            //    this._context.SaveChanges();

            //    transaction.Commit();
            //    semaphore.Release();
            //    return Ok(new { clientPaymnet.Id });
            //}
            //catch (Exception ex)
            //{
            //    semaphore.Release();
            //    transaction.Rollback();
            //    _logging.WriteExption(ex);
            //    return BadRequest();

            //}

        }
        [HttpGet("GetOrderByAgent/{orderCode}")]
        public IActionResult GetOrderByAgent(string orderCode)
        {
            var orders = this._context.Orders.Where(c => c.Code == orderCode)
                .Include(c => c.Orderplaced)
                .Include(c => c.MoenyPlaced)
                .Include(c => c.Region)
                .Include(c => c.Country)
                .Include(c => c.Client)
                .Include(c => c.Agent)
                .ToList();
            if (orders.Count() == 0)
            {
                return Conflict(new { message = "الشحنة غير موجودة" });
            }
            var lastOrderAdded = orders.Last();
            var orderInStor = orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Store).ToList();
            orders = orders.Except(orderInStor).ToList();

            var fOrder = orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.CompletelyReturned || c.OrderplacedId == (int)OrderplacedEnum.Unacceptable || (c.OrderplacedId == (int)OrderplacedEnum.Delivered && (c.MoenyPlacedId == (int)MoneyPalcedEnum.InsideCompany || c.MoenyPlacedId == (int)MoneyPalcedEnum.Delivered))).ToList();
            orders = orders.Except(fOrder).ToList();
            var orderInCompany = orders.Where(c => c.MoenyPlacedId == (int)MoneyPalcedEnum.InsideCompany).ToList();
            orders = orders.Except(orderInCompany).ToList();
            if (orders.Count() == 0)
            {
                if (lastOrderAdded.OrderplacedId == (int)OrderplacedEnum.Store)
                {
                    return Conflict(new { message = "الشحنة ما زالت في المخزن" });
                }
                if (lastOrderAdded.MoenyPlacedId == (int)MoneyPalcedEnum.InsideCompany)
                {
                    return Conflict(new { message = "الشحنة داخل الشركة" });
                }
                else
                {
                    return Conflict(new { message = "تم إستلام الشحنة مسبقاً" });
                }

            }

            return Ok(_mapper.Map<OrderDto[]>(orders));
        }
        [HttpGet("GetEarnings")]
        public IActionResult GetEarnings([FromQuery] PagingDto pagingDto, [FromQuery] DateFiter dateFiter)
        {
            var ordersQuery = this._context.Orders
                .Include(c => c.Orderplaced)
                .Include(c => c.MoenyPlaced)
                .Where(c => c.OrderStateId == (int)OrderStateEnum.Finished && c.OrderplacedId != (int)OrderplacedEnum.CompletelyReturned);
            if (dateFiter.FromDate != null)
                ordersQuery = ordersQuery.Where(c => c.Date >= dateFiter.FromDate);
            if (dateFiter.ToDate != null)
                ordersQuery = ordersQuery.Where(c => c.Date <= dateFiter.ToDate);
            var totalRecord = ordersQuery.Count();
            var totalEarinig = ordersQuery.Sum(c => c.DeliveryCost - c.AgentCost);
            var orders = ordersQuery.Skip((pagingDto.Page - 1) * pagingDto.RowCount).Take(pagingDto.RowCount).ToList();
            return Ok(new { data = new { orders = _mapper.Map<OrderDto[]>(orders), totalEarinig }, total = totalRecord });
        }
        [HttpGet("ShipmentsNotReimbursedToTheClient/{clientId}")]
        public IActionResult ShipmentsNotReimbursedToTheClient(int clientId)
        {
            var orders = this._context.Orders.Where(c => c.ClientId == clientId && c.IsClientDiliverdMoney == false && c.OrderplacedId >= (int)OrderplacedEnum.Way)
                 .Include(c => c.Agent)
                     .ThenInclude(c => c.UserPhones)
                 .Include(c => c.Region)
                 .Include(c => c.Country)
                 .Include(c => c.Orderplaced)
                 .Include(c => c.MoenyPlaced).ToList();
            return Ok(_mapper.Map<OrderDto[]>(orders));
        }
        [HttpPut("ReiveMoneyFromClient")]
        public IActionResult ReiveMoneyFromClient([FromBody] int[] ids)
        {
            var orders = this._context.Orders.Where(c => ids.Contains(c.Id))
                .ToList();
            orders.ForEach(c => c.OrderStateId = (int)OrderStateEnum.Finished);
            this._context.SaveChanges();
            return Ok();
        }
        [HttpGet("GetOrderByAgnetPrintNumber")]
        public IActionResult GetOrderByAgnetPrintNumber([FromQuery] int printNumber)
        {
            var printed = this._context.AgentPrints.Include(c => c.AgentPrintDetails).FirstOrDefault(c => c.Id == printNumber);
            if (printed == null)
            {
                this.err.Messges.Add($"رقم الطباعة غير موجود");
                return Conflict(this.err);
            }
            var x = _mapper.Map<PrintOrdersDto>(printed);
            return Ok(x);
        }
        [HttpGet("GetOrderByClientPrintNumber")]
        public IActionResult GetOrderByClientPrintNumber([FromQuery] int printNumber)
        {
            var printed = this._context.Printeds.Where(c => c.PrintNmber == printNumber && c.Type == PrintType.Client)
                .Include(c => c.Discounts)
                .Include(c => c.Receipts)
                .Include(c => c.ClientPrints)
                    .ThenInclude(c => c.OrderPlaced)
                .FirstOrDefault();
            if (printed == null)
            {
                this.err.Messges.Add($"رقم الطباعة غير موجود");
                return Conflict(this.err);
            }
            var x = _mapper.Map<PrintOrdersDto>(printed);
            return Ok(x);
        }
        /// <summary>
        /// طلبات في ذمة المندوب
        /// </summary>
        /// <param name="agnetId"></param>
        /// <returns></returns>
        [HttpGet("OrderVicdanAgent/{agnetId}")]
        public IActionResult OrderVicdanAgent(int agnetId)
        {
            var orders = this._context.Orders.
                Where(c => c.AgentId == agnetId)
                .Where(c => c.AgentRequestStatus == (int)AgentRequestStatusEnum.Pending || (c.MoenyPlacedId == (int)MoneyPalcedEnum.WithAgent) || (c.IsClientDiliverdMoney == true && c.OrderplacedId == (int)OrderplacedEnum.Way))
                .Include(c => c.Client)
                 .Include(c => c.Region)
                 .Include(c => c.Country)
                 .Include(c => c.Orderplaced)
                 .Include(c => c.MoenyPlaced);
            return Ok(_mapper.Map<OrderDto[]>(orders));
        }
        [HttpGet("GetOrderForPayBy/{clientId}/{code}")]
        public async Task<ActionResult<PayForClientDto>> GetByCodeAndClient(int clientId, string code)
        {
            var order = await _context.Orders.Where(c => c.ClientId == clientId && c.Code == code)
                   .Include(c => c.OrderPrints)
                   .ThenInclude(c => c.Print)
                   .FirstOrDefaultAsync();
            if (order == null)
            {
                return Conflict(new { Message = "الشحنة غير موجودة" });
            }

            if (order.IsClientDiliverdMoney && order.OrderStateId != (int)OrderStateEnum.ShortageOfCash)
            {
                return Conflict(new { Message = "تم تسليم كلفة الشحنة من قبل" });
            }
            if (order.OrderplacedId == (int)OrderplacedEnum.Client)
            {
                return Conflict(new { Message = "الشحنة عند العميل " });
            }
            if (order.OrderplacedId == (int)OrderplacedEnum.Store)
            {
                return Conflict(new { Message = "الشحنة داخل المخزن" });
            }
            await _context.Entry(order).Reference(c => c.MoenyPlaced).LoadAsync();
            await _context.Entry(order).Reference(c => c.Orderplaced).LoadAsync();
            await _context.Entry(order).Reference(c => c.Country).LoadAsync();
            await _context.Entry(order.Country).Collection(c => c.Regions).LoadAsync();
            await _context.Entry(order).Reference(c => c.Region).LoadAsync();
            await _context.Entry(order).Reference(c => c.Agent).LoadAsync();
            return Ok(_mapper.Map<PayForClientDto>(order));
        }
        [HttpPut("ReSend")]
        public async Task<IActionResult> ReSend([FromBody] OrderReSend orderReSend)
        {
            var order = await this._context.Orders.FindAsync(orderReSend.Id);
            order.CountryId = orderReSend.CountryId;
            order.RegionId = orderReSend.RegionId;
            order.AgentId = orderReSend.AgnetId;
            if (order.OldCost != null)
            {
                order.Cost = (decimal)order.OldCost;
                order.OldCost = null;
            }
            order.IsClientDiliverdMoney = false;

            order.OrderStateId = (int)OrderStateEnum.Processing;
            order.OrderplacedId = (int)OrderplacedEnum.Store;
            order.DeliveryCost = orderReSend.DeliveryCost;
            order.MoenyPlacedId = (int)MoneyPalcedEnum.OutSideCompany;
            order.AgentCost = (await this._context.Users.FindAsync(order.AgentId)).Salary ?? 0;
            this._context.Update(order);
            await this._context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("MakeStoreOrderCompletelyReturned")]
        public IActionResult MakeOrderCompletelyReturned([FromBody] int id)
        {
            var order = this._context.Orders.Find(id);
            OrderLog log = order;
            this._context.Add(log);
            if (order.OrderplacedId != (int)OrderplacedEnum.Store)
            {
                this.err.Messges.Add($"الشحنة ليست في المخزن");
                return Conflict(err);
            }
            order.OrderplacedId = (int)OrderplacedEnum.CompletelyReturned;
            order.MoenyPlacedId = (int)MoneyPalcedEnum.InsideCompany;
            order.OldCost = order.Cost;
            order.Cost = 0;
            if (order.OldDeliveryCost == null)
                order.OldDeliveryCost = order.DeliveryCost;
            order.DeliveryCost = 0;
            order.AgentCost = 0;
            order.UpdatedDate = DateTime.Now;
            order.UpdatedBy = AuthoticateUserName();
            order.SystemNote = "MakeStoreOrderCompletelyReturned";
            this._context.Update(order);
            OrderLog orderLog = new OrderLog();
            this._context.SaveChanges();
            return Ok(_mapper.Map<OrderDto>(order));
        }
        [HttpPut("TransferOrderToAnotherAgnet")]
        public IActionResult TransferOrderToAnotherAgnet([FromBody] TransferOrderToAnotherAgnetDto transferOrderToAnotherAgnetDto)
        {
            var agnet = this._context.Users.Find(transferOrderToAnotherAgnetDto.NewAgentId);
            var orders = this._context.Orders.Where(c => transferOrderToAnotherAgnetDto.Ids.Contains(c.Id)).ToList();
            orders.ForEach(c =>
            {
                c.AgentId = agnet.Id;
                c.AgentCost = agnet.Salary ?? 0;

            });
            this._context.SaveChanges();
            return Ok();
        }
        [HttpPatch("AddPrintNumber/{orderId}")]
        public IActionResult AddPrintNumber(int orderId)
        {
            var order = this._context.Orders.Find(orderId);
            order.PrintedTimes += 1;
            this._context.Update(order);
            this._context.SaveChanges();
            return Ok();
        }
        [HttpPatch("AddPrintNumberMultiple")]
        public async Task<IActionResult> AddPrintNumber([FromBody] int[] orderids)
        {
            var orders = await this._context.Orders.Where(c => orderids.Contains(c.Id)).ToListAsync();
            foreach (var item in orders)
            {
                item.PrintedTimes += 1;
                this._context.Update(item);
            }
            this._context.SaveChanges();
            return Ok();
        }
        [HttpGet("OrderRequestEditState")]
        public IActionResult OrderRequestEditState()
        {
            var response = this._context.ApproveAgentEditOrderRequests.Where(c => c.IsApprove == null)
                .Include(c => c.Order)
                .Include(c => c.Agent)
                .Include(c => c.OrderPlaced)
                .ToList();

            var x = _mapper.Map<ApproveAgentEditOrderRequestDto[]>(response);
            return Ok(x);
        }

        [HttpPut("DisAproveOrderRequestEditState")]
        public IActionResult DisAproveOrderRequestEditStateCount([FromBody] int[] ids)
        {

            var requests = this._context.ApproveAgentEditOrderRequests.Where(c => ids.Contains(c.Id)).ToList();
            requests.ForEach(c =>
            {
                c.IsApprove = false;
                var order = this._context.Orders.Find(c.OrderId);
                order.AgentRequestStatus = (int)AgentRequestStatusEnum.DisApprove;
            });

            this._context.SaveChanges();
            return Ok();
        }
        [HttpPut("AproveOrderRequestEditState")]
        public async Task<IActionResult> OrderRequestEditStateCount([FromBody] int[] ids)
        {
            var requests = this._context.ApproveAgentEditOrderRequests.Where(c => ids.Contains(c.Id)).ToList();
            var transaction = this._context.Database.BeginTransaction();
            try
            {

                requests.ForEach(c =>
                {
                    c.IsApprove = true;
                    this._context.Update(c);
                });
                List<Notfication> notfications = new List<Notfication>();
                List<Notfication> addednotfications = new List<Notfication>();
                foreach (var item in requests)
                {
                    var order = this._context.Orders.Find(item.OrderId);


                    OrderLog log = order;
                    this._context.Add(log);
                    order.OrderplacedId = item.OrderPlacedId;
                    order.MoenyPlacedId = (int)MoneyPalcedEnum.WithAgent;
                    this._context.Entry(order).Reference(c => c.MoenyPlaced).Load();
                    this._context.Entry(order).Reference(c => c.Orderplaced).Load();
                    order.AgentRequestStatus = (int)AgentRequestStatusEnum.Approve;


                    order.SystemNote = "OrderRequestEditStateCount";
                    if (order.IsClientDiliverdMoney)
                    {
                        switch (order.OrderplacedId)
                        {
                            case (int)OrderplacedEnum.Delivered:
                                {
                                    if (Decimal.Compare(order.Cost, item.NewAmount) != 0)
                                    {
                                        if (order.OldCost == null)
                                            order.OldCost = order.Cost;
                                        order.Cost = item.NewAmount;
                                    }
                                    var payForClient = order.ShouldToPay();


                                    if (Decimal.Compare(payForClient, (order.ClientPaied ?? 0)) != 0)
                                    {
                                        order.OrderStateId = (int)OrderStateEnum.ShortageOfCash;
                                        if (order.MoenyPlacedId == (int)MoneyPalcedEnum.Delivered)
                                        {
                                            order.MoenyPlacedId = (int)MoneyPalcedEnum.InsideCompany;
                                        }
                                    }
                                    else
                                    {
                                        order.OrderStateId = (int)OrderStateEnum.Finished;
                                    }

                                }
                                break;
                            case (int)OrderplacedEnum.CompletelyReturned:
                                {
                                    if (order.OldCost == null)
                                        order.OldCost = order.Cost;
                                    order.Cost = 0;
                                    order.AgentCost = 0;
                                    order.OrderStateId = (int)OrderStateEnum.ShortageOfCash;
                                }
                                break;
                            case (int)OrderplacedEnum.Unacceptable:
                                {
                                    if (order.OldCost == null)
                                    {
                                        order.OldCost = order.Cost;
                                    }
                                    order.OrderStateId = (int)OrderStateEnum.ShortageOfCash;
                                }
                                break;
                            case (int)OrderplacedEnum.PartialReturned:
                                {
                                    if (order.OldCost == null)
                                        order.OldCost = order.Cost;
                                    order.Cost = item.NewAmount;
                                    order.OrderStateId = (int)OrderStateEnum.ShortageOfCash;
                                }
                                break;
                        }
                    }
                    else
                    {
                        switch (order.OrderplacedId)
                        {
                            case (int)OrderplacedEnum.PartialReturned:
                            case (int)OrderplacedEnum.Delivered:
                                {
                                    if (order.Cost != item.NewAmount)
                                    {
                                        if (order.OldCost == null)
                                            order.OldCost = order.Cost;
                                        order.Cost = item.NewAmount;
                                    }
                                }
                                break;
                            case (int)OrderplacedEnum.CompletelyReturned:
                                {
                                    if (order.OldCost == null)
                                    {
                                        order.OldCost = order.Cost;
                                    }
                                    order.Cost = 0;
                                    if (order.OldDeliveryCost == null)
                                        order.OldDeliveryCost = order.DeliveryCost;
                                    order.DeliveryCost = 0;
                                    order.AgentCost = 0;
                                }
                                break;
                            case (int)OrderplacedEnum.Unacceptable:
                                {
                                    if (order.OldCost == null)
                                        order.OldCost = order.Cost;
                                    order.Cost = 0;
                                }
                                break;

                        }
                    }
                    this._context.Update(order);
                    if (order.OrderStateId != (int)OrderStateEnum.Finished && order.OrderplacedId != (int)OrderplacedEnum.Way)
                    {
                        var clientNotigaction = notfications.Where(c => c.ClientId == order.ClientId && c.OrderPlacedId == order.OrderplacedId && c.MoneyPlacedId == order.MoenyPlacedId).FirstOrDefault();
                        if (clientNotigaction == null)
                        {
                            clientNotigaction = new Notfication()
                            {
                                ClientId = order.ClientId,
                                OrderPlacedId = item.OrderPlacedId,
                                MoneyPlacedId = (int)MoneyPalcedEnum.WithAgent,
                                IsSeen = false,
                                OrderCount = 1
                            };
                            notfications.Add(clientNotigaction);
                        }
                        else
                        {
                            clientNotigaction.OrderCount++;
                        }
                    }
                    Notfication notfication = new Notfication()
                    {
                        Note = $"الطلب {order.Code} اصبح {order.Orderplaced.Name} و موقع المبلغ  {order.MoenyPlaced.Name}",
                        ClientId = order.ClientId
                    };
                    this._context.Add(notfication);
                    addednotfications.Add(notfication);
                }
                foreach (var item in notfications)
                {
                    addednotfications.Add(item);
                    this._context.Add(item);
                }
                this._context.SaveChanges();
                {
                    var newnotifications = addednotfications.GroupBy(c => c.ClientId).ToList();
                    foreach (var item in newnotifications)
                    {
                        var key = item.Key;
                        List<NotficationDto> notficationDtos = new List<NotficationDto>();
                        foreach (var groupItem in item)
                        {
                            notficationDtos.Add(_mapper.Map<NotficationDto>(groupItem));
                        }
                        await notificationHub.AllNotification(key.ToString(), notficationDtos.ToArray());
                    }
                }
                transaction.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logging.WriteExption(ex);
                return BadRequest();
            }
        }
        [HttpPost("ForzenInWay")]
        public async Task<IActionResult> ForzenInWay([FromForm] FrozenOrder frozenOrder)
        {
            var query = this._context.Orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Way);
            if (frozenOrder.AgentId != null)
            {
                query = query.Where(c => c.AgentId == (int)frozenOrder.AgentId);
            }
            var date = frozenOrder.CurrentDate.AddHours(-frozenOrder.Hour);
            query = query.Where(c => c.Date <= date);
            query = query.Include(c => c.Client)
                 .Include(c => c.Region)
                 .Include(c => c.Agent)
                 .Include(c => c.Country)
                 .Include(c => c.Orderplaced)
                 .Include(c => c.MoenyPlaced);
            var orders = await query.ToListAsync();
            return Ok(_mapper.Map<OrderDto[]>(orders));
        }

    }
}