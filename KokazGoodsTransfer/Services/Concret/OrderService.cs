using AutoMapper;
using KokazGoodsTransfer.CustomException;
using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Static;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Concret
{
    public partial class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IUintOfWork _uintOfWork;
        private readonly INotificationService _notificationService;
        private readonly ITreasuryService _treasuryService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IRepository<ReceiptOfTheOrderStatus> _receiptOfTheOrderStatusRepository;
        private readonly IRepository<ReceiptOfTheOrderStatusDetali> _receiptOfTheOrderStatusDetalisRepository;
        private readonly ICountryCashedService _countryCashedService;
        private readonly Logging _logging;
        private static readonly Func<Order, bool> _finishOrderExpression = c => c.OrderplacedId == (int)OrderplacedEnum.CompletelyReturned || c.OrderplacedId == (int)OrderplacedEnum.Unacceptable
|| (c.OrderplacedId == (int)OrderplacedEnum.Delivered && (c.MoenyPlacedId == (int)MoneyPalcedEnum.InsideCompany || c.MoenyPlacedId == (int)MoneyPalcedEnum.Delivered));
        private readonly string currentUser;
        public OrderService(IUintOfWork uintOfWork, IOrderRepository repository, INotificationService notificationService, ITreasuryService treasuryService, IMapper mapper, IUserService userService, IRepository<ReceiptOfTheOrderStatus> receiptOfTheOrderStatusRepository, Logging logging, IRepository<ReceiptOfTheOrderStatusDetali> receiptOfTheOrderStatusDetalisRepository, ICountryCashedService countryCashedService, IHttpContextAccessor httpContextAccessor)
        {
            _uintOfWork = uintOfWork;
            _notificationService = notificationService;
            _treasuryService = treasuryService;
            _mapper = mapper;
            _userService = userService;
            _receiptOfTheOrderStatusRepository = receiptOfTheOrderStatusRepository;
            _repository = repository;
            _logging = logging;
            _receiptOfTheOrderStatusDetalisRepository = receiptOfTheOrderStatusDetalisRepository;
            _countryCashedService = countryCashedService;
            currentUser = httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
        }

        public async Task<GenaricErrorResponse<IEnumerable<OrderDto>, string, IEnumerable<string>>> GetOrderToReciveFromAgent(string code)
        {
            var orders = await _uintOfWork.Repository<Order>().GetAsync(c => c.Code == code, c => c.Client, c => c.Agent, c => c.MoenyPlaced, c => c.Orderplaced, c => c.Country);

            if (!orders.Any())
            {
                return new GenaricErrorResponse<IEnumerable<OrderDto>, string, IEnumerable<string>>("الشحنة غير موجودة");
            }
            var lastOrderAdded = orders.OrderBy(c => c.Id).Last();
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
                        OrderId = order.Id
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
                _logging.WriteExption(ex);
                await _uintOfWork.Rollback();
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
                        OrderId = order.Id
                    });
                }
                receiptOfTheOrderStatus.ReceiptOfTheOrderStatusDetalis = receiptOfTheOrderStatusDetalis;
                await _uintOfWork.Add(receiptOfTheOrderStatus);
                await _uintOfWork.Commit();
            }
            catch (Exception ex)
            {
                _logging.WriteExption(ex);
                await _uintOfWork.Rollback();
                return new ErrorResponse<string, IEnumerable<string>>("حدث خطأ ما ");
            }
            return new ErrorResponse<string, IEnumerable<string>>();

        }
        public async Task<GenaricErrorResponse<ReceiptOfTheOrderStatusDto, string, IEnumerable<string>>> GetReceiptOfTheOrderStatusById(int id)
        {
            var response = (await _receiptOfTheOrderStatusRepository.GetByFilterInclue(c => c.Id == id, new string[] { "Recvier", "ReceiptOfTheOrderStatusDetalis.Agent", "ReceiptOfTheOrderStatusDetalis.MoneyPlaced", "ReceiptOfTheOrderStatusDetalis.OrderPlaced", "ReceiptOfTheOrderStatusDetalis.Client" })).FirstOrDefault();
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
        public async Task<GenaricErrorResponse<int, string, string>> MakeOrderInWay(int[] ids)
        {
            var orders = await _uintOfWork.Repository<Order>().GetByFilterInclue(c => ids.Contains(c.Id), new string[] { "Agent.UserPhones", "Client", "Country", "Region" });
            if (orders.Any(c => c.OrderplacedId != (int)OrderplacedEnum.Store))
            {
                var errors = orders.Where(c => c.OrderplacedId != (int)OrderplacedEnum.Store).Select(c => $"الشحنة رقم{c.Code} ليست في المخزن");
                return new GenaricErrorResponse<int, string, string>(errors, true);
            }
            var agent = orders.FirstOrDefault().Agent;
            var agnetPrint = new AgentPrint()
            {
                Date = DateTime.UtcNow,
                PrinterName = _userService.AuthoticateUserName(),
                DestinationName = agent.Name,
                DestinationPhone = agent.UserPhones.FirstOrDefault()?.Phone ?? ""
            };
            await _uintOfWork.BegeinTransaction();
            var agnetOrderPrints = new List<AgentOrderPrint>();
            var agentPrintsDetials = new List<AgentPrintDetail>();
            try
            {
                await _uintOfWork.Repository<AgentPrint>().AddAsync(agnetPrint);
                foreach (var item in orders)
                {


                    item.OrderplacedId = (int)OrderplacedEnum.Way;

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
                        Region = item.Region?.Name,
                        Date = item.Date,
                        ClientNote = item.ClientNote,
                        Address = item.Address
                    };
                    agnetOrderPrints.Add(agnetOrderPrint);
                    agentPrintsDetials.Add(agentPrintDetials);
                }
                await _uintOfWork.UpdateRange(orders);
                await _uintOfWork.AddRange(agnetOrderPrints);
                await _uintOfWork.AddRange(agentPrintsDetials);
                await _uintOfWork.Commit();
                return new GenaricErrorResponse<int, string, string>(agnetPrint.Id);
            }
            catch (Exception ex)
            {
                await _uintOfWork.Rollback();
                _logging.WriteExption(ex);
                return new GenaricErrorResponse<int, string, string>("حدث خطأ ما ", false, true);
            }
        }
        public async Task<PagingResualt<IEnumerable<ReceiptOfTheOrderStatusDto>>> GetReceiptOfTheOrderStatus(PagingDto Paging, string code)
        {
            PagingResualt<IEnumerable<ReceiptOfTheOrderStatus>> response;
            if (string.IsNullOrEmpty(code))
            {
                response = await _receiptOfTheOrderStatusRepository.GetAsync(Paging, new string[] { "Recvier" }, orderBy: c => c.OrderByDescending(r => r.Id));
            }
            else
            {
                response = await _receiptOfTheOrderStatusRepository.GetAsync(Paging, c => c.ReceiptOfTheOrderStatusDetalis.Any(c => c.OrderCode == code), new string[] { "Recvier" }, orderBy: c => c.OrderByDescending(r => r.Id));

            }

            var ids = response.Data.Select(c => c.Id);

            var types = (await _receiptOfTheOrderStatusDetalisRepository.Select(c => ids.Contains(c.ReceiptOfTheOrderStatusId), c => new { c.Id, c.ReceiptOfTheOrderStatusId, c.OrderPlaced }, c => c.OrderPlaced)).GroupBy(c => c.ReceiptOfTheOrderStatusId).ToDictionary(c => c.Key, c => c.ToList());

            var dtos = _mapper.Map<List<ReceiptOfTheOrderStatusDto>>(response.Data);
            dtos.ForEach(c =>
            {
                if (types.ContainsKey(c.Id))
                {
                    var orderPlaced = types.Where(t => t.Key == c.Id).SelectMany(c => c.Value.Select(c => c.OrderPlaced.Name)).ToHashSet();
                    c.Types = String.Join(',', orderPlaced);
                }
            });
            return new PagingResualt<IEnumerable<ReceiptOfTheOrderStatusDto>>()
            {
                Total = response.Total,
                Data = dtos
            };
        }

        public async Task<PagingResualt<IEnumerable<OrderDto>>> GetOrderFiltered(PagingDto pagingDto, OrderFilter orderFilter)
        {
            var pagingResult = await _repository.Get(pagingDto, orderFilter, new string[] { "Client", "Agent", "Region", "Country", "Orderplaced", "MoenyPlaced", "OrderClientPaymnets.ClientPayment", "AgentOrderPrints.AgentPrint" });
            return new PagingResualt<IEnumerable<OrderDto>>()
            {
                Data = _mapper.Map<OrderDto[]>(pagingResult.Data),
                Total = pagingResult.Total
            };
        }
        public async Task CreateOrders(IEnumerable<CreateMultipleOrder> createMultipleOrders)
        {
            {
                var clientsIds = createMultipleOrders.Select(c => c.ClientId);
                var codes = createMultipleOrders.Select(c => c.Code);

                var exsitinOrders = await _uintOfWork.Repository<Order>().GetAsync(c => codes.Contains(c.Code) && clientsIds.Contains(c.ClientId));
                if (exsitinOrders.Any())
                {
                    exsitinOrders = exsitinOrders.Where(eo => createMultipleOrders.Any(co => co.ClientId == eo.ClientId && eo.Code == co.Code));
                    if (exsitinOrders.Any())
                    {
                        throw new ConfilectException(exsitinOrders.Select(c => $"الكود{c.Code} مكرر"));
                    }
                }

            }
            var mainCountryId = (await _countryCashedService.GetAsync(c => c.IsMain == true)).First().Id;
            var agnetsIds = createMultipleOrders.Select(c => c.AgentId);
            var agnets = await _uintOfWork.Repository<User>().Select(c => agnetsIds.Contains(c.Id), c => new { c.Id, c.Salary });
            await _uintOfWork.BegeinTransaction();
            try
            {
                var orders = createMultipleOrders.Select(item =>
                 {
                     var order = _mapper.Map<Order>(item);
                     order.AgentCost = agnets.FirstOrDefault(c => c.Id == order.AgentId)?.Salary ?? 0;
                     order.Date = item.Date;
                     order.CurrentCountry = mainCountryId;
                     order.CreatedBy = currentUser;
                     return order;
                 });
                await _uintOfWork.AddRange(orders);
                await _uintOfWork.Commit();
            }
            catch (Exception ex)
            {

                await _uintOfWork.Rollback();
                _logging.WriteExption(ex);
                throw ex;
            }
        }

        public async Task CreateOrder(CreateOrdersFromEmployee createOrdersFromEmployee)
        {
            var country = await _countryCashedService.GetById(createOrdersFromEmployee.CountryId);
            await _uintOfWork.BegeinTransaction();
            try
            {
                var order = _mapper.Map<CreateOrdersFromEmployee, Order>(createOrdersFromEmployee);
                order.CurrentCountry = country.Id;
                order.CreatedBy = currentUser;
                if (await _uintOfWork.Repository<Order>().Any(c => c.Code == order.Code && c.ClientId == order.ClientId))
                {
                    throw new ConfilectException($"الكود{order.Code} مكرر");
                }
                if (createOrdersFromEmployee.RegionId == null)
                {
                    if (!String.IsNullOrWhiteSpace(createOrdersFromEmployee.RegionName))
                    {
                        var region = new Region()
                        {
                            Name = createOrdersFromEmployee.RegionName,
                            CountryId = createOrdersFromEmployee.CountryId
                        };
                        await _uintOfWork.Add(region);
                        order.RegionId = region.Id;
                        order.Seen = true;
                    }
                    order.AgentCost = (await _uintOfWork.Repository<User>().FirstOrDefualt(c => c.Id == order.AgentId))?.Salary ?? 0;
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
                await _uintOfWork.Add(order);

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
                            await _uintOfWork.Add(orderType);
                            orderId = orderType.Id;
                        }
                        OrderItem orderItem = new OrderItem()
                        {
                            OrderId = order.Id,
                            Count = item.Count,
                            OrderTpyeId = orderId
                        };
                        await _uintOfWork.Add(orderItem);
                    }
                }

                await _uintOfWork.Commit();
            }
            catch (Exception ex)
            {
                await _uintOfWork.Rollback();
                _logging.WriteExption(ex);
                throw ex;
            }
        }

        public async Task<bool> Any(Expression<Func<Order, bool>> expression)
        {
            return await _repository.Any(expression);
        }

        public async Task<IEnumerable<CodeStatus>> GetCodeStatuses(int clientId, string[] codes)
        {
            List<CodeStatus> codeStatuses = new List<CodeStatus>();
            var nonAvilableCode = await _repository.Select(c => c.ClientId == clientId && codes.Contains(c.Code), c => c.Code);
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
            return codeStatuses;
        }

        public async Task<IEnumerable<OrderDto>> GetAll(Expression<Func<Order, bool>> expression, string[] propertySelector = null)
        {
            var orders = await _repository.GetByFilterInclue(expression, propertySelector);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }
        public Task<GenaricErrorResponse<int, string, string>> DeleiverMoneyForClient(DeleiverMoneyForClientDto deleiverMoneyForClientDto)
        {

            throw new NotImplementedException();
        }
        private async Task<PagingResualt<IEnumerable<ReceiptOfTheOrderStatusDto>>> GetReceiptOfTheOrderStatus(PagingDto Paging)
        {
            var response = await _receiptOfTheOrderStatusRepository.GetAsync(Paging, new string[] { "Recvier" }, orderBy: c => c.OrderByDescending(r => r.Id));
            var ids = response.Data.Select(c => c.Id);

            var groups = (await _receiptOfTheOrderStatusDetalisRepository.Select(c => ids.Contains(c.ReceiptOfTheOrderStatusId), c => new { c.Id, c.ReceiptOfTheOrderStatusId, c.OrderPlaced }, c => c.OrderPlaced)).GroupBy(c => c.ReceiptOfTheOrderStatusId);
            var types = groups.ToDictionary(c => c.Key, c => c.ToList());
            var dtos = _mapper.Map<List<ReceiptOfTheOrderStatusDto>>(response.Data);
            dtos.ForEach(c =>
            {
                if (types.ContainsKey(c.Id))
                {
                    var orderPlaced = types.Where(t => t.Key == c.Id).SelectMany(c => c.Value.Select(c => c.OrderPlaced.Name));
                    c.Types = String.Join(',', orderPlaced);
                }
            });
            return new PagingResualt<IEnumerable<ReceiptOfTheOrderStatusDto>>()
            {
                Total = response.Total,
                Data = dtos
            };
        }

        public async Task Delete(int id)
        {
            var order = await _repository.FirstOrDefualt(c => c.Id == id);
            await _repository.Delete(order);
        }
    }


}
