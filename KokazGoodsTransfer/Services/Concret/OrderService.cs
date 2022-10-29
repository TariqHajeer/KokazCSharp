﻿using AutoMapper;
using KokazGoodsTransfer.CustomException;
using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.NotifcationDtos;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Helpers.Extensions;
using KokazGoodsTransfer.HubsConfig;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Static;
using KokazGoodsTransfer.Services.Interfaces;
using LinqKit;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading;
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
        private readonly IRepository<Country> _countryRepository;
        private readonly IRepository<AgentCountry> _agentCountryRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<ClientPayment> _clientPaymentRepository;
        private readonly IRepository<DisAcceptOrder> _DisAcceptOrderRepository;
        private readonly NotificationHub _notificationHub;
        private readonly IRepository<Branch> _branchRepository;
        private readonly Logging _logging;
        private readonly IHttpContextAccessorService _httpContextAccessorService;
        private readonly int _currentBranchId;

        static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private static readonly Func<Order, bool> _finishOrderExpression = c => c.OrderplacedId == (int)OrderplacedEnum.CompletelyReturned || c.OrderplacedId == (int)OrderplacedEnum.Unacceptable
|| (c.OrderplacedId == (int)OrderplacedEnum.Delivered && (c.MoenyPlacedId == (int)MoneyPalcedEnum.InsideCompany || c.MoenyPlacedId == (int)MoneyPalcedEnum.Delivered));
        private readonly string currentUser;
        private readonly int currentUserId;
        public OrderService(IUintOfWork uintOfWork, IOrderRepository repository, INotificationService notificationService,
            ITreasuryService treasuryService, IMapper mapper, IUserService userService,
            IRepository<ReceiptOfTheOrderStatus> receiptOfTheOrderStatusRepository, Logging logging,
            IRepository<ReceiptOfTheOrderStatusDetali> receiptOfTheOrderStatusDetalisRepository,
            ICountryCashedService countryCashedService, IHttpContextAccessor httpContextAccessor,
            IRepository<Country> countryRepository, IRepository<AgentCountry> agentCountryRepository,
            IRepository<User> userRepository, IRepository<ClientPayment> clientPaymentRepository, IRepository<DisAcceptOrder> disAcceptOrderRepository, NotificationHub notificationHub, IRepository<Branch> branchRepository, IHttpContextAccessorService httpContextAccessorService)
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
            _countryRepository = countryRepository;
            _agentCountryRepository = agentCountryRepository;
            _userRepository = userRepository;
            currentUserId = Convert.ToInt32(httpContextAccessor.HttpContext.User.Claims.Where(c => c.Type == "UserID").Single().Value);
            _clientPaymentRepository = clientPaymentRepository;
            _DisAcceptOrderRepository = disAcceptOrderRepository;
            _notificationHub = notificationHub;
            _branchRepository = branchRepository;
            _httpContextAccessorService = httpContextAccessorService;
            _currentBranchId = _httpContextAccessorService.CurrentBranchId();
        }

        public async Task<GenaricErrorResponse<IEnumerable<OrderDto>, string, IEnumerable<string>>> GetOrderToReciveFromAgent(string code)
        {
            var orders = await _uintOfWork.Repository<Order>().GetAsync(c => c.CurrentBranchId == _currentBranchId && c.Code == code, c => c.Client, c => c.Agent, c => c.MoenyPlaced, c => c.Orderplaced, c => c.Country);

            if (!orders.Any())
            {
                throw new ConflictException("الشحنة غير موجودة");
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
                    throw new ConflictException("الشحنة في المخزن");
                if (lastOrderAdded.OrderplacedId == (int)OrderplacedEnum.Client)
                    throw new ConflictException("الشحنة عند العميل");
                if (lastOrderAdded.OrderplacedId == (int)MoneyPalcedEnum.InsideCompany)
                    throw new ConflictException("الشحنة داخل الشركة");
            }
            return new GenaricErrorResponse<IEnumerable<OrderDto>, string, IEnumerable<string>>(_mapper.Map<OrderDto[]>(orders));
        }
        public async Task<ErrorResponse<string, IEnumerable<string>>> ReceiptOfTheStatusOfTheDeliveredShipment(IEnumerable<ReceiptOfTheStatusOfTheDeliveredShipmentWithCostDto> receiptOfTheStatusOfTheDeliveredShipmentWithCostDtos)
        {
            var moneyPlacedes = Consts.MoneyPlaceds;
            var orderPlacedes = Consts.OrderPlaceds;
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
                    errors.Add($"لا يمكن وضع حالة الشحنة {item.OrderplacedId.GetDescription()} للشحنة ذات الرقم : {code}");
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
                    order.AgentRequestStatus = (int)AgentRequestStatusEnum.None;
                }
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
            var moneyPlacedes = Consts.MoneyPlaceds;
            var orderPlacedes = Consts.OrderPlaceds;
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
                    errors.Add($"لا يمكن وضع حالة الشحنة {item.OrderplacedId.GetDescription()} للشحنة ذات الرقم : {code}");
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
                    order.AgentRequestStatus = (int)AgentRequestStatusEnum.None;
                }
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
        public async Task<int> MakeOrderInWay(int[] ids)
        {
            var orders = await _uintOfWork.Repository<Order>().GetByFilterInclue(c => ids.Contains(c.Id), new string[] { "Agent.UserPhones", "Client", "Country", "Region" });
            if (orders.Any(c => c.OrderplacedId != (int)OrderplacedEnum.Store))
            {
                var errors = orders.Where(c => c.OrderplacedId != (int)OrderplacedEnum.Store).Select(c => $"الشحنة رقم{c.Code} ليست في المخزن");
                throw new ConflictException(errors);
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
            return agnetPrint.Id;
        }
        public async Task<PagingResualt<IEnumerable<OrderDto>>> GetOrdersComeToMyBranch(PagingDto pagingDto, OrderFilter orderFilter)
        {
            var predicate = GetFilterAsLinq(orderFilter);
            var pagingResult = await _repository.GetAsync(pagingDto, predicate);
            predicate = predicate.And(c => c.SecondBranchId == _currentBranchId && c.CurrentBranchId != _currentBranchId && c.OrderplacedId == (int)OrderplacedEnum.Way);
            pagingResult = await _repository.GetAsync(pagingDto, predicate, c => c.Country, c => c.Client);
            return new PagingResualt<IEnumerable<OrderDto>>()
            {
                Total = pagingResult.Total,
                Data = _mapper.Map<IEnumerable<OrderDto>>(pagingResult.Data)
            };
        }
        public async Task TransferToSecondBranch(int[] ids)
        {
            var orders = await _repository.GetAsync(c => ids.Contains(c.Id));
            if (orders.Any(c => c.CurrentBranchId != _currentBranchId || c.OrderplacedId != (int)OrderplacedEnum.Store || c.SecondBranchId == null))
            {
                throw new ConflictException("هناك شحنات لا يمكن إرسالها ");
            }
            orders.ForEach(c =>
            {
                c.OrderplacedId = (int)OrderplacedEnum.Way;
                c.InWayToBranch = true;
            });
            await _repository.Update(orders);
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

        ExpressionStarter<Order> GetFilterAsLinq(OrderFilter filter)
        {
            var preidcate = PredicateBuilder.New<Order>(true);
            if (filter.CountryId != null)
                preidcate = preidcate.And(c => c.CountryId == filter.CountryId);
            if (!string.IsNullOrEmpty(filter.Code))
                preidcate = preidcate.And(c => c.Code == filter.Code);
            if (filter.ClientId != null)
            {
                preidcate = preidcate.And(c => c.ClientId == filter.ClientId);
            }
            if (filter.RegionId != null)
            {
                preidcate = preidcate.And(c => c.RegionId == filter.RegionId);
            }
            if (filter.RecipientName != string.Empty && filter.RecipientName != null)
            {
                preidcate = preidcate.And(c => c.RecipientName.StartsWith(filter.RecipientName));
            }
            if (filter.MonePlacedId != null)
            {
                preidcate = preidcate.And(c => c.MoenyPlacedId == filter.MonePlacedId);
            }
            if (filter.OrderplacedId != null)
            {
                preidcate = preidcate.And(c => c.OrderplacedId == filter.OrderplacedId);
            }
            if (filter.Phone != string.Empty && filter.Phone != null)
            {
                preidcate = preidcate.And(c => c.RecipientPhones.Contains(filter.Phone));
            }
            if (filter.AgentId != null)
            {
                preidcate = preidcate.And(c => c.AgentId == filter.AgentId);
            }
            if (filter.IsClientDiliverdMoney != null)
            {
                preidcate = preidcate.And(c => c.IsClientDiliverdMoney == filter.IsClientDiliverdMoney);
            }
            if (filter.ClientPrintNumber != null)
            {
                preidcate = preidcate.And(c => c.OrderClientPaymnets.Any(op => op.ClientPayment.Id == filter.ClientPrintNumber));
            }
            if (filter.AgentPrintNumber != null)
            {
                preidcate = preidcate.And(c => c.AgentOrderPrints.Any(c => c.AgentPrint.Id == filter.AgentPrintNumber));
            }
            if (filter.CreatedDate != null)
            {
                preidcate = preidcate.And(c => c.Date == filter.CreatedDate);
            }
            if (filter.Note != "" && filter.Note != null)
            {
                preidcate = preidcate.And(c => c.Note.Contains(filter.Note));
            }
            if (filter.AgentPrintStartDate != null)
            {
                ///TODO :
                ///chould check this query 
                preidcate = preidcate.And(c => c.AgentOrderPrints.Select(c => c.AgentPrint).OrderBy(c => c.Id).LastOrDefault().Date >= filter.AgentPrintStartDate);

            }
            if (filter.AgentPrintEndDate != null)
            {
                ///TODO :
                ///chould check this query 
                preidcate = preidcate.And(c => c.AgentOrderPrints.Select(c => c.AgentPrint).OrderBy(c => c.Id).LastOrDefault().Date <= filter.AgentPrintEndDate);
            }
            if (!string.IsNullOrEmpty(filter.CreatedBy?.Trim()))
            {
                preidcate = preidcate.And(c => c.CreatedBy == filter.CreatedBy);
            }
            if (filter.CreatedDateRangeFilter != null)
            {
                if (filter.CreatedDateRangeFilter.Start != null)
                    preidcate = preidcate.And(c => c.Date >= filter.CreatedDateRangeFilter.Start);
                if (filter.CreatedDateRangeFilter.End != null)
                    preidcate = preidcate.And(c => c.Date <= filter.CreatedDateRangeFilter.End);
            }
            if (filter.HaveScoundBranch != null)
            {
                if (filter.HaveScoundBranch == true)
                    preidcate = preidcate.And(c => c.SecondBranchId != null);
                else
                    preidcate = preidcate.And(c => c.SecondBranchId == null);
            }
            if (filter.OrderState != null)
            {
                preidcate = preidcate.And(c => c.OrderStateId == (int)filter.OrderState);
            }
            return preidcate;
        }
        public async Task<PagingResualt<IEnumerable<OrderDto>>> GetOrderFiltered(PagingDto pagingDto, OrderFilter orderFilter)
        {
            var includes = new string[] { "Client", "Agent", "Region", "Country", "OrderClientPaymnets.ClientPayment", "AgentOrderPrints.AgentPrint" };
            var pagingResult = await _repository.GetAsync(pagingDto, GetFilterAsLinq(orderFilter), includes, null);
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
                        throw new ConflictException(exsitinOrders.Select(c => $"الكود{c.Code} مكرر"));
                    }
                }

            }
            var mainCountryId = (await _countryCashedService.GetAsync(c => c.IsMain == true)).First().Id;
            var agnetsIds = createMultipleOrders.Select(c => c.AgentId);
            var agnets = await _uintOfWork.Repository<User>().Select(c => agnetsIds.Contains(c.Id), c => new { c.Id, c.Salary });
            await _uintOfWork.BegeinTransaction();
            try
            {
                var countryIds = createMultipleOrders.Select(c => c.CountryId);
                var branches = await _branchRepository.GetAsync(c => countryIds.Contains(c.CountryId) && c.Id != _currentBranchId);
                var orders = createMultipleOrders.Select(item =>
                 {
                     var order = _mapper.Map<Order>(item);
                     order.AgentCost = agnets.FirstOrDefault(c => c.Id == order.AgentId)?.Salary ?? 0;
                     order.Date = item.Date;
                     order.CurrentCountry = mainCountryId;
                     order.CreatedBy = currentUser;
                     order.CurrentBranchId = _currentBranchId;
                     var secoundBranch = branches.FirstOrDefault(c => c.CountryId == item.CountryId);
                     if (secoundBranch != null)
                     {
                         order.SecondBranchId = secoundBranch.Id;
                         if (order.AgentId != null)
                             throw new ConflictException("لا يمكن اختيار مندوب إذا كان الطلب موجه إلى فرع آخر");
                     }
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
                order.CurrentBranchId = _currentBranchId;
                var nextBranch = await _branchRepository.FirstOrDefualt(c => c.CountryId == country.Id && c.Id != _currentBranchId);
                if (nextBranch != null)
                {
                    order.SecondBranchId = nextBranch.Id;
                    if (order.AgentId != null)
                    {
                        throw new ConflictException("لا يمكن اختيار مندوب إذا كان الطلب موجه إلى فرع آخر");
                    }
                }
                else if (order.AgentId == null)
                {
                    throw new ConflictException("يجب اختيار المندوب");
                }

                order.CreatedBy = currentUser;
                if (await _uintOfWork.Repository<Order>().Any(c => c.Code == order.Code && c.ClientId == order.ClientId))
                {
                    throw new ConflictException($"الكود{order.Code} مكرر");
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
        public async Task<int> DeleiverMoneyForClientWithStatus(int[] ids)
        {
            var includes = new string[] { "Client.ClientPhones", "Country" };
            var orders = await _repository.GetByFilterInclue(c => ids.Contains(c.Id), includes);
            var client = orders.FirstOrDefault().Client;
            if (orders.Any(c => c.ClientId != client.Id))
            {
                throw new ConflictException("ليست جميع الشحنات لنفس العميل");
            }
            semaphore.Wait();
            var clientPayment = new ClientPayment()
            {
                Date = DateTime.UtcNow,
                PrinterName = currentUser,
                DestinationName = client.Name,
                DestinationPhone = client.ClientPhones.FirstOrDefault()?.Phone ?? "",

            };
            var total = 0m;
            await _uintOfWork.BegeinTransaction();
            await _uintOfWork.Add(clientPayment);
            var orderClientPaymnets = new List<OrderClientPaymnet>();
            var clientPaymentsDeitlas = new List<ClientPaymentDetail>();
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
                await _uintOfWork.Update(item);
                var orderClientPayment = new OrderClientPaymnet()
                {
                    OrderId = item.Id,
                    ClientPaymentId = clientPayment.Id
                };
                var clientPaymnetDetial = new ClientPaymentDetail()
                {
                    Code = item.Code,
                    Total = item.Cost,
                    Country = item.Country.Name,
                    ClientPaymentId = clientPayment.Id,
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
                orderClientPaymnets.Add(orderClientPayment);
                clientPaymentsDeitlas.Add(clientPaymnetDetial);
            }
            await _uintOfWork.AddRange(orderClientPaymnets);
            await _uintOfWork.AddRange(clientPaymentsDeitlas);
            var treasury = await _uintOfWork.Repository<Treasury>().FirstOrDefualt(c => c.Id == currentUserId);
            treasury.Total -= total;
            var history = new TreasuryHistory()
            {
                ClientPaymentId = clientPayment.Id,
                TreasuryId = currentUserId,
                Amount = -total,
                CreatedOnUtc = DateTime.UtcNow
            };
            await _uintOfWork.Update(treasury);
            await _uintOfWork.Add(history);
            await _uintOfWork.Commit();
            semaphore.Release();
            return clientPayment.Id;
        }
        public async Task<int> DeleiverMoneyForClient(DeleiverMoneyForClientDto deleiverMoneyForClientDto)
        {
            var includes = new string[] { "Client.ClientPhones", "Country" };
            var orders = await _repository.GetByFilterInclue(c => deleiverMoneyForClientDto.Ids.Contains(c.Id), includes);
            var client = orders.FirstOrDefault().Client;
            if (orders.Any(c => c.ClientId != client.Id))
            {
                throw new ConflictException("ليست جميع الشحنات لنفس العميل");
            }
            semaphore.Wait();
            var clientPayment = new ClientPayment()
            {
                Date = DateTime.UtcNow,
                PrinterName = currentUser,
                DestinationName = client.Name,
                DestinationPhone = client.ClientPhones.FirstOrDefault()?.Phone ?? "",

            };
            var total = 0m;
            await _uintOfWork.BegeinTransaction();
            await _uintOfWork.Add(clientPayment);
            if (!orders.All(c => c.OrderplacedId == (int)OrderplacedEnum.CompletelyReturned || c.OrderplacedId == (int)OrderplacedEnum.Unacceptable))
            {
                var recepits = await _uintOfWork.Repository<Receipt>().GetAsync(c => c.ClientPaymentId == null && c.ClientId == client.Id);
                total += recepits.Sum(c => c.Amount);
                recepits.ForEach(c =>
                {
                    c.ClientPaymentId = clientPayment.Id;

                });
                await _uintOfWork.UpdateRange(recepits);
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
                await _repository.Update(item);
                var orderClientPaymnet = new OrderClientPaymnet()
                {
                    OrderId = item.Id,
                    ClientPaymentId = clientPayment.Id
                };

                var clientPaymentDetials = new ClientPaymentDetail()
                {
                    Code = item.Code,
                    Total = item.Cost,
                    Country = item.Country.Name,
                    ClientPaymentId = clientPayment.Id,
                    Phone = item.RecipientPhones,
                    DeliveryCost = item.DeliveryCost,
                    LastTotal = item.OldCost,
                    Note = item.Note,
                    MoneyPlacedId = item.MoenyPlacedId,
                    OrderPlacedId = item.OrderplacedId,
                    PayForClient = currentPay,
                    Date = item.Date,
                    ClientNote = item.ClientNote
                };
                total += currentPay;
                await _uintOfWork.Add(orderClientPaymnet);
                await _uintOfWork.Add(clientPaymentDetials);
            }
            client.Points += totalPoints;
            await _uintOfWork.Update(client);
            if (deleiverMoneyForClientDto.PointsSettingId != null)
            {

                var pointSetting = await _uintOfWork.Repository<PointsSetting>().FirstOrDefualt(c => c.Id == deleiverMoneyForClientDto.PointsSettingId);
                Discount discount = new Discount()
                {
                    Money = pointSetting.Money,
                    Points = pointSetting.Points,
                    ClientPaymentId = clientPayment.Id
                };
                await _uintOfWork.Add(discount);
                total -= discount.Money;
            }
            await _uintOfWork.Add(new Notfication()
            {
                Note = "تم تسديدك برقم " + clientPayment.Id,
                ClientId = client.Id
            });

            var treasury = await _uintOfWork.Repository<Treasury>().FirstOrDefualt(c => c.Id == currentUserId);
            treasury.Total -= total;
            var history = new TreasuryHistory()
            {
                ClientPaymentId = clientPayment.Id,
                CashMovmentId = null,
                TreasuryId = currentUserId,
                Amount = -total,
                CreatedOnUtc = DateTime.UtcNow
            };
            await _uintOfWork.Update(treasury);
            await _uintOfWork.Add(history);
            await _uintOfWork.Commit();
            semaphore.Release();
            return clientPayment.Id;
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

        public async Task<IEnumerable<OrderDto>> ForzenInWay(FrozenOrder frozenOrder)
        {
            var date = frozenOrder.CurrentDate.AddHours(-frozenOrder.Hour);
            IEnumerable<Order> orders;
            if (frozenOrder.AgentId != null)
            {
                orders = await _repository.GetAsync(c => c.Date <= date && c.AgentId == frozenOrder.AgentId && c.OrderplacedId == (int)OrderplacedEnum.Way, c => c.Client, c => c.Region, c => c.Agent, c => c.Country);
            }
            else
            {
                orders = await _repository.GetAsync(c => c.Date <= date && c.OrderplacedId == (int)OrderplacedEnum.Way, c => c.Client, c => c.Region, c => c.Agent, c => c.Country);
            }
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> GetById(int id)
        {
            var order = await _repository.GetByIdIncludeAllForEmployee(id);
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<IEnumerable<PayForClientDto>> OrdersDontFinished(OrderDontFinishedFilter orderDontFinishedFilter)
        {
            var orders = await _repository.OrdersDontFinished(orderDontFinishedFilter);
            return _mapper.Map<IEnumerable<PayForClientDto>>(orders);
        }

        public async Task<IEnumerable<OrderDto>> NewOrderDontSned()
        {
            var includes = new string[] { "Client.ClientPhones", "Client.Country", "Region", "Country.AgentCountries.Agent", "OrderItems.OrderTpye" };
            var orders = await _repository.GetByFilterInclue(c => c.IsSend == false && c.OrderplacedId == (int)OrderplacedEnum.Client, includes);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<IEnumerable<OrderDto>> OrderAtClient(OrderFilter orderFilter)
        {
            var orders = await _repository.OrderAtClient(orderFilter);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<PayForClientDto> GetByCodeAndClient(int clientId, string code)
        {
            var includes = new string[] { "OrderClientPaymnets.ClientPayment", "AgentOrderPrints.AgentPrint" };
            var order = await _repository.FirstOrDefualt(c => c.ClientId == clientId && c.Code == code, includes);
            if (order == null)
                throw new ConflictException("الشحنة غير موجودة");
            if (order.IsClientDiliverdMoney && order.OrderStateId != (int)OrderStateEnum.ShortageOfCash)
            {
                throw new ConflictException("تم تسليم كلفة الشحنة من قبل");
            }
            if (order.OrderplacedId == (int)OrderplacedEnum.Client)
            {
                throw new ConflictException("الشحنة عند العميل");
            }
            if (order.OrderplacedId == (int)OrderplacedEnum.Store)
            {
                throw new ConflictException("الشحنة داخل المخزن");
            }
            await _repository.LoadRefernces(order, c => c.Country);
            await _repository.LoadRefernces(order, c => c.Region);
            await _repository.LoadRefernces(order, c => c.Agent);
            await _countryRepository.LoadCollection(order.Country, c => c.Regions);
            return _mapper.Map<PayForClientDto>(order);
        }

        public async Task ReiveMoneyFromClient(int[] ids)
        {
            var orders = await _repository.GetAsync(c => ids.Contains(c.Id));
            orders.ToList().ForEach(c => c.OrderStateId = (int)OrderStateEnum.Finished);
            await _repository.Update(orders);
        }
        public async Task<EarningsDto> GetEarnings(PagingDto pagingDto, DateFiter dateFiter)
        {
            var predicate = PredicateBuilder.New<Order>(true);
            predicate = predicate.And(c => c.OrderStateId == (int)OrderStateEnum.Finished && c.OrderplacedId != (int)OrderplacedEnum.CompletelyReturned);
            if (dateFiter.FromDate != null)
                predicate = predicate.And(c => c.Date >= dateFiter.FromDate);
            if (dateFiter.ToDate != null)
                predicate = predicate.And(c => c.Date <= dateFiter.ToDate);
            var pagingResualt = await _repository.GetAsync(pagingDto, predicate, c => c.MoenyPlaced);
            var sum = await _repository.Sum(c => c.DeliveryCost - c.AgentCost, predicate);
            return new EarningsDto()
            {
                TotalEarinig = sum,
                TotalRecord = pagingResualt.Total,
                Orders = _mapper.Map<IEnumerable<OrderDto>>(pagingResualt.Data)
            };


        }
        public async Task<IEnumerable<OrderDto>> GetAsync(Expression<Func<Order, bool>> expression, string[] propertySelector = null)
        {
            var orders = await _repository.GetByFilterInclue(expression, propertySelector);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task Accept(IdsDto idsDto)
        {
            var order = await _repository.GetById(idsDto.OrderId);
            if (!await _agentCountryRepository.Any(c => c.CountryId == order.CountryId && c.AgentId == idsDto.AgentId))
            {
                throw new ConflictException("تضارب المندوب و المدينة");
            }
            order.AgentId = idsDto.AgentId;
            order.AgentCost = (await _userRepository.FirstOrDefualt(c => c.Id == idsDto.AgentId))?.Salary ?? 0;
            order.OrderplacedId = (int)OrderplacedEnum.Store;
            order.IsSend = true;
            await _repository.Update(order);
        }

        public async Task AcceptMultiple(IEnumerable<IdsDto> idsDtos)
        {
            //get data 
            var orders = await _repository.GetAsync(c => idsDtos.Select(dto => dto.OrderId).Contains(c.Id));
            var agentsContries = await _agentCountryRepository.GetAsync(c => idsDtos.Select(dto => dto.AgentId).Contains(c.AgentId));

            //validation 
            if (idsDtos.Select(c => c.OrderId).Except(orders.Select(c => c.Id)).Any())
                throw new ConflictException("");

            if (idsDtos.Select(c => c.AgentId).Except(agentsContries.Select(c => c.AgentId)).Any())
                throw new ConflictException("");
            var agentsIds = orders.Select(c => c.AgentId);
            var agents = await _userRepository.GetAsync(c => agentsIds.Contains(c.Id));
            foreach (var item in idsDtos)
            {
                var order = orders.First(c => c.Id == item.OrderId);
                var agentCountries = agentsContries.Where(c => c.AgentId == item.AgentId).ToList();
                if (!agentsContries.Any(c => c.CountryId != order.CountryId))
                {
                    throw new ConflictException("تضارب المندوب و المدينة");
                }
                order.AgentId = item.AgentId;
                order.AgentCost = agents.First(c => c.Id == item.AgentId)?.Salary ?? 0;
                order.OrderplacedId = (int)OrderplacedEnum.Store;
                order.IsSend = true;
            }
            await _repository.Update(orders);
        }

        public async Task DisAccept(DateWithId<int> dateWithId)
        {
            var order = await _uintOfWork.Repository<Order>().FirstOrDefualt(c => c.Id == dateWithId.Ids);
            await _uintOfWork.BegeinTransaction();
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
                UpdatedBy = currentUser,
                UpdatedDate = dateWithId.Date
            };
            await _uintOfWork.Remove(order);
            await _uintOfWork.Add(disAcceptOrder);
            await _uintOfWork.Commit();
        }

        public async Task DisAcceptMultiple(DateWithId<List<int>> dateWithIds)
        {
            var ids = dateWithIds.Ids;
            var orders = await _uintOfWork.Repository<Order>().GetAsync(c => ids.Contains(c.Id));
            if (ids.Except(orders.Select(c => c.Id)).Any())
            {
                throw new ConflictException("");
            }
            var dis = orders.Select(order => new DisAcceptOrder()
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
                UpdatedBy = currentUser,
                UpdatedDate = dateWithIds.Date
            });
            await _uintOfWork.RemoveRange(orders);
            await _uintOfWork.AddRange(dis);
            await _uintOfWork.Commit();

        }
        public async Task ReSend(OrderReSend orderReSend)
        {
            var order = await _repository.FirstOrDefualt(c => c.Id == orderReSend.Id);
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
            order.AgentCost = (await _userRepository.FirstOrDefualt(c => c.Id == order.AgentId)).Salary ?? 0;
            await _repository.Update(order);
        }
        public async Task<OrderDto> MakeOrderCompletelyReturned(int id)
        {

            var order = await _uintOfWork.Repository<Order>().FirstOrDefualt(c => c.Id == id);
            OrderLog log = order;
            await _uintOfWork.BegeinTransaction();
            if (order.OrderplacedId != (int)OrderplacedEnum.Store)
            {
                throw new ConflictException("الشحنة ليست في المخزن");
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
            order.UpdatedBy = currentUser;
            order.SystemNote = "MakeStoreOrderCompletelyReturned";
            await _uintOfWork.Update(order);
            await _uintOfWork.Commit();
            return _mapper.Map<OrderDto>(order);
        }

        public async Task AddPrintNumber(int orderId)
        {
            var order = await _repository.FirstOrDefualt(c => c.Id == orderId);
            order.PrintedTimes += 1;
            await _repository.Update(order);
        }

        public async Task AddPrintNumber(int[] orderIds)
        {
            var orders = await _repository.GetAsync(c => orderIds.Contains(c.Id));
            orders.ForEach(c => c.PrintedTimes += 1);
            await _repository.Update(orders);
        }
        public async Task<IEnumerable<OrderDto>> GetOrderByAgent(string orderCode)
        {
            var orders = await _repository.GetAsync(c => c.Code == orderCode, c => c.Region, c => c.Country, c => c.Client, c => c.Agent);
            if (orders.Count() == 0)
            {
                throw new ConflictException("الشحنة غير موجودة");
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
                    throw new ConflictException("الشحنة ما زالت في المخزن");
                }
                if (lastOrderAdded.MoenyPlacedId == (int)MoneyPalcedEnum.InsideCompany)
                {
                    throw new ConflictException("الشحنة داخل الشركة");
                }
                else
                {
                    throw new ConflictException("تم إستلام الشحنة مسبقاً");
                }

            }
            return _mapper.Map<OrderDto[]>(orders);

        }
        public async Task TransferOrderToAnotherAgnet(TransferOrderToAnotherAgnetDto transferOrderToAnotherAgnetDto)
        {
            var agnet = await _userRepository.GetById(transferOrderToAnotherAgnetDto.NewAgentId);
            var orders = await _repository.GetAsync(c => transferOrderToAnotherAgnetDto.Ids.Contains(c.Id));
            orders.ForEach(c =>
            {
                c.AgentId = agnet.Id;
                c.AgentCost = agnet.Salary ?? 0;
            });
            await _repository.Update(orders);
        }
        public async Task Edit(UpdateOrder updateOrder)
        {
            var order = await _uintOfWork.Repository<Order>().FirstOrDefualt(c => c.Id == updateOrder.Id);
            OrderLog log = order;
            if (order.Code != updateOrder.Code)
            {
                if (await _uintOfWork.Repository<Order>().Any(c => c.ClientId == order.ClientId && c.Code == updateOrder.Code))
                {
                    throw new ConflictException("الكود{order.Code} مكرر");
                }
            }
            order.Code = updateOrder.Code;
            if (order.AgentId != updateOrder.AgentId)
            {
                order.OrderStateId = (int)OrderStateEnum.Processing;
                order.MoenyPlacedId = (int)MoneyPalcedEnum.OutSideCompany;
                order.OrderplacedId = (int)OrderplacedEnum.Store;
            }
            await _uintOfWork.BegeinTransaction();
            await _uintOfWork.Add(log);
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
                    await _uintOfWork.Add(receipt);
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
            await _uintOfWork.Update(order);
            await _uintOfWork.Commit();
        }
        public async Task<PrintOrdersDto> GetOrderByClientPrintNumber(int printNumber)
        {
            var includes = new string[] { "Discounts", "Receipts", "ClientPaymentDetails.OrderPlaced" };
            var printed = await _clientPaymentRepository.FirstOrDefualt(c => c.Id == printNumber, includes);
            if (printed == null)
                throw new ConflictException("رقم الطباعة غير موجود");
            return _mapper.Map<PrintOrdersDto>(printed);
        }
        public async Task<PagingResualt<IEnumerable<PrintOrdersDto>>> GetClientprint(PagingDto pagingDto, int? number, string clientName, string code)
        {
            var exprtion = PredicateBuilder.New<ClientPayment>(true);
            if (number != null)
            {
                exprtion = exprtion.And(c => c.Id == number);
            }
            if (clientName != null)
            {
                exprtion = exprtion.And(c => c.DestinationName == clientName);
            }
            if (!string.IsNullOrEmpty(code))
            {
                exprtion = exprtion.And(c => c.ClientPaymentDetails.Any(c => c.Code.StartsWith(code)));
            }
            var includes = new string[] { "ClientPaymentDetails.OrderPlaced" };
            var paginResult = await _clientPaymentRepository.GetAsync(pagingDto, exprtion, includes, c => c.OrderByDescending(c => c.Id));
            return new PagingResualt<IEnumerable<PrintOrdersDto>>()
            {
                Total = paginResult.Total,
                Data = _mapper.Map<IEnumerable<PrintOrdersDto>>(paginResult.Data)
            };
        }

        public async Task<PagingResualt<IEnumerable<OrderDto>>> DisAccpted(PagingDto pagingDto, OrderFilter orderFilter)
        {
            var predicate = PredicateBuilder.New<DisAcceptOrder>(true);

            if (orderFilter.CountryId != null)
            {
                predicate = predicate.And(c => c.CountryId == orderFilter.CountryId);
            }
            if (orderFilter.Code != string.Empty && orderFilter.Code != null)
            {
                predicate = predicate.And(c => c.Code.StartsWith(orderFilter.Code));
            }
            if (orderFilter.ClientId != null)
            {
                predicate = predicate.And(c => c.ClientId == orderFilter.ClientId);
            }
            if (orderFilter.RegionId != null)
            {
                predicate = predicate.And(c => c.RegionId == orderFilter.RegionId);
            }
            if (orderFilter.RecipientName != string.Empty && orderFilter.RecipientName != null)
            {
                predicate = predicate.And(c => c.RecipientName.StartsWith(orderFilter.RecipientName));
            }
            if (orderFilter.Phone != string.Empty && orderFilter.Phone != null)
            {
                predicate = predicate.And(c => c.RecipientPhones.Contains(orderFilter.Phone));
            }
            if (orderFilter.CreatedDate != null)
            {
                predicate = predicate.And(c => c.Date == orderFilter.CreatedDate);
            }
            var pagingResult = await _DisAcceptOrderRepository.GetAsync(pagingDto, predicate, c => c.Client, c => c.Region, c => c.Country);
            return new PagingResualt<IEnumerable<OrderDto>>()
            {
                Total = pagingResult.Total,
                Data = _mapper.Map<IEnumerable<OrderDto>>(pagingResult.Data)
            };
        }

        public async Task<int> Count(Expression<Func<Order, bool>> filter = null)
        {
            return await _repository.Count(filter);
        }

        public async Task<IEnumerable<ApproveAgentEditOrderRequestDto>> GetOrderRequestEditState()
        {
            var orders = await _repository.GetAsync(c => c.AgentRequestStatus == (int)AgentRequestStatusEnum.Pending, c => c.Agent, c => c.NewOrderPlaced);
            return _mapper.Map<ApproveAgentEditOrderRequestDto[]>(orders);
        }

        public async Task DisAproveOrderRequestEditState(int[] ids)
        {
            var orders = await _repository.GetAsync(c => ids.Contains(c.Id));
            orders.ForEach(c =>
            {
                c.AgentRequestStatus = (int)AgentRequestStatusEnum.DisApprove;
                c.NewOrderPlacedId = null;
                c.NewCost = null;
            });
            await _repository.Update(orders);
        }
        public async Task AproveOrderRequestEditState(int[] ids)
        {
            var orders = await _uintOfWork.Repository<Order>().GetAsync(c => ids.Contains(c.Id));
            orders.ForEach(c =>
            {
                c.AgentRequestStatus = (int)AgentRequestStatusEnum.Approve;
            });
            await _uintOfWork.BegeinTransaction();

            List<Notfication> notficationsGroup = new List<Notfication>();
            List<Notfication> addednotfications = new List<Notfication>();
            List<Notfication> notfications = new List<Notfication>();
            List<OrderLog> logs = new List<OrderLog>();
            foreach (var order in orders)
            {

                OrderLog log = order;
                logs.Add(log);
                order.OrderplacedId = order.NewOrderPlacedId.Value;
                order.AgentRequestStatus = (int)AgentRequestStatusEnum.Approve;

                order.SystemNote = "OrderRequestEditStateCount";
                if (order.IsClientDiliverdMoney)
                {
                    switch (order.OrderplacedId)
                    {
                        case (int)OrderplacedEnum.Delivered:
                            {
                                if (Decimal.Compare(order.Cost, order.NewCost.Value) != 0)
                                {
                                    if (order.OldCost == null)
                                        order.OldCost = order.Cost;
                                    order.Cost = order.NewCost.Value;
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
                                order.Cost = order.NewCost.Value;
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
                                if (order.Cost != order.NewCost.Value)
                                {
                                    if (order.OldCost == null)
                                        order.OldCost = order.Cost;
                                    order.Cost = order.NewCost.Value;
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
                if (order.OrderStateId != (int)OrderStateEnum.Finished && order.OrderplacedId != (int)OrderplacedEnum.Way)
                {
                    var clientNotigaction = notficationsGroup.Where(c => c.ClientId == order.ClientId && c.OrderPlacedId == order.OrderplacedId && c.MoneyPlacedId == order.MoenyPlacedId).FirstOrDefault();
                    if (clientNotigaction == null)
                    {
                        clientNotigaction = new Notfication()
                        {
                            ClientId = order.ClientId,
                            OrderPlacedId = order.NewOrderPlacedId.Value,
                            MoneyPlacedId = (int)MoneyPalcedEnum.WithAgent,
                            IsSeen = false,
                            OrderCount = 1
                        };
                        notficationsGroup.Add(clientNotigaction);
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
                notfications.Add(notfication);
            }
            await _uintOfWork.AddRange(notfications);
            await _uintOfWork.AddRange(notficationsGroup);
            addednotfications.AddRange(notfications);
            addednotfications.AddRange(notficationsGroup);

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
                    await _notificationHub.AllNotification(key.ToString(), notficationDtos.ToArray());
                }
            }
            await _uintOfWork.Commit();

        }

        public async Task<IEnumerable<string>> GetCreatedByNames()
        {
            return await _repository.GetCreatedByNames();
        }

        public async Task<IEnumerable<OrderDto>> GetForReSendMultiple(string code)
        {

            var includes = new string[] { nameof(Order.Client), nameof(Order.Agent), nameof(Order.Country), nameof(Order.Region) };
            var orders = await _repository.GetAsync(c => c.Code == code && c.OrderplacedId != (int)OrderplacedEnum.Delivered && c.OrderplacedId != (int)OrderplacedEnum.PartialReturned && c.OrderplacedId != (int)OrderplacedEnum.Client, c => c.Client, c => c.Agent, c => c.Region, c => c.Country);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task ReSendMultiple(List<OrderReSend> orderReSends)
        {
            var ordersIds = orderReSends.Select(c => c.Id);

            var orders = await _repository.GetAsync(c => ordersIds.Contains(c.Id));
            var agentIds = orderReSends.Select(c => c.AgnetId).Distinct();
            var agents = await _userRepository.Select(c => new { c.Id, c.Salary }, c => agentIds.Contains(c.Id));
            foreach (var orderReSend in orderReSends)
            {
                var order = orders.Single(c => c.Id == orderReSend.Id);
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
                order.AgentCost = agents.Single(c => c.Id == order.AgentId).Salary ?? 0;
            }
            await _repository.Update(orders);

        }

        public async Task<PagingResualt<IEnumerable<OrderDto>>> GetInStockToTransferToSecondBranch(PagingDto pagingDto, OrderFilter filter)
        {
            var predicate = GetFilterAsLinq(filter);
            predicate = predicate.And(c => c.OrderplacedId == (int)OrderplacedEnum.Store & c.SecondBranchId != null && c.CurrentBranchId == _currentBranchId && c.BranchId == _currentBranchId && c.InWayToBranch == false);
            var pagingResult = await _repository.GetAsync(pagingDto, predicate, c => c.Country, c => c.Client);
            return new PagingResualt<IEnumerable<OrderDto>>()
            {
                Total = pagingResult.Total,
                Data = _mapper.Map<IEnumerable<OrderDto>>(pagingResult.Data)
            };
        }

        public async Task ReceiveOrdersToMyBranch(IEnumerable<ReceiveOrdersToMyBranchDto> receiveOrdersToMyBranchDtos)
        {
            var ids = receiveOrdersToMyBranchDtos.Select(c => c.OrderId);
            var orders = await _repository.GetAsync(c => ids.Contains(c.Id));
            receiveOrdersToMyBranchDtos.ForEach(rmb =>
            {
                var order = orders.First(c => c.Id == rmb.OrderId);
                order.CurrentBranchId = _currentBranchId;
                order.OrderplacedId = (int)OrderplacedEnum.Store;
                order.DeliveryCost = rmb.DeliveryCost;
                order.RegionId = rmb.RegionId;
                order.AgentId = rmb.AgentId;
                order.InWayToBranch = false;
            });
            await _repository.Update(orders);
        }

        public async Task<IEnumerable<OrderDto>> GetOrderReturnedToSecondBranch(string code)
        {
            var order = await _repository.GetAsync(c => c.Code == code && c.CurrentBranchId == _currentBranchId && (c.OrderplacedId == (int)OrderplacedEnum.CompletelyReturned || c.OrderplacedId == (int)OrderplacedEnum.PartialReturned || c.OrderplacedId == (int)OrderplacedEnum.Unacceptable) && c.MoenyPlacedId == (int)MoneyPalcedEnum.InsideCompany && c.InWayToBranch == false);
            return _mapper.Map<IEnumerable<OrderDto>>(order);
        }

        public async Task SendOrdersReturnedToSecondBranch(int[] ids)
        {
            var orders = await _repository.GetAsync(c => ids.Contains(c.Id));
            if (orders.Any(c => c.CurrentBranchId != _currentBranchId))
                throw new ConflictException("الشحنة ليست في فرعك");
            if (orders.Any(c => !c.IsOrderReturn()))
                throw new ConflictException("الشحنة ليست مرتجعة");
            if (orders.Any(c => !c.IsOrderInMyStore()))
                throw new ConflictException("الشحنة ليست في المخزن");
            orders.ForEach(c =>
            {
                c.InWayToBranch = true;
            });
            await _repository.Update(orders);
        }
        public async Task<PagingResualt<IEnumerable<OrderDto>>> GetOrdersReturnedToMyBranch(PagingDto pagingDto)
        {
            var pagingResualt = await _repository.GetAsync(pagingDto, c => c.InWayToBranch && c.BranchId == _currentBranchId && c.CurrentBranchId != _currentBranchId);
            return new PagingResualt<IEnumerable<OrderDto>>()
            {
                Total = pagingResualt.Total,
                Data = _mapper.Map<IEnumerable<OrderDto>>(pagingResualt.Data)
            };
        }
        public async Task ReceiveReturnedToMyBranch(int[] ids)
        {
            var orders = await _repository.GetAsync(c => ids.Contains(c.Id));
            orders.ForEach(c =>
            {
                c.CurrentBranchId = _currentBranchId;
                c.InWayToBranch = false;
            });
            await _repository.Update(orders);
        }
    }


}
