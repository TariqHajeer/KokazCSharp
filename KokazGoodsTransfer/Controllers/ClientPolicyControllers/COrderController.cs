using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Clients;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.NotifcationDtos;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Dtos.ReceiptDtos;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.HubsConfig;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class COrderController : AbstractClientPolicyController
    {
        private readonly NotificationHub _notificationHub;
        public COrderController(KokazContext context, IMapper mapper, Logging logging, NotificationHub notificationHub) : base(context, mapper, logging)
        {
            _notificationHub = notificationHub;
        }
        private async Task<List<string>> Validate(CreateOrderFromClient createOrderFromClient)
        {
            List<string> erros = new List<string>();
            if (await CodeExist(createOrderFromClient.Code))
            {
                erros.Add("الكود موجود مسبقاً");
            }
            if (this._context.Countries.Find(createOrderFromClient.CountryId) == null)
            {
                erros.Add("المدينة غير موجودة");
            }
            if (createOrderFromClient.RecipientPhones.Length == 0)
            {
                erros.Add("رقم الهاتف مطلوب");
            }
            if (createOrderFromClient.OrderItem != null && createOrderFromClient.OrderItem.Count > 0)
            {
                foreach (var item in createOrderFromClient.OrderItem)
                {
                    if (item.OrderTypeId != null)
                    {
                        var orderType = this._context.OrderTypes.Find(item.OrderTypeId);
                        if (orderType == null)
                        {
                            erros.Add("النوع غير موجود");
                            break;
                        }
                    }
                }
            }
            return erros;
        }
        /// <summary>
        /// إضافة طلب
        /// </summary>
        /// <param name="createOrderFromClient"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderFromClient createOrderFromClient)
        {
            var dbTransacrion = this._context.Database.BeginTransaction();
            try
            {
                var validate = await this.Validate(createOrderFromClient);
                if (validate.Count != 0)
                {
                    return Conflict(new { messages = validate });
                }

                var country = this._context.Countries.Find(createOrderFromClient.CountryId);
                var order = _mapper.Map<Order>(createOrderFromClient);
                order.ClientId = AuthoticateUserId();
                order.CreatedBy = AuthoticateUserName();
                order.DeliveryCost = country.DeliveryCost;
                order.CreatedBy = AuthoticateUserName();
                order.MoenyPlacedId = (int)MoneyPalcedEnum.OutSideCompany;
                order.OrderplacedId = (int)OrderplacedEnum.Client;
                order.OrderStateId = (int)OrderStateEnum.Processing;
                order.RecipientPhones = String.Join(',', createOrderFromClient.RecipientPhones);
                order.IsSend = false;
                order.CurrentCountry = this._context.Countries.Where(c => c.IsMain == true).FirstOrDefault().Id;
                this._context.Add(order);
                this._context.SaveChanges();
                var orderItem = createOrderFromClient.OrderItem;

                if (orderItem != null)
                {
                    foreach (var item in orderItem)
                    {
                        int orderTypeId;
                        if (item.OrderTypeId == null)
                        {
                            if (item.OrderTypeName == "")
                                return Conflict();
                            var similerOrderType = this._context.OrderTypes.Where(c => c.Name == item.OrderTypeName).FirstOrDefault();
                            if (similerOrderType == null)
                            {
                                var orderType = new OrderType()
                                {
                                    Name = item.OrderTypeName,
                                };
                                this._context.Add(orderType);
                                this._context.SaveChanges();
                                orderTypeId = orderType.Id;

                            }
                            else
                            {
                                orderTypeId = similerOrderType.Id;
                            }
                        }
                        else
                        {
                            orderTypeId = (int)item.OrderTypeId;
                        }
                        this._context.Add(new OrderItem()
                        {
                            OrderTpyeId = orderTypeId,
                            Count = item.Count,
                            OrderId = order.Id
                        });
                        this._context.SaveChanges();
                    }
                }
                await dbTransacrion.CommitAsync();
                var newOrdersDontSendCount = await this._context.Orders
                .Where(c => c.IsSend == false && c.OrderplacedId == (int)OrderplacedEnum.Client)
                .CountAsync();
                AdminNotification adminNotification = new AdminNotification()
                {
                    NewOrdersDontSendCount = newOrdersDontSendCount
                };
                await _notificationHub.AdminNotifcation(adminNotification);
                return Ok(_mapper.Map<OrderResponseClientDto>(order));
            }

            catch (Exception ex)
            {
                dbTransacrion.Rollback();
                _logging.WriteExption(ex);
                return BadRequest();

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("codeExist")]
        public async Task<IActionResult> CheckCodeExist([FromQuery] string code)
        {
            return Ok(await CodeExist(code));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {

            var order = await this._context.Orders
                .Include(c => c.Agent)
                .Include(c => c.Country)
                .Include(c => c.Orderplaced)
                .Include(c => c.MoenyPlaced)
                .Include(c => c.OrderItems)
                    .ThenInclude(c => c.OrderTpye)
                .Include(c => c.OrderPrints)
                    .ThenInclude(c => c.Print)
                        .ThenInclude(c => c.ClientPrints)
            .FirstOrDefaultAsync(c => c.Id == id);

            return Ok(_mapper.Map<OrderDto>(order));

        }
        [HttpPut]
        public IActionResult Edit([FromBody] EditOrder editOrder)
        {
            var order = this._context.Orders.Find(editOrder.Id);

            this._context.Entry(order).Collection(c => c.OrderItems).Load();
            order.Code = editOrder.Code;
            order.CountryId = editOrder.CountryId;
            order.Address = editOrder.Address;
            order.RecipientName = editOrder.RecipientName;
            order.ClientNote = editOrder.ClientNote;
            order.Cost = editOrder.Cost;
            order.Date = editOrder.Date;
            var country = this._context.Countries.Find(editOrder.CountryId);
            order.DeliveryCost = country.DeliveryCost;
            order.RecipientPhones = String.Join(',', editOrder.RecipientPhones);
            var transaction = this._context.Database.BeginTransaction();
            try
            {
                this._context.Update(order);
                this._context.SaveChanges();
                order.OrderItems.Clear();
                this._context.SaveChanges();

                foreach (var item in editOrder.OrderItem)
                {
                    int orderTypeId;
                    if (item.OrderTypeId == null)
                    {
                        if (item.OrderTypeName == "")
                            return Conflict();
                        var similerOrderType = this._context.OrderTypes.Where(c => c.Name == item.OrderTypeName).FirstOrDefault();
                        if (similerOrderType == null)
                        {
                            var orderType = new OrderType()
                            {
                                Name = item.OrderTypeName,
                            };
                            this._context.Add(orderType);
                            this._context.SaveChanges();
                            orderTypeId = orderType.Id;

                        }
                        else
                        {
                            orderTypeId = similerOrderType.Id;
                        }
                    }
                    else
                    {
                        orderTypeId = (int)item.OrderTypeId;
                    }
                    this._context.Add(new OrderItem()
                    {
                        OrderTpyeId = orderTypeId,
                        Count = item.Count,
                        OrderId = order.Id
                    });
                    this._context.SaveChanges();
                }
                transaction.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logging.WriteExption(ex);
                return Conflict();
            }
        }
        async Task<bool> CodeExist(string code)
        {
            return await this._context.Orders.Where(c => c.Code == code && c.ClientId == AuthoticateUserId()).AnyAsync();
        }
        [HttpGet]
        public IActionResult Get([FromQuery] PagingDto pagingDto, [FromQuery] COrderFilter orderFilter)
        {
            var orderIQ = this._context.Orders
                .Where(c => c.ClientId == AuthoticateUserId());
            if (orderFilter.CountryId != null)
            {
                orderIQ = orderIQ.Where(c => c.CountryId == orderFilter.CountryId);
            }
            if (orderFilter.Code != string.Empty && orderFilter.Code != null)
            {
                orderIQ = orderIQ.Where(c => c.Code.StartsWith(orderFilter.Code));
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
            if (orderFilter.IsClientDiliverdMoney != null)
            {
                orderIQ = orderIQ.Where(c => c.IsClientDiliverdMoney == orderFilter.IsClientDiliverdMoney);
            }
            if (orderFilter.ClientPrintNumber != null)
            {
                orderIQ = orderIQ.Where(c => c.OrderPrints.Any(op => op.Print.PrintNmber == orderFilter.ClientPrintNumber && op.Print.Type == PrintType.Client));
            }
            var total = orderIQ.Count();
            var orders = orderIQ.Skip((pagingDto.Page - 1) * pagingDto.RowCount).Take(pagingDto.RowCount)
                .Include(c => c.Country)
                .Include(c => c.Orderplaced)
                .Include(c => c.MoenyPlaced)
                .Include(c => c.OrderItems)
                    .ThenInclude(c => c.OrderTpye)
                .Include(c => c.OrderPrints)
                    .ThenInclude(c => c.Print)
                    .ThenInclude(c => c.ClientPrints)
                .ToList();
            return Ok(new { data = _mapper.Map<OrderDto[]>(orders), total });
        }
        [HttpGet("NonSendOrder")]
        public IActionResult NonSendOrder()
        {
            var orders = this._context.Orders
                .Include(c => c.Country)
                .Include(c => c.MoenyPlaced)
                .Include(c => c.Orderplaced)
                .Where(c => c.IsSend == false && c.ClientId == AuthoticateUserId()).ToList();
            return Ok(_mapper.Map<OrderDto[]>(orders));
        }
        [HttpPost("Sned")]
        public async Task<IActionResult> Send([FromBody] int[] ids)
        {
            var sendOrder = await this._context.Orders.Where(c => ids.Contains(c.Id)).ToListAsync();
            sendOrder.ForEach(c => c.IsSend = true);
            await this._context.SaveChangesAsync();
            var newOrdersCount = await this._context.Orders
                .Where(c => c.IsSend == true && c.OrderplacedId == (int)OrderplacedEnum.Client)
                .CountAsync();
            var newOrdersDontSendCount = await this._context.Orders
                .Where(c => c.IsSend == false && c.OrderplacedId == (int)OrderplacedEnum.Client)
                .CountAsync();
            AdminNotification adminNotification = new AdminNotification()
            {
                NewOrdersCount = newOrdersCount,
                NewOrdersDontSendCount = newOrdersDontSendCount
            };
            await _notificationHub.AdminNotifcation(adminNotification);
            return Ok();
        }
        [HttpGet("OrdersDontFinished")]
        public IActionResult OrdersDontFinished([FromQuery] OrderDontFinishFilter orderDontFinishFilter)
        {
            List<Order> orders = new List<Order>();

            if (orderDontFinishFilter.ClientDoNotDeleviredMoney)
            {
                var list = this._context.Orders.Where(c => c.IsClientDiliverdMoney == false && orderDontFinishFilter.OrderPlacedId.Contains(c.OrderplacedId) && c.ClientId == AuthoticateUserId())
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
            if (orderDontFinishFilter.IsClientDeleviredMoney)
            {

                var list = this._context.Orders.Where(c => c.OrderStateId == (int)OrderStateEnum.ShortageOfCash && orderDontFinishFilter.OrderPlacedId.Contains(c.OrderplacedId) && c.ClientId == AuthoticateUserId())
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
            orders.ForEach(o =>
            {
                if (o.MoenyPlacedId == (int)MoneyPalcedEnum.WithAgent)
                {
                    o.MoenyPlacedId = (int)MoneyPalcedEnum.OutSideCompany;
                    o.MoenyPlaced = _context.MoenyPlaceds.Find(o.MoenyPlacedId);
                }
            });
            var o = _mapper.Map<PayForClientDto[]>(orders);
            return Ok(o);
        }
        [HttpGet("UnPaidRecipt")]
        public IActionResult UnPaidRecipt()
        {
            var repiq = this._context.Receipts.Where(c => c.ClientId == AuthoticateUserId() && c.PrintId == null).ToList();
            return Ok(_mapper.Map<ReceiptDto[]>(repiq));
        }

        [HttpGet("NewNotfiaction")]
        public IActionResult NewNotfiaction()
        {
            return Ok(this._context.Notfications.Where(c => c.ClientId == AuthoticateUserId() && c.IsSeen != true).Count());
        }
        [HttpGet("Notifcation")]
        public IActionResult Notifcation()
        {
            var notifactions = this._context.Notfications.Include(c => c.MoneyPlaced)
                .Include(c => c.OrderPlaced)
                .Where(c => c.ClientId == AuthoticateUserId() && c.IsSeen != true)
                .OrderByDescending(c => c.Id);
            var response = _mapper.Map<NotficationDto[]>(notifactions);
            response = response.OrderBy(c => c.Note).ThenBy(c => c.Id).ToArray();
            return Ok(response);

        }
        [HttpPut("SeeNotifactions")]
        public async Task<IActionResult> SeeNotifactions([FromBody] int[] ids)
        {
            var notfications = await this._context.Notfications.Where(c => ids.Contains(c.Id)).ToListAsync();
            notfications.ForEach(c =>
            {
                c.IsSeen = true;
                this._context.Update(c);
            });
            await this._context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await this._context.Orders.FindAsync(id);
            if (order.IsSend == true)
                return Conflict();
            this._context.Remove(order);
            await this._context.SaveChangesAsync();
            return Ok();
        }

    }
}