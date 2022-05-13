using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Models;
using System.Linq;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Models.Static;
using System;
using AutoMapper;
using KokazGoodsTransfer.DAL.Helper;

namespace KokazGoodsTransfer.Services.Concret
{
    public class OrderService : IOrderService
    {
        private readonly IUintOfWork _uintOfWork;
        private readonly INotificationService _notificationService;
        private readonly ITreasuryService _treasuryService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IRepository<ReceiptOfTheOrderStatus> _receiptOfTheOrderStatusRepository;
        private static readonly Func<Order, bool> _finishOrderExpression = c => c.OrderplacedId == (int)OrderplacedEnum.CompletelyReturned || c.OrderplacedId == (int)OrderplacedEnum.Unacceptable
|| (c.OrderplacedId == (int)OrderplacedEnum.Delivered && (c.MoenyPlacedId == (int)MoneyPalcedEnum.InsideCompany || c.MoenyPlacedId == (int)MoneyPalcedEnum.Delivered));
        public OrderService(IUintOfWork uintOfWork, INotificationService notificationService, ITreasuryService treasuryService, IMapper mapper, IUserService userService, IRepository<ReceiptOfTheOrderStatus> receiptOfTheOrderStatusRepository)
        {
            _uintOfWork = uintOfWork;
            _notificationService = notificationService;
            _treasuryService = treasuryService;
            _mapper = mapper;
            _userService = userService;
            _receiptOfTheOrderStatusRepository = receiptOfTheOrderStatusRepository;
        }

        public async Task<GenaricErrorResponse<IEnumerable<OrderDto>, string, IEnumerable<string>>> GetOrderToReciveFromAgent(string code)
        {
            var orders = await _uintOfWork.Repository<Order>().GetAsync(c => c.Code == code, c => c.Client, c => c.Agent, c => c.MoenyPlaced, c => c.Orderplaced, c => c.Country);
            var lastOrderAdded = orders.OrderBy(c => c.Id).Last();
            if (!orders.Any())
            {
                return new GenaricErrorResponse<IEnumerable<OrderDto>, string, IEnumerable<string>>("الشحنة غير موجودة");
            }
            ///التأكد من ان الشحنة ليست عند العميل او في المخزن
            {
                var orderInSotre = orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Store);
                var orderWithClient = orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Client);
                orders = orders.Except(orderInSotre.Union(orderWithClient));
            }
            {
                var finishOrders = orders.Where(_finishOrderExpression);
                orders = orders.Except(finishOrders);
            }
            {
                var orderInCompany = orders.Where(c => c.MoenyPlacedId == (int)MoneyPalcedEnum.InsideCompany).ToList();
                orders = orders.Except(orderInCompany);
            }
            if (!orders.Any())
            {
                var lastOrderPlacedAdded = lastOrderAdded.OrderplacedId;
                if (lastOrderAdded.OrderplacedId == (int)OrderplacedEnum.Store)
                    return new GenaricErrorResponse<IEnumerable<OrderDto>, string, IEnumerable<string>>("الشحنة في المخزن");
                if (lastOrderAdded.OrderplacedId == (int)OrderplacedEnum.Client)
                    return new GenaricErrorResponse<IEnumerable<OrderDto>, string, IEnumerable<string>>("الشحنة عند العميل");
                if (lastOrderAdded.OrderplacedId == (int)MoneyPalcedEnum.InsideCompany)
                    return new GenaricErrorResponse<IEnumerable<OrderDto>, string, IEnumerable<string>>("الشحنة داخل الشركة");
            }
            return new GenaricErrorResponse<IEnumerable<OrderDto>, string, IEnumerable<string>>(_mapper.Map<OrderDto[]>(orders));
        }
        public async Task<ErrorResponse<string, IEnumerable<string>>> ReceiptOfTheStatusOfTheDeliveredShipment(IEnumerable<ReceiptOfTheStatusOfTheDeliveredShipmentWithCostDto> receiptOfTheStatusOfTheDeliveredShipmentWithCostDtos)
        {
            var moneyPlacedes = await _uintOfWork.Repository<MoenyPlaced>().GetAll();
            var orderPlacedes = await _uintOfWork.Repository<OrderPlaced>().GetAll();
            var outSideCompny = moneyPlacedes.First(c => c.Id == (int)MoneyPalcedEnum.OutSideCompany).Name;
            var response = new ErrorResponse<string, IEnumerable<string>>();
            if (!receiptOfTheStatusOfTheDeliveredShipmentWithCostDtos.All(c => c.OrderplacedId == OrderplacedEnum.Way || c.OrderplacedId == OrderplacedEnum.Delivered || c.OrderplacedId == OrderplacedEnum.PartialReturned))
            {
                var wrongData = receiptOfTheStatusOfTheDeliveredShipmentWithCostDtos.Where(c => !(c.OrderplacedId == OrderplacedEnum.Way || c.OrderplacedId == OrderplacedEnum.Delivered || c.OrderplacedId == OrderplacedEnum.PartialReturned));
                var worngDataIds = wrongData.Select(c => c.Id);
                var worngOrders = await _uintOfWork.Repository<Order>().GetAsync(c => worngDataIds.Contains(c.Id));
                List<string> errors = new List<string>();
                foreach (var item in receiptOfTheStatusOfTheDeliveredShipmentWithCostDtos)
                {
                    string code = worngOrders.Where(c => c.Id == item.Id).FirstOrDefault()?.Code;
                    errors.Add($"لا يمكن وضع حالة الشحنة {OrderPlacedEnumToString(item.OrderplacedId)} للشحنة ذات الرقم : {code}");
                }
                response.Errors = errors;
                return response;
            }
            List<Notfication> notfications = new List<Notfication>();
            List<Notfication> addednotfications = new List<Notfication>();

            var ids = new HashSet<int>(receiptOfTheStatusOfTheDeliveredShipmentWithCostDtos.Select(c => c.Id));

            var orders = await _uintOfWork.Repository<Order>().GetAsync(c => ids.Contains(c.Id));
            var repatedOrders = orders.Where(order => receiptOfTheStatusOfTheDeliveredShipmentWithCostDtos.Any(r => r.EqualToOrder(order))).ToList();
            orders = orders.Except(repatedOrders).ToList();
            var exptedOrdersIds = repatedOrders.Select(c => c.Id);
            receiptOfTheStatusOfTheDeliveredShipmentWithCostDtos = receiptOfTheStatusOfTheDeliveredShipmentWithCostDtos.Where(c => !exptedOrdersIds.Contains(c.Id));
            if (!receiptOfTheStatusOfTheDeliveredShipmentWithCostDtos.Any())
            {
                return new ErrorResponse<string, IEnumerable<string>>();
            }
            List<OrderLog> logs = new List<OrderLog>();
            foreach (var item in receiptOfTheStatusOfTheDeliveredShipmentWithCostDtos)
            {
                var order = orders.First(c => c.Id == item.Id);
                logs.Add(order);
                order.MoenyPlacedId = (int)item.MoenyPlacedId;
                order.OrderplacedId = (int)item.OrderplacedId;
                order.Note = item.Note;
                if (order.DeliveryCost != item.DeliveryCost)
                {
                    if (order.OldDeliveryCost == null)
                    {
                        order.OldDeliveryCost = order.DeliveryCost;
                    }
                }
                order.DeliveryCost = item.DeliveryCost;
                order.AgentCost = item.AgentCost;
                order.SystemNote = "ReceiptOfTheStatusOfTheDeliveredShipment";
                if (order.IsClientDiliverdMoney)
                {
                    switch (order.OrderplacedId)
                    {
                        case (int)OrderplacedEnum.Delivered:
                            {
                                if (decimal.Compare(order.Cost, item.Cost) != 0)
                                {
                                    if (order.OldCost == null)
                                        order.OldCost = order.Cost;
                                    order.Cost = item.Cost;
                                }
                                var payForClient = order.ShouldToPay();

                                if (decimal.Compare(payForClient, (order.ClientPaied ?? 0)) != 0)
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
                    if (order.Cost != item.Cost)
                    {
                        if (order.OldCost == null)
                            order.OldCost = order.Cost;
                        order.Cost = item.Cost;
                    }
                }
                if (order.MoenyPlacedId == (int)MoneyPalcedEnum.InsideCompany)
                {
                    order.ApproveAgentEditOrderRequests.Clear();
                }
                order.MoenyPlaced = moneyPlacedes.First(c => c.Id == order.MoenyPlacedId);
                order.Orderplaced = orderPlacedes.First(c => c.Id == order.OrderplacedId);
            }

            await _uintOfWork.BegeinTransaction();
            try
            {
                await _uintOfWork.UpdateRange(orders);
                await _uintOfWork.AddRange(logs);
                await _notificationService.SendOrderReciveNotifcation(orders);

                var receiptOfTheOrderStatus = new ReceiptOfTheOrderStatus
                {
                    CreatedOn = DateTime.UtcNow,
                    RecvierId = _userService.AuthoticateUserId()
                };
                var receiptOfTheOrderStatusDetalis = new List<ReceiptOfTheOrderStatusDetali>();
                foreach (var order in orders)
                {
                    receiptOfTheOrderStatusDetalis.Add(new ReceiptOfTheOrderStatusDetali()
                    {
                        OrderCode = order.Code,
                        ClientId = order.ClientId,
                        Cost = order.Cost,
                        AgentCost = order.AgentCost,
                        AgentId = (int)order.AgentId,
                        MoneyPlacedId = order.MoenyPlacedId,
                        OrderPlacedId = order.OrderplacedId,
                        OrderStateId = order.OrderStateId,
                    });
                }
                receiptOfTheOrderStatus.ReceiptOfTheOrderStatusDetalis = receiptOfTheOrderStatusDetalis;
                await _uintOfWork.Add(receiptOfTheOrderStatus);

                await _treasuryService.IncreaseAmountByOrderFromAgent(receiptOfTheOrderStatus);
                await _uintOfWork.Commit();
                return response;
            }
            catch (Exception ex)
            {
                await _uintOfWork.RoleBack();
                return new ErrorResponse<string, IEnumerable<string>>("حدث خطأ ما ");
            }

        }
        public async Task<ErrorResponse<string, IEnumerable<string>>> ReceiptOfTheStatusOfTheReturnedShipment(IEnumerable<ReceiptOfTheStatusOfTheDeliveredShipmentDto> receiptOfTheStatusOfTheDeliveredShipmentDtos)
        {
            var moneyPlacedes = await _uintOfWork.Repository<MoenyPlaced>().GetAll();
            var orderPlacedes = await _uintOfWork.Repository<OrderPlaced>().GetAll();
            var outSideCompny = moneyPlacedes.First(c => c.Id == (int)MoneyPalcedEnum.OutSideCompany).Name;
            var response = new ErrorResponse<string, IEnumerable<string>>();

            var orders = (await _uintOfWork.Repository<Order>().GetAsync(c => new HashSet<int>(receiptOfTheStatusOfTheDeliveredShipmentDtos.Select(c => c.Id)).Contains(c.Id))).ToList();
            var repatedOrders = orders.Where(order => receiptOfTheStatusOfTheDeliveredShipmentDtos.Any(r => r.EqualToOrder(order))).ToList();
            orders = orders.Except(repatedOrders).ToList();
            var exptedOrdersIds = repatedOrders.Select(c => c.Id);
            receiptOfTheStatusOfTheDeliveredShipmentDtos = receiptOfTheStatusOfTheDeliveredShipmentDtos.Where(c => !exptedOrdersIds.Contains(c.Id));
            if (!receiptOfTheStatusOfTheDeliveredShipmentDtos.Any())
            {
                return new ErrorResponse<string, IEnumerable<string>>();
            }
            if (!receiptOfTheStatusOfTheDeliveredShipmentDtos.All(c => c.OrderplacedId == OrderplacedEnum.Way || c.OrderplacedId == OrderplacedEnum.CompletelyReturned || c.OrderplacedId == OrderplacedEnum.Unacceptable || c.OrderplacedId == OrderplacedEnum.Unacceptable))
            {
                var wrongData = receiptOfTheStatusOfTheDeliveredShipmentDtos.Where(c => !(c.OrderplacedId == OrderplacedEnum.Way || c.OrderplacedId == OrderplacedEnum.CompletelyReturned || c.OrderplacedId == OrderplacedEnum.Unacceptable || c.OrderplacedId == OrderplacedEnum.Unacceptable));
                var worngDataIds = wrongData.Select(c => c.Id);
                var worngOrders = await _uintOfWork.Repository<Order>().GetAsync(c => worngDataIds.Contains(c.Id));
                List<string> errors = new List<string>();
                foreach (var item in receiptOfTheStatusOfTheDeliveredShipmentDtos)
                {
                    string code = worngOrders.Where(c => c.Id == item.Id).FirstOrDefault()?.Code;
                    errors.Add($"لا يمكن وضع حالة الشحنة {OrderPlacedEnumToString(item.OrderplacedId)} للشحنة ذات الرقم : {code}");
                }
                response.Errors = errors;
                return response;
            }
            List<Notfication> notfications = new List<Notfication>();
            List<Notfication> addednotfications = new List<Notfication>();

            List<OrderLog> logs = new List<OrderLog>();
            foreach (var item in receiptOfTheStatusOfTheDeliveredShipmentDtos)
            {
                var order = orders.First(c => c.Id == item.Id);
                logs.Add(order);
                order.MoenyPlacedId = (int)item.MoenyPlacedId;
                order.OrderplacedId = (int)item.OrderplacedId;
                order.Note = item.Note;

                if (order.DeliveryCost != item.DeliveryCost)
                {
                    if (order.OldDeliveryCost == null)
                    {
                        order.OldDeliveryCost = order.DeliveryCost;
                    }
                }
                order.DeliveryCost = item.DeliveryCost;
                order.AgentCost = item.AgentCost;
                order.SystemNote = "ReceiptOfTheStatusOfTheReturnedShipment";
                if (order.IsClientDiliverdMoney)
                {
                    switch (order.OrderplacedId)
                    {
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

                    }
                }
                else
                {
                    switch (order.OrderplacedId)
                    {
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
                order.MoenyPlaced = moneyPlacedes.First(c => c.Id == order.MoenyPlacedId);
                order.Orderplaced = orderPlacedes.First(c => c.Id == order.OrderplacedId);
            }
            await _uintOfWork.BegeinTransaction();
            try
            {
                await _uintOfWork.UpdateRange(orders);
                await _notificationService.SendOrderReciveNotifcation(orders);
                var receiptOfTheOrderStatus = new ReceiptOfTheOrderStatus
                {
                    CreatedOn = DateTime.UtcNow,
                    RecvierId = _userService.AuthoticateUserId()
                };
                var receiptOfTheOrderStatusDetalis = new List<ReceiptOfTheOrderStatusDetali>();
                foreach (var order in orders)
                {
                    receiptOfTheOrderStatusDetalis.Add(new ReceiptOfTheOrderStatusDetali()
                    {
                        OrderCode = order.Code,
                        ClientId = order.ClientId,
                        Cost = order.Cost,
                        AgentCost = order.AgentCost,
                        AgentId = (int)order.AgentId,
                        MoneyPlacedId = order.MoenyPlacedId,
                        OrderPlacedId = order.OrderplacedId,
                        OrderStateId = order.OrderStateId,
                    });
                }
                receiptOfTheOrderStatus.ReceiptOfTheOrderStatusDetalis = receiptOfTheOrderStatusDetalis;
                await _uintOfWork.Add(receiptOfTheOrderStatus);
                await _uintOfWork.Commit();
            }
            catch (Exception ex)
            {
                await _uintOfWork.RoleBack();
                return new ErrorResponse<string, IEnumerable<string>>("حدث خطأ ما ");
            }
            return new ErrorResponse<string, IEnumerable<string>>();

        }
        public async Task<GenaricErrorResponse<ReceiptOfTheOrderStatusDto, string, IEnumerable<string>>> GetReceiptOfTheOrderStatusById(int id)
        {
            var response = (await _receiptOfTheOrderStatusRepository.GetByFilterInclue(c => c.Id == id, new string[] { "Recvier", "ReceiptOfTheOrderStatusDetalis.Agent", "ReceiptOfTheOrderStatusDetalis.MoneyPlaced", "ReceiptOfTheOrderStatusDetalis.OrderPlaced" })).FirstOrDefault();
            var dto = _mapper.Map<ReceiptOfTheOrderStatusDto>(response);
            return new GenaricErrorResponse<ReceiptOfTheOrderStatusDto, string, IEnumerable<string>>(dto);
        }

        string OrderPlacedEnumToString(OrderplacedEnum orderplacedEnum)
        {
            return orderplacedEnum switch
            {
                OrderplacedEnum.Client => "عميل",
                OrderplacedEnum.Store => "مخزن",
                OrderplacedEnum.Way => "طريق",
                OrderplacedEnum.Delivered => "تم التسليم",
                OrderplacedEnum.CompletelyReturned => "مرتجع كلي",
                OrderplacedEnum.PartialReturned => "مرتجع جزئي",
                OrderplacedEnum.Unacceptable => "مرفوض",
                OrderplacedEnum.Delayed => "مؤجل",
                _ => "غير معلوم",
            };
        }

        public async Task<PagingResualt<IEnumerable<ReceiptOfTheOrderStatusDto>>> GetReceiptOfTheOrderStatus(PagingDto Paging)
        {
            var response = await _receiptOfTheOrderStatusRepository.GetAsync(Paging, new string[] { "Recvier" });
            var dtos = _mapper.Map<ReceiptOfTheOrderStatusDto[]>(response.Data);
            return new PagingResualt<IEnumerable<ReceiptOfTheOrderStatusDto>>()
            {
                Total = response.Total,
                Data = dtos
            };
        }
    }
}
