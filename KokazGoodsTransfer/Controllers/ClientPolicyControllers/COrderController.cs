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
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class COrderController : AbstractClientPolicyController
    {
        public COrderController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
        private List<string> Validate(CreateOrderFromClient createOrderFromClient)
        {
            List<string> erros = new List<string>();
            if (CodeExist(createOrderFromClient.Code))
            {
                erros.Add("الكود موجود مسبقاً");
            }
            if (this.Context.Countries.Find(createOrderFromClient.CountryId) == null)
            {
                erros.Add("المدينة غير موجودة");
            }
            //if (createOrderFromClient.RegionId != null)
            //{
            //    var region = this.Context.Regions.Find(createOrderFromClient.RegionId);
            //    if (region == null || region.CountryId != createOrderFromClient.CountryId)
            //    {
            //        erros.Add("المنطقة غير موجودة");
            //    }
            //}
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
                        var orderType = this.Context.OrderTypes.Find(item.OrderTypeId);
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
        [HttpPost]
        public IActionResult Create(CreateOrderFromClient createOrderFromClient)
        {
            var dbTransacrion = this.Context.Database.BeginTransaction();
            try
            {
                var validate = this.Validate(createOrderFromClient);
                if (validate.Count != 0)
                {
                    return Conflict(new { messages = validate });
                }

                var country = this.Context.Countries.Find(createOrderFromClient.CountryId);
                var order = mapper.Map<Order>(createOrderFromClient);
                order.ClientId = AuthoticateUserId();
                order.CreatedBy = AuthoticateUserName();
                order.DeliveryCost = country.DeliveryCost;
                order.CreatedBy = AuthoticateUserName();
                order.MoenyPlacedId = (int)MoneyPalcedEnum.OutSideCompany;
                order.OrderplacedId = (int)OrderplacedEnum.Client;
                order.OrderStateId = (int)OrderStateEnum.Processing;
                order.RecipientPhones = String.Join(',', createOrderFromClient.RecipientPhones);
                order.IsSend = false;
                this.Context.Add(order);
                this.Context.SaveChanges();
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
                            var similerOrderType = this.Context.OrderTypes.Where(c => c.Name == item.OrderTypeName).FirstOrDefault();
                            if (similerOrderType == null)
                            {
                                var orderType = new OrderType()
                                {
                                    Name = item.OrderTypeName,
                                };
                                this.Context.Add(orderType);
                                this.Context.SaveChanges();
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
                        this.Context.Add(new OrderItem()
                        {
                            OrderTpyeId = orderTypeId,
                            Count = item.Count,
                            OrderId = order.Id
                        });
                        this.Context.SaveChanges();
                    }
                }
                dbTransacrion.Commit();
                return Ok(mapper.Map<OrderResponseClientDto>(order));
            }

            catch (Exception ex)
            {
                dbTransacrion.Rollback();
                return BadRequest();

            }
        }
        [HttpGet("codeExist")]
        public IActionResult CheckCodeExist([FromQuery] string code)
        {
            return Ok(CodeExist(code));
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {

            var order = this.Context.Orders
                .Include(c => c.Country)
                .Include(c => c.Orderplaced)
                .Include(c => c.MoenyPlaced)
                .Include(c => c.OrderItems)
                    .ThenInclude(c => c.OrderTpye)
                .Include(c => c.OrderPrints)
                    .ThenInclude(c => c.Print)
                        .ThenInclude(c => c.ClientPrints)
            .FirstOrDefault(c => c.Id == id);
            //if(order.ClientId!=AuthoticateUserId())
            return Ok(mapper.Map<OrderDto>(order));

        }
        [HttpPut]
        public IActionResult Edit([FromBody] EditOrder editOrder)
        {
            var order = this.Context.Orders.Find(editOrder.Id);

            this.Context.Entry(order).Collection(c => c.OrderItems).Load();
            order.Code = editOrder.Code;
            order.CountryId = editOrder.CountryId;
            order.Address = editOrder.Address;
            order.RecipientName = editOrder.RecipientName;
            order.ClientNote = editOrder.ClientNote;
            order.Cost = editOrder.Cost;
            order.Date = editOrder.Date;
            var country = this.Context.Countries.Find(editOrder.CountryId);
            order.DeliveryCost = country.DeliveryCost;
            order.RecipientPhones = String.Join(',', editOrder.RecipientPhones);
            var transaction = this.Context.Database.BeginTransaction();
            try
            {
                this.Context.Update(order);
                this.Context.SaveChanges();
                order.OrderItems.Clear();
                this.Context.SaveChanges();

                foreach (var item in editOrder.OrderItem)
                {
                    int orderTypeId;
                    if (item.OrderTypeId == null)
                    {
                        if (item.OrderTypeName == "")
                            return Conflict();
                        var similerOrderType = this.Context.OrderTypes.Where(c => c.Name == item.OrderTypeName).FirstOrDefault();
                        if (similerOrderType == null)
                        {
                            var orderType = new OrderType()
                            {
                                Name = item.OrderTypeName,
                            };
                            this.Context.Add(orderType);
                            this.Context.SaveChanges();
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
                    this.Context.Add(new OrderItem()
                    {
                        OrderTpyeId = orderTypeId,
                        Count = item.Count,
                        OrderId = order.Id
                    });
                    this.Context.SaveChanges();
                }
                transaction.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return Conflict();
            }
        }
        bool CodeExist(string code)
        {
            if (this.Context.Orders.Where(c => c.Code == code && c.ClientId == AuthoticateUserId()).Any())
            {
                return true;
            }
            return false;
        }
        [HttpGet]

        public IActionResult Get([FromQuery] PagingDto pagingDto, [FromQuery]COrderFilter orderFilter)
        {
            var orderIQ = this.Context.Orders
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
            return Ok(new { data = mapper.Map<OrderDto[]>(orders), total });
        }
        [HttpGet("NonSendOrder")]
        public IActionResult NonSendOrder()
        {
            var orders = this.Context.Orders
                .Include(c => c.Country)
                .Include(c => c.MoenyPlaced)
                .Include(c => c.Orderplaced)
                .Where(c => c.IsSend == false && c.ClientId == AuthoticateUserId()).ToList();
            return Ok(mapper.Map<OrderDto[]>(orders));
        }
        [HttpPost("Sned")]
        public IActionResult Send([FromBody] int[] ids)
        {
            var sendOrder = this.Context.Orders.Where(c => ids.Contains(c.Id)).ToList();
            sendOrder.ForEach(c => c.IsSend = true);
            this.Context.SaveChanges();
            return Ok();
        }
        [HttpGet("OrdersDontFinished")]
        public IActionResult OrdersDontFinished([FromQuery]OrderDontFinishFilter orderDontFinishFilter)
        {
            List<Order> orders = new List<Order>();

            if (orderDontFinishFilter.ClientDoNotDeleviredMoney)
            {
                var list = this.Context.Orders.Where(c => c.IsClientDiliverdMoney == false && orderDontFinishFilter.OrderPlacedId.Contains(c.OrderplacedId) && c.ClientId == AuthoticateUserId())
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

                var list = this.Context.Orders.Where(c => c.OrderStateId == (int)OrderStateEnum.ShortageOfCash && orderDontFinishFilter.OrderPlacedId.Contains(c.OrderplacedId) && c.ClientId == AuthoticateUserId())
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
            var o = mapper.Map<OrderDto[]>(orders);
            return Ok(o);
        }
        [HttpGet("UnPaidRecipt")]
        public IActionResult UnPaidRecipt()
        {
            var repiq = this.Context.Receipts.Where(c => c.ClientId == AuthoticateUserId() && c.PrintId == null).ToList();
            return Ok(mapper.Map<ReceiptDto[]>(repiq));
        }

        [HttpGet("NewNotfiaction")]
        public IActionResult NewNotfiaction()
        {
            return Ok(this.Context.Notfications.Where(c => c.ClientId == AuthoticateUserId() && c.IsSeen != true).Count());
        }
        [HttpGet("Notifcation")]
        public IActionResult Notifcation([FromQuery]PagingDto pagingDto)
        {
            var notifactions = this.Context.Notfications
                .Include(c => c.MoneyPlaced)
                .Include(c => c.OrderPlaced)
                .Where(c => c.ClientId == AuthoticateUserId());
            var total = notifactions.Count();
            notifactions = notifactions.OrderByDescending(c => c.Id).Skip(pagingDto.Page - 1).Take(pagingDto.RowCount);
            return Ok(new { Total = total, Data = mapper.Map<NotficationDto[]>(notifactions) });
        }
        [HttpPut("SeeNotifactions")]
        public IActionResult SeeNotifactions([FromBody] int[] ids)
        {
            var notfications = this.Context.Notfications.Where(c => ids.Contains(c.Id)).ToList();
            notfications.ForEach(c =>
            {
                c.IsSeen = true;
                this.Context.Update(c);
            });
            this.Context.SaveChanges();
            return Ok();
        }

    }
}