using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Common;
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
using KokazGoodsTransfer.DAL.Helper;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class OrderController : AbstractEmployeePolicyController
    {
        private readonly IIndexService<MoenyPlaced> _moneyPlacedIndexService;
        private readonly IIndexService<OrderPlaced> _orderPlacedIndexService;
        private readonly IOrderService _orderService;
        private readonly NotificationHub notificationHub;
        readonly ErrorMessage err;
        static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        public OrderController(KokazContext context, IMapper mapper, NotificationHub notificationHub, IIndexService<MoenyPlaced> moneyPlacedIndexService, IIndexService<OrderPlaced> orderPlacedIndexService, IOrderService orderService) : base(context, mapper)
        {
            err = new ErrorMessage
            {
                Controller = "Order"
            };
            this.notificationHub = notificationHub;
            _moneyPlacedIndexService = moneyPlacedIndexService;
            _orderPlacedIndexService = orderPlacedIndexService;
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagingDto pagingDto, [FromQuery] OrderFilter orderFilter)
        {
            var result = await _orderService.GetOrderFiltered(pagingDto, orderFilter);
            return Ok(new { data = result.Data, total = result.Total });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrdersFromEmployee createOrdersFromEmployee)
        {
            await _orderService.CreateOrder(createOrdersFromEmployee);
            return Ok();
        }
        [HttpPost("createMultiple")]
        public async Task<IActionResult> Create([FromBody] List<CreateMultipleOrder> createMultipleOrders)
        {
            await _orderService.CreateOrders(createMultipleOrders);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderService.Delete(id);
            return Ok();

        }
        [HttpGet("WithoutPaging")]
        public async Task<IActionResult> Get([FromQuery] OrderFilter orderFilter)
        {
            var result = await _orderService.GetOrderFiltered(null, orderFilter);
            return Ok(new { data = result.Data, total = result.Total });
        }
        [HttpPost("ForzenInWay")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> ForzenInWay([FromForm] FrozenOrder frozenOrder)
        {
            return Ok(await _orderService.ForzenInWay(frozenOrder));
        }
        [HttpGet("ReceiptOfTheOrderStatus/{id}")]
        public async Task<ActionResult<GenaricErrorResponse<ReceiptOfTheOrderStatusDetaliDto, string, IEnumerable<string>>>> ReceiptOfTheOrderStatusById(int id)
        {
            return Ok(await _orderService.GetReceiptOfTheOrderStatusById(id));
        }
        [HttpGet("ReceiptOfTheOrderStatus")]
        public async Task<ActionResult<PagingResualt<IEnumerable<ReceiptOfTheOrderStatusDto>>>> GetReceiptOfTheOrderStatus([FromQuery] PagingDto PagingDto, string code)
        {
            return Ok(await _orderService.GetReceiptOfTheOrderStatus(PagingDto, code));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _orderService.GetById(id));
        }
        [HttpGet("orderPlace")]
        public async Task<IActionResult> GetOrderPalce() => Ok(await _orderPlacedIndexService.GetAllLite());
        [HttpGet("MoenyPlaced")]
        public async Task<IActionResult> GetMoenyPlaced() => Ok(await _moneyPlacedIndexService.GetAllLite());
        [HttpPut("MakeOrderInWay")]
        public async Task<ActionResult<GenaricErrorResponse<int, string, string>>> MakeOrderInWay([FromBody] int[] ids)
        {
            var result = await _orderService.MakeOrderInWay(ids);
            return GetResult(result);
        }
        [HttpPut("ReceiptOfTheStatusOfTheDeliveredShipment")]
        public async Task<ActionResult<ErrorResponse<string, IEnumerable<string>>>> ReceiptOfTheStatusOfTheDeliveredShipment(IEnumerable<ReceiptOfTheStatusOfTheDeliveredShipmentWithCostDto> receiptOfTheStatusOfTheDeliveredShipmentWithCostDtos)
        {
            return Ok(await _orderService.ReceiptOfTheStatusOfTheDeliveredShipment(receiptOfTheStatusOfTheDeliveredShipmentWithCostDtos));
        }
        [HttpPut("ReceiptOfTheStatusOfTheReturnedShipment")]
        public async Task<ActionResult<ErrorResponse<string, IEnumerable<string>>>> ReceiptOfTheStatusOfTheReturnedShipment(IEnumerable<ReceiptOfTheStatusOfTheDeliveredShipmentDto> receiptOfTheStatusOfTheDeliveredShipmentDtos)
        {
            return Ok(await _orderService.ReceiptOfTheStatusOfTheReturnedShipment(receiptOfTheStatusOfTheDeliveredShipmentDtos));
        }
        [HttpGet("OrdersDontFinished")]
        public async Task<ActionResult<IEnumerable<PayForClientDto>>> Get([FromQuery] OrderDontFinishedFilter orderDontFinishedFilter)
        {
            return Ok(await _orderService.OrdersDontFinished(orderDontFinishedFilter));
        }



        [HttpGet("chekcCode")]
        public async Task<ActionResult<bool>> CheckCode([FromQuery] string code, int clientid)
        {
            return Ok(await _orderService.Any(c => c.ClientId == clientid && c.Code == code));
        }
        [HttpPost("CheckMulieCode/{clientId}")]
        public async Task<IActionResult> CheckMulieCode(int clientId, [FromBody] string[] codes)
        {
            return Ok(await _orderService.GetCodeStatuses(clientId, codes));
        }

        [HttpGet("NewOrders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetNewOrders()
        {
            var orders = await _orderService.GetAll(c => c.IsSend == true && c.OrderplacedId == (int)OrderplacedEnum.Client, new string[] { "Client.ClientPhones", "Client.Country", "Region", "Country.AgentCountries.Agent", "OrderItems.OrderTpye" });
            return Ok(orders);
        }

        [HttpGet("NewOrderDontSned")]
        public async Task<IActionResult> NewOrderDontSned()
        {
            return Ok(await _orderService.NewOrderDontSned());
        }

        [HttpGet("OrderAtClient")]
        public async Task<IActionResult> OrderAtClient([FromQuery] OrderFilter orderFilter)
        {
            return Ok(await _orderService.OrderAtClient(orderFilter));
        }
        [HttpGet("GetOrderToReciveFromAgent/{code}")]
        public async Task<ActionResult<GenaricErrorResponse<IEnumerable<OrderDto>, string, string>>> GetOrderToReciveFromAgent(string code)
        {
            return Ok(await _orderService.GetOrderToReciveFromAgent(code));
        }
        [HttpGet("GetOrderForPayBy/{clientId}/{code}")]
        public async Task<ActionResult<PayForClientDto>> GetByCodeAndClient(int clientId, string code)
        {
            return Ok(await _orderService.GetByCodeAndClient(clientId, code));
        }
        [HttpPut("ReiveMoneyFromClient")]
        public async Task<IActionResult> ReiveMoneyFromClient([FromBody] int[] ids)
        {
            await _orderService.ReiveMoneyFromClient(ids);
            return Ok();
        }
        [HttpGet("ShipmentsNotReimbursedToTheClient/{clientId}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> ShipmentsNotReimbursedToTheClient(int clientId)
        {
            var includes = new string[] { "Agent.UserPhones", "Region", "Country", "MoenyPlaced", "Orderplaced" };
            var orders = await _orderService.GetAsync(c => c.ClientId == clientId && c.IsClientDiliverdMoney == false && c.OrderplacedId >= (int)OrderplacedEnum.Way, includes);
            return Ok(orders);
        }
        [HttpGet("GetEarnings")]
        public async Task<IActionResult> GetEarnings([FromQuery] PagingDto pagingDto, [FromQuery] DateFiter dateFiter)
        {
            return Ok(await _orderService.GetEarnings(pagingDto, dateFiter));
        }
        /// <summary>
        /// طلبات في ذمة المندوب
        /// </summary>
        /// <param name="agnetId"></param>
        /// <returns></returns>
        [HttpGet("OrderVicdanAgent/{agnetId}")]
        public IActionResult OrderVicdanAgent(int agnetId)
        {
            var includes = new string[] { "Client", "Region", "Country", "Orderplaced", "MoenyPlaced" };
            var orders = _orderService.GetAsync(c => c.AgentId == agnetId && c.AgentRequestStatus == (int)AgentRequestStatusEnum.Pending || (c.MoenyPlacedId == (int)MoneyPalcedEnum.WithAgent) || (c.IsClientDiliverdMoney == true && c.OrderplacedId == (int)OrderplacedEnum.Way), includes);
            return Ok(orders);
        }

        [HttpPut("Accept")]
        public async Task<IActionResult> Accept([FromBody] IdsDto idsDto)
        {
            await _orderService.Accept(idsDto);
            return Ok();
        }
        [HttpPut("Acceptmultiple")]
        public async Task<IActionResult> AcceptMultiple([FromBody] List<IdsDto> idsDto)
        {
            await _orderService.AcceptMultiple(idsDto);
            return Ok();
        }

        [HttpPut("DisAccept")]
        public async Task<IActionResult> DisAccept([FromBody] DateWithId<int> dateWithId)
        {
            await _orderService.DisAccept(dateWithId);
            return Ok();
        }

        [HttpPut("DisAcceptmultiple")]
        public async Task<IActionResult> DisAcceptMultiple([FromBody] DateWithId<List<int>> dateWithIds)
        {
            await _orderService.DisAcceptMultiple(dateWithIds);
            return Ok();
        }

        [HttpPut("ReSend")]
        public async Task<IActionResult> ReSend([FromBody] OrderReSend orderReSend)
        {
            await _orderService.ReSend(orderReSend);
            return Ok();
        }

        [HttpPut("MakeStoreOrderCompletelyReturned")]
        public async Task<ActionResult<OrderDto>> MakeOrderCompletelyReturned([FromBody] int id)
        {
            return Ok(await _orderService.MakeOrderCompletelyReturned(id));
        }
        [HttpPatch("AddPrintNumber/{orderId}")]
        public async Task<IActionResult> AddPrintNumber(int orderId)
        {
            await _orderService.AddPrintNumber(orderId);
            return Ok();
        }
        [HttpPatch("AddPrintNumberMultiple")]
        public async Task<IActionResult> AddPrintNumber([FromBody] int[] orderids)
        {
            await _orderService.AddPrintNumber(orderids);
            return Ok();
        }
        [HttpGet("GetOrderByAgent/{orderCode}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrderByAgent(string orderCode)
        {
            return Ok(await _orderService.GetOrderByAgent(orderCode));
        }

        [HttpPut("TransferOrderToAnotherAgnet")]
        public async Task<IActionResult> TransferOrderToAnotherAgnet([FromBody] TransferOrderToAnotherAgnetDto transferOrderToAnotherAgnetDto)
        {
            await _orderService.TransferOrderToAnotherAgnet(transferOrderToAnotherAgnetDto);
            return Ok();
        }
        [HttpPatch]
        public async Task<IActionResult> Edit([FromBody] UpdateOrder updateOrder)
        {
            await _orderService.Edit(updateOrder);
            return Ok();

        }

        /// <summary>
        /// تسديد العميل
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut("DeleiverMoneyForClient")]
        public async Task<IActionResult> DeleiverMoneyForClient([FromBody] DeleiverMoneyForClientDto deleiverMoneyForClientDto)
        {
            var id = await _orderService.DeleiverMoneyForClient(deleiverMoneyForClientDto);
            return Ok(new { printNumber = id });
        }
    }
    public partial class OrderController
    {


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

        [HttpGet("GetClientprint")]
        public async Task<IActionResult> GetClientprint([FromQuery] PagingDto pagingDto, [FromQuery] int? number, string clientName, string code)
        {
            var clientPaymentIq = this._context.ClientPayments.AsQueryable();

            if (number != null)
            {
                clientPaymentIq = clientPaymentIq.Where(c => c.Id == number);
            }
            if (clientName != null)
            {
                clientPaymentIq = clientPaymentIq.Where(c => c.DestinationName == clientName);
            }
            if (!string.IsNullOrEmpty(code))
            {
                clientPaymentIq = clientPaymentIq.Where(c => c.ClientPaymentDetails.Any(c => c.Code.StartsWith(code)));
            }

            var total = await clientPaymentIq.CountAsync();
            var clientPayments = await clientPaymentIq.OrderByDescending(c => c.Id).Skip((pagingDto.Page - 1) * pagingDto.RowCount).Take(pagingDto.RowCount)
                .Include(c => c.ClientPaymentDetails)
                .ThenInclude(c => c.OrderPlaced)
                .ToListAsync();

            return Ok(new { data = _mapper.Map<PrintOrdersDto[]>(clientPayments), total });
        }
        [HttpGet("GetAgentPrint")]
        public async Task<IActionResult> GetAgentPrint([FromQuery] PagingDto pagingDto, [FromQuery] int? number, string agnetName)
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
            var total = await ordersPrint.CountAsync();
            var orders = await ordersPrint.OrderByDescending(c => c.Id).Skip((pagingDto.Page - 1) * pagingDto.RowCount).Take(pagingDto.RowCount).ToListAsync();
            return Ok(new { data = _mapper.Map<PrintOrdersDto[]>(orders), total });
        }
       
        /// <summary>
        /// تسديد الشركات
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut("DeleiverMoneyForClientWithStatus")]
        public async Task<IActionResult> DeleiverMoneyForClientWithStatus(int[] ids)
        {
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

            var clientPaymnet = new ClientPayment()
            {
                Date = DateTime.UtcNow,
                PrinterName = User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value,
                DestinationName = client.Name,
                DestinationPhone = client.ClientPhones.FirstOrDefault()?.Phone ?? "",
            };
            var total = 0m;
            var transaction = this._context.Database.BeginTransaction();
            try
            {
                this._context.Add(clientPaymnet);
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
                    var orderClientPayment = new OrderClientPaymnet()
                    {
                        OrderId = item.Id,
                        ClientPaymentId = clientPaymnet.Id
                    };
                    var clientPaymnetDetial = new ClientPaymentDetail()
                    {
                        Code = item.Code,
                        Total = item.Cost,
                        Country = item.Country.Name,
                        ClientPaymentId = clientPaymnet.Id,
                        Phone = item.RecipientPhones,
                        DeliveryCost = item.DeliveryCost,
                        MoneyPlacedId = item.MoenyPlacedId,
                        OrderPlacedId = item.OrderplacedId,
                        LastTotal = item.OldCost,
                        PayForClient = cureentPay,
                        Date = item.Date,
                        Note = item.Note,
                        ClientNote = item.ClientNote
                    };
                    total += cureentPay;
                    this._context.Add(orderClientPayment);
                    this._context.Add(clientPaymnetDetial);
                }
                this._context.SaveChanges();
                var treasury = await _context.Treasuries.FindAsync(AuthoticateUserId());
                treasury.Total -= total;
                var history = new TreasuryHistory()
                {
                    ClientPaymentId = clientPaymnet.Id,
                    TreasuryId = AuthoticateUserId(),
                    Amount = -total,
                    CreatedOnUtc = DateTime.UtcNow
                };
                this._context.Update(treasury);
                await this._context.AddAsync(history);
                await _context.SaveChangesAsync();
                transaction.Commit();
                semaphore.Release();
                return Ok(new { printNumber = clientPaymnet.Id });
            }
            catch (Exception ex)
            {
                semaphore.Release();
                transaction.Rollback();
                throw ex;
            }

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
        public async Task<IActionResult> GetOrderByClientPrintNumber([FromQuery] int printNumber)
        {
            var printed = await this._context.ClientPayments.Where(c => c.Id == printNumber)
                .Include(c => c.Discounts)
                .Include(c => c.Receipts)
                .Include(c => c.ClientPaymentDetails)
                 .ThenInclude(c => c.OrderPlaced)
                .FirstOrDefaultAsync();
            if (printed == null)
            {
                this.err.Messges.Add($"رقم الطباعة غير موجود");
                return Conflict(this.err);
            }
            var x = _mapper.Map<PrintOrdersDto>(printed);
            return Ok(x);
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
                        List<NotificationDto> notficationDtos = new List<NotificationDto>();
                        foreach (var groupItem in item)
                        {
                            notficationDtos.Add(_mapper.Map<NotificationDto>(groupItem));
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
                throw ex;
            }
        }

    }
}