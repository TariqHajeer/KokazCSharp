using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExcelDataReader;
using KokazGoodsTransfer.Dtos.Clients;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.NotifcationDtos;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Dtos.ReceiptDtos;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.HubsConfig;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Static;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class COrderController : OldAbstractClientPolicyController
    {
        private readonly NotificationHub _notificationHub;
        private readonly IOrderClientSerivce _orderClientSerivce;
        private readonly INotificationService _notificationService;
        public COrderController(KokazContext context, IMapper mapper, Logging logging, NotificationHub notificationHub, IOrderClientSerivce orderClientSerivce, INotificationService notificationService) : base(context, mapper)
        {
            _notificationHub = notificationHub;
            _orderClientSerivce = orderClientSerivce;
            _notificationService = notificationService;
        }
        private async Task<List<string>> Validate(CreateOrderFromClient createOrderFromClient)
        {
            List<string> erros = new List<string>();
            if (await _orderClientSerivce.CodeExist(createOrderFromClient.Code))
            {
                erros.Add("الكود موجود مسبقاً");
            }
            if (createOrderFromClient.RecipientPhones.Length == 0)
            {
                erros.Add("رقم الهاتف مطلوب");
            }
            if (createOrderFromClient.OrderItem != null && createOrderFromClient.OrderItem.Count > 0)
            {
                var orderTypesIds = createOrderFromClient.OrderItem.Where(c => c.OrderTypeId != null).Select(c => c.OrderTypeId.Value).ToArray();
                if(await _orderClientSerivce.CheckOrderTypesIdsExists(orderTypesIds))
                {
                    erros.Add("النوع غير موجود");
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
                throw ex;
            }
        }
        [HttpPost("UploadExcel")]
        public async Task<IActionResult> UploadExcel(IFormFile file, [FromForm] DateTime dateTime)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            HashSet<string> errors = new HashSet<string>();
            var excelOrder = new List<OrderFromExcelDto>();
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                stream.Position = 0;
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        var order = new OrderFromExcelDto();
                        if (!reader.IsDBNull(0))
                        {
                            order.Code = reader.GetValue(0).ToString();
                        }
                        else
                        {
                            errors.Add("يجب ملئ الكود ");
                        }
                        if (!reader.IsDBNull(1))
                        {
                            order.RecipientName = reader.GetValue(1).ToString();
                        }
                        if (!reader.IsDBNull(2))
                        {
                            order.Country = reader.GetValue(2).ToString();
                        }
                        else
                        {
                            errors.Add("يجب ملئ المحافظة");
                        }
                        if (!reader.IsDBNull(3))
                        {
                            if (Decimal.TryParse(reader.GetValue(3).ToString(), out var d))
                            {
                                order.Cost = d;
                            }
                            else
                            {
                                errors.Add("كلفة الطلب ليست رقم");
                            }
                        }
                        else
                        {
                            errors.Add("كلفة الطلب إجبارية");
                        }
                        if (!reader.IsDBNull(4))
                        {
                            order.Address = reader.GetValue(4).ToString();
                        }
                        if (!reader.IsDBNull(5))
                        {
                            order.Phone = reader.GetValue(5).ToString();
                            if (order.Phone.Length > 15)
                            {
                                errors.Add("رقم الهاتف لا يجب ان يكون اكبر من 15 رقم");
                            }
                        }
                        else
                        {
                            errors.Add("رقم الهاتف إجباري");
                        }
                        if (!reader.IsDBNull(6))
                        {
                            order.Note = reader.GetValue(6).ToString();
                        }
                        excelOrder.Add(order);
                    }

                }
            }
            var codes = excelOrder.Select(c => c.Code);
            var similarOrders = await _context.Orders.Where(c => codes.Contains(c.Code) && c.ClientId == AuthoticateUserId()).Select(c => c.Code).ToListAsync();
            var simialrCodeInExcelTable = await _context.OrderFromExcels.Where(c => c.ClientId == AuthoticateUserId() && codes.Contains(c.Code)).Select(c => c.Code).ToListAsync();
            if (similarOrders.Any())
            {
                errors.Add($"الأكواد مكررة{string.Join(",", similarOrders)}");
            }
            if (simialrCodeInExcelTable.Any())
            {
                errors.Add($"الأكواد {string.Join(",", simialrCodeInExcelTable)} مكررة يجب مراجعة واجهة التصحيح");
            }
            if (errors.Any())
            {
                return Conflict(errors);
            }
            bool correct = false;
            var dbTransacrion = this._context.Database.BeginTransaction();
            try
            {
                var countriesName = excelOrder.Select(c => c.Country).Distinct().ToList();
                var countries = await _context.Countries.Where(c => countriesName.Contains(c.Name)).ToListAsync();
                foreach (var item in excelOrder)
                {
                    var country = countries.FirstOrDefault(c => c.Name == item.Country);
                    if (country == null)
                    {
                        var orderFromExcel = new OrderFromExcel()
                        {
                            Address = item.Address,
                            Code = item.Code,
                            Cost = item.Cost,
                            Country = item.Country,
                            Note = item.Note,
                            Phone = item.Phone,
                            RecipientName = item.RecipientName,
                            ClientId = AuthoticateUserId(),
                            CreateDate = dateTime
                        };
                        await _context.AddAsync(orderFromExcel);
                        correct = true;
                    }
                    else
                    {
                        var order = new Order()
                        {
                            Code = item.Code,
                            CountryId = country.Id,
                            Address = item.Address,
                            RecipientName = item.RecipientName,
                            RecipientPhones = item.Phone,
                            ClientNote = item.Note,
                            Cost = item.Cost,
                            Date = dateTime,
                            MoenyPlacedId = (int)MoneyPalcedEnum.OutSideCompany,
                            OrderplacedId = (int)OrderplacedEnum.Client,
                            OrderStateId = (int)OrderStateEnum.Processing,
                            ClientId = AuthoticateUserId(),
                            CreatedBy = AuthoticateUserName(),
                            DeliveryCost = country.DeliveryCost,
                            IsSend = false,
                        };
                        _context.Add(order);
                    }
                }

                await _context.SaveChangesAsync();
                await dbTransacrion.CommitAsync();
                var newOrdersDontSendCount = await this._context.Orders
                .Where(c => c.IsSend == false && c.OrderplacedId == (int)OrderplacedEnum.Client)
                .CountAsync();
                AdminNotification adminNotification = new AdminNotification()
                {
                    NewOrdersDontSendCount = newOrdersDontSendCount
                };
                await _notificationHub.AdminNotifcation(adminNotification);
                return Ok(correct);
            }
            catch (Exception ex)
            {
                await dbTransacrion.RollbackAsync();
                throw ex;
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
            return Ok(await _orderClientSerivce.CodeExist(code));
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
                .Include(c => c.OrderClientPaymnets)
                .ThenInclude(c => c.ClientPayment)
                .ThenInclude(c => c.ClientPaymentDetails)
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
                throw ex;
            }
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagingDto pagingDto, [FromQuery] COrderFilter orderFilter)
        {
            var paginResult = await _orderClientSerivce.Get(pagingDto, orderFilter);
            return Ok(new { data = paginResult.Data, total = paginResult.Total });
        }
        [HttpGet("NonSendOrder")]
        public async Task<IActionResult> NonSendOrder()
        {
            return Ok(await _orderClientSerivce.NonSendOrder());
        }
        [HttpPost("Sned")]
        public async Task<IActionResult> Send([FromBody] int[] ids)
        {
            await _orderClientSerivce.Send(ids);
            return Ok();
        }
        [HttpGet("OrdersDontFinished")]
        public async Task<IActionResult> OrdersDontFinished([FromQuery] OrderDontFinishFilter orderDontFinishFilter)
        {
            List<Order> orders = new List<Order>();

            if (orderDontFinishFilter.ClientDoNotDeleviredMoney)
            {
                var list = await this._context.Orders.Where(c => c.IsClientDiliverdMoney == false && orderDontFinishFilter.OrderPlacedId.Contains(c.OrderplacedId) && c.ClientId == AuthoticateUserId())
                   .Include(c => c.Region)
                   .Include(c => c.Country)
                   .Include(c => c.MoenyPlaced)
                   .Include(c => c.Orderplaced)
                   .Include(c => c.Agent)
                   .Include(c => c.OrderClientPaymnets)
                   .ThenInclude(c => c.ClientPayment)
                   .Include(c => c.AgentOrderPrints)
                   .ThenInclude(c => c.AgentPrint)
                   .ToListAsync();
                if (list != null && list.Count() > 0)
                {
                    orders.AddRange(list);
                }
            }
            if (orderDontFinishFilter.IsClientDeleviredMoney)
            {

                var list = await this._context.Orders.Where(c => c.OrderStateId == (int)OrderStateEnum.ShortageOfCash && orderDontFinishFilter.OrderPlacedId.Contains(c.OrderplacedId) && c.ClientId == AuthoticateUserId())
               .Include(c => c.Region)
               .Include(c => c.Country)
               .Include(c => c.Orderplaced)
               .Include(c => c.MoenyPlaced)
               .Include(c => c.Agent)
               .Include(c => c.OrderClientPaymnets)
                   .ThenInclude(c => c.ClientPayment)
                   .Include(c => c.AgentOrderPrints)
                   .ThenInclude(c => c.AgentPrint)
               .ToListAsync();
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
            var repiq = this._context.Receipts.Where(c => c.ClientId == AuthoticateUserId() && c.ClientPayment == null).ToList();
            return Ok(_mapper.Map<ReceiptDto[]>(repiq));
        }

        [HttpGet("NewNotfiaction")]
        public async Task<IActionResult> NewNotfiaction()
        {
            return Ok(await _notificationService.NewNotfiactionCount());
        }
        [HttpGet("Notifcation")]
        public async Task<IActionResult> Notifcation()
        {
            return Ok(await _notificationService.GetClientNotifcations());

        }
        [HttpPut("SeeNotifactions")]
        public async Task<IActionResult> SeeNotifactions([FromBody] int[] ids)
        {
            await _notificationService.SeeNotifactions(ids);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderClientSerivce.Delete(id);
            return Ok();
        }
        [HttpGet("OrdersNeedToRevision")]
        public async Task<IActionResult> OrdersNeedToRevision()
        {
            var orders = await _context.OrderFromExcels.Where(c => c.ClientId == AuthoticateUserId()).ToListAsync();
            return Ok(orders);
        }
        [HttpPut("CorrectOrderCountry")]
        public async Task<IActionResult> CorrectOrderCountry(List<KeyValuePair<int, int>> pairs)
        {
            var ids = pairs.Select(c => c.Key).ToList();
            var cids = pairs.Select(c => c.Value).ToList();

            var orders = await _context.OrderFromExcels.Where(c => ids.Contains(c.Id)).ToListAsync();
            var countries = await _context.Countries.Where(c => cids.Contains(c.Id)).ToListAsync();
            foreach (var item in pairs)
            {
                var ofe = orders.FirstOrDefault(c => c.Id == item.Key);
                if (ofe == null)
                    continue;
                var country = countries.FirstOrDefault(c => c.Id == item.Value);
                var order = new Order()
                {
                    Code = ofe.Code,
                    CountryId = item.Value,
                    Address = ofe.Address,
                    RecipientName = ofe.RecipientName,
                    RecipientPhones = ofe.Phone,
                    ClientNote = ofe.Note,
                    Cost = ofe.Cost,
                    Date = ofe.CreateDate,
                    MoenyPlacedId = (int)MoneyPalcedEnum.OutSideCompany,
                    OrderplacedId = (int)OrderplacedEnum.Client,
                    OrderStateId = (int)OrderStateEnum.Processing,
                    ClientId = AuthoticateUserId(),
                    CreatedBy = AuthoticateUserName(),
                    DeliveryCost = country.DeliveryCost,
                    IsSend = false,
                };
                _context.Add(order);
            }
            _context.OrderFromExcels.RemoveRange(orders);
            _context.SaveChanges();
            var newOrdersDontSendCount = await this._context.Orders
                .Where(c => c.IsSend == false && c.OrderplacedId == (int)OrderplacedEnum.Client)
                .CountAsync();
            AdminNotification adminNotification = new AdminNotification()
            {
                NewOrdersDontSendCount = newOrdersDontSendCount
            };
            await _notificationHub.AdminNotifcation(adminNotification);
            return Ok();
        }
    }
}