﻿using AutoMapper;
using Quqaz.Web.CustomException;
using Quqaz.Web.DAL.Helper;
using Quqaz.Web.DAL.Infrastructure.Interfaces;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.NotifcationDtos;
using Quqaz.Web.Dtos.OrdersDtos;
using Quqaz.Web.Dtos.OrdersDtos.OrderWithBranchDto;
using Quqaz.Web.Dtos.OrdersDtos.Queries;
using Quqaz.Web.Helpers;
using Quqaz.Web.Helpers.Extensions;
using Quqaz.Web.HubsConfig;
using Quqaz.Web.Models;
using Quqaz.Web.Models.Static;
using Quqaz.Web.Models.TransferToBranchModels;
using Quqaz.Web.Services.Interfaces;
using LinqKit;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Quqaz.Web.Dtos.OrdersDtos.Commands;
using Quqaz.Web.Models.SendOrdersReturnedToMainBranchModels;

namespace Quqaz.Web.Services.Concret
{
    public partial class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IUintOfWork _uintOfWork;
        private readonly IRepository<SendOrdersReturnedToMainBranchReport> _sendOrdersReturnedToMainBranchReportRepository;
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
        private readonly IRepository<TransferToOtherBranch> _transferToOtherBranchRepository;
        private readonly IRepository<TransferToOtherBranchDetials> _transferToOtherBranchDetialsRepository;
        private readonly NotificationHub _notificationHub;
        private readonly IRepository<Branch> _branchRepository;
        private readonly Logging _logging;
        private readonly IHttpContextAccessorService _httpContextAccessorService;
        private readonly IWebHostEnvironment _environment;
        private readonly IRepository<MediatorCountry> _mediatorCountry;
        private readonly IRepository<Driver> _driverRepo;
        private readonly int _currentBranchId;
        static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private static readonly Func<Order, bool> _finishOrderPlaceExpression = c => c.OrderPlace == OrderPlace.CompletelyReturned || c.OrderPlace == OrderPlace.Unacceptable;
        private readonly string currentUser;
        private readonly int currentUserId;
        public OrderService(IUintOfWork uintOfWork, IOrderRepository repository, INotificationService notificationService,
            ITreasuryService treasuryService, IMapper mapper, IUserService userService,
            IRepository<ReceiptOfTheOrderStatus> receiptOfTheOrderStatusRepository, Logging logging,
            IRepository<ReceiptOfTheOrderStatusDetali> receiptOfTheOrderStatusDetalisRepository,
            ICountryCashedService countryCashedService,
            IRepository<Country> countryRepository, IRepository<AgentCountry> agentCountryRepository,
            IRepository<User> userRepository, IRepository<ClientPayment> clientPaymentRepository, IRepository<DisAcceptOrder> disAcceptOrderRepository, NotificationHub notificationHub, IRepository<Branch> branchRepository, IHttpContextAccessorService httpContextAccessorService, IRepository<TransferToOtherBranch> transferToOtherBranch, IWebHostEnvironment environment, IRepository<TransferToOtherBranchDetials> transferToOtherBranchDetialsRepository
, IRepository<MediatorCountry> mediatorCountry, IRepository<Driver> driverRepo, IRepository<SendOrdersReturnedToMainBranchReport> sendOrdersReturnedToMainBranchReportRepository)
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
            _httpContextAccessorService = httpContextAccessorService;
            _countryCashedService = countryCashedService;
            currentUser = _httpContextAccessorService.AuthoticateUserName();
            _countryRepository = countryRepository;
            _agentCountryRepository = agentCountryRepository;
            _userRepository = userRepository;
            currentUserId = _httpContextAccessorService.AuthoticateUserId();
            _clientPaymentRepository = clientPaymentRepository;
            _DisAcceptOrderRepository = disAcceptOrderRepository;
            _notificationHub = notificationHub;
            _branchRepository = branchRepository;
            _httpContextAccessorService = httpContextAccessorService;
            _currentBranchId = _httpContextAccessorService.CurrentBranchId();
            _transferToOtherBranchRepository = transferToOtherBranch;
            _environment = environment;
            _transferToOtherBranchDetialsRepository = transferToOtherBranchDetialsRepository;
            _mediatorCountry = mediatorCountry;
            _driverRepo = driverRepo;
            _sendOrdersReturnedToMainBranchReportRepository = sendOrdersReturnedToMainBranchReportRepository;
        }

        public async Task<GenaricErrorResponse<IEnumerable<OrderDto>, string, IEnumerable<string>>> GetOrderToReciveFromAgent(string code)
        {
            var orders = await _uintOfWork.Repository<Order>().GetAsync(c => c.Code == code && (c.BranchId == _currentBranchId || c.CurrentBranchId == _currentBranchId), c => c.Client, c => c.Agent, c => c.Country);

            if (!orders.Any())
            {
                throw new ConflictException("الشحنة غير موجودة");
            }
            var lastOrderAdded = orders.OrderBy(c => c.Id).Last();
            ///التأكد من ان الشحنة ليست عند العميل او في المخزن
            {
                var orderInSotre = orders.Where(c => c.OrderPlace == OrderPlace.Store);
                var orderWithClient = orders.Where(c => c.OrderPlace == OrderPlace.Client);
                orders = orders.Except(orderInSotre.Union(orderWithClient));
            }
            {
                bool finishOrderExpression(Order c) => (_finishOrderPlaceExpression(c) && c.CurrentBranchId == _currentBranchId) || (c.OrderPlace == OrderPlace.Delivered && (c.MoneyPlace == MoneyPalce.InsideCompany || c.MoneyPlace == MoneyPalce.Delivered));
                var finishOrders = orders.Where(finishOrderExpression);
                orders = orders.Except(finishOrders).ToList();
            }
            {
            }
            {
                var orderInCompany = orders.Where(c => c.MoneyPlace == MoneyPalce.InsideCompany && c.CurrentBranchId == _currentBranchId).ToList();
                orders = orders.Except(orderInCompany).ToList();
            }
            if (!orders.Any())
            {
                var lastOrderPlacedAdded = lastOrderAdded.OrderPlace;
                if (lastOrderAdded.OrderPlace == OrderPlace.Store)
                    throw new ConflictException("الشحنة في المخزن");
                if (lastOrderAdded.OrderPlace == OrderPlace.Client)
                    throw new ConflictException("الشحنة عند العميل");
                if (lastOrderAdded.MoneyPlace == MoneyPalce.InsideCompany)
                {
                    if (lastOrderAdded.CurrentBranchId == _currentBranchId)
                        throw new ConflictException("الشحنة داخل الشركة");
                }
            }
            return new GenaricErrorResponse<IEnumerable<OrderDto>, string, IEnumerable<string>>(_mapper.Map<OrderDto[]>(orders));
        }
        public async Task<ErrorResponse<string, IEnumerable<string>>> ReceiptOfTheStatusOfTheDeliveredShipment(IEnumerable<ReceiptOfTheStatusOfTheDeliveredShipmentWithCostDto> receiptOfTheStatusOfTheDeliveredShipmentWithCostDtos)
        {
            var moneyPlacedes = Consts.MoneyPlaceds;
            var orderPlacedes = Consts.OrderPlaceds;
            var outSideCompny = moneyPlacedes.First(c => c.Id == (int)MoneyPalce.OutSideCompany).Name;
            var response = new ErrorResponse<string, IEnumerable<string>>();
            if (!receiptOfTheStatusOfTheDeliveredShipmentWithCostDtos.All(c => c.OrderplacedId == OrderPlace.Way || c.OrderplacedId == OrderPlace.Delivered || c.OrderplacedId == OrderPlace.PartialReturned))
            {
                var wrongData = receiptOfTheStatusOfTheDeliveredShipmentWithCostDtos.Where(c => !(c.OrderplacedId == OrderPlace.Way || c.OrderplacedId == OrderPlace.Delivered || c.OrderplacedId == OrderPlace.PartialReturned));
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
            var OrderNotInWay = orders.Where(c => c.OrderPlace != OrderPlace.Way);
            if (OrderNotInWay.Any())
            {
                throw new ConflictException("هناك شحنات ليست مع المندوب");
            }
            List<OrderLog> logs = new List<OrderLog>();
            foreach (var item in receiptOfTheStatusOfTheDeliveredShipmentWithCostDtos)
            {
                var order = orders.First(c => c.Id == item.Id);
                logs.Add(order);
                order.MoneyPlace = item.MoenyPlacedId;
                order.OrderPlace = item.OrderplacedId;
                order.Note = item.Note;
                if (order.DeliveryCost != item.DeliveryCost)
                {
                    order.OldDeliveryCost ??= order.DeliveryCost;
                }
                order.DeliveryCost = item.DeliveryCost;
                order.AgentCost = item.AgentCost;
                order.SystemNote = "ReceiptOfTheStatusOfTheDeliveredShipment";
                if (order.IsClientDiliverdMoney)
                {
                    switch (order.OrderPlace)
                    {
                        case OrderPlace.Delivered:
                            {
                                if (decimal.Compare(order.Cost, item.Cost) != 0)
                                {
                                    order.OldCost ??= order.Cost;
                                    order.Cost = item.Cost;
                                }
                                var payForClient = order.ShouldToPay();

                                if (decimal.Compare(payForClient, (order.ClientPaied ?? 0)) != 0)
                                {
                                    order.OrderState = OrderState.ShortageOfCash;
                                    if (order.MoneyPlace == MoneyPalce.Delivered)
                                    {
                                        order.MoneyPlace = MoneyPalce.InsideCompany;
                                    }
                                }
                                else
                                {
                                    order.OrderState = OrderState.Finished;
                                }
                            }
                            break;
                        case OrderPlace.PartialReturned:
                            {
                                order.OldCost ??= order.Cost;
                                order.Cost = item.Cost;
                                order.OrderState = OrderState.ShortageOfCash;
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
                if (order.MoneyPlace == MoneyPalce.InsideCompany)
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
                        AgentId = order.AgentId.Value,
                        MoneyPalce = order.MoneyPlace,
                        OrderPlace = order.OrderPlace,
                        OrderState = order.OrderState,
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
            var outSideCompny = moneyPlacedes.First(c => c.Id == (int)MoneyPalce.OutSideCompany).Name;
            var response = new ErrorResponse<string, IEnumerable<string>>();

            var orders = (await _uintOfWork.Repository<Order>().GetAsync(c => new HashSet<int>(receiptOfTheStatusOfTheDeliveredShipmentDtos.Select(c => c.Id)).Contains(c.Id))).ToList();
            var repatedOrders = orders.Where(order => receiptOfTheStatusOfTheDeliveredShipmentDtos.Any(r => r.EqualToOrder(order) && order.CurrentBranchId == _currentBranchId)).ToList();
            orders = orders.Except(repatedOrders).ToList();
            var exptedOrdersIds = repatedOrders.Select(c => c.Id);
            receiptOfTheStatusOfTheDeliveredShipmentDtos = receiptOfTheStatusOfTheDeliveredShipmentDtos.Where(c => !exptedOrdersIds.Contains(c.Id));
            if (!receiptOfTheStatusOfTheDeliveredShipmentDtos.Any())
            {
                return new ErrorResponse<string, IEnumerable<string>>();
            }
            if (!receiptOfTheStatusOfTheDeliveredShipmentDtos.All(c => c.OrderplacedId == OrderPlace.Way || c.OrderplacedId == OrderPlace.CompletelyReturned || c.OrderplacedId == OrderPlace.Unacceptable || c.OrderplacedId == OrderPlace.Delayed))
            {
                var wrongData = receiptOfTheStatusOfTheDeliveredShipmentDtos.Where(c => !(c.OrderplacedId == OrderPlace.Way || c.OrderplacedId == OrderPlace.CompletelyReturned || c.OrderplacedId == OrderPlace.Unacceptable || c.OrderplacedId == OrderPlace.Delayed)).ToList();
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
                order.MoneyPlace = item.MoenyPlacedId;
                order.CurrentBranchId = _currentBranchId;
                order.OrderPlace = item.OrderplacedId;
                order.Note = item.Note;

                if (order.DeliveryCost != item.DeliveryCost)
                {
                    order.OldDeliveryCost ??= order.DeliveryCost;
                }
                order.DeliveryCost = item.DeliveryCost;
                order.AgentCost = item.AgentCost;
                order.SystemNote = "ReceiptOfTheStatusOfTheReturnedShipment";
                if (order.IsClientDiliverdMoney)
                {
                    switch (order.OrderPlace)
                    {
                        case OrderPlace.CompletelyReturned:
                            {
                                order.OldCost ??= order.Cost;
                                order.Cost = 0;
                                order.AgentCost = 0;
                                order.OldDeliveryCost = order.DeliveryCost;
                                order.DeliveryCost = 0;
                                order.OrderState = OrderState.ShortageOfCash;
                            }
                            break;
                        case OrderPlace.Unacceptable:
                            {
                                order.OldCost ??= order.Cost;
                                order.OrderState = OrderState.ShortageOfCash;
                            }
                            break;

                    }
                }
                else
                {
                    switch (order.OrderPlace)
                    {
                        case OrderPlace.CompletelyReturned:
                            {
                                order.OldCost ??= order.Cost;
                                order.Cost = 0;
                                order.OldDeliveryCost ??= order.DeliveryCost;
                                order.DeliveryCost = 0;
                                order.AgentCost = 0;
                            }
                            break;
                        case OrderPlace.Unacceptable:
                            {
                                order.OldCost ??= order.Cost;
                                order.Cost = 0;
                            }
                            break;
                    }
                }
                if (order.MoneyPlace == MoneyPalce.InsideCompany)
                {
                    order.AgentRequestStatus = (int)AgentRequestStatusEnum.None;
                }
            }
            var orderNotForMyBranch = orders.Where(c => c.BranchId != _currentBranchId).ToList();
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
                        AgentId = order.AgentId.Value,
                        MoneyPalce = order.MoneyPlace,
                        OrderPlace = order.OrderPlace,
                        OrderState = order.OrderState,
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
            var includes = new string[] { nameof(ReceiptOfTheOrderStatus.Recvier), $"{nameof(ReceiptOfTheOrderStatus.ReceiptOfTheOrderStatusDetalis)}.{nameof(ReceiptOfTheOrderStatusDetali.Agent)}", $"{nameof(ReceiptOfTheOrderStatus.ReceiptOfTheOrderStatusDetalis)}.{nameof(ReceiptOfTheOrderStatusDetali.Client)}" };
            var response = (await _receiptOfTheOrderStatusRepository.GetByFilterInclue(c => c.Id == id, includes)).FirstOrDefault();
            var dto = _mapper.Map<ReceiptOfTheOrderStatusDto>(response);
            return new GenaricErrorResponse<ReceiptOfTheOrderStatusDto, string, IEnumerable<string>>(dto);
        }
        public async Task<int> MakeOrderInWay(MakeOrderInWayDto makeOrderInWayDto)
        {
            var includes = new string[] { $"{nameof(Order.Agent)}.{nameof(User.UserPhones)}", nameof(Order.Client), nameof(Order.Country), nameof(Order.Region) };
            var orders = await _uintOfWork.Repository<Order>().GetByFilterInclue(c => makeOrderInWayDto.Ids.Contains(c.Id), includes);
            if (orders.Any(c => c.OrderPlace != OrderPlace.Store))
            {
                var errors = orders.Where(c => c.OrderPlace != OrderPlace.Store).Select(c => $"الشحنة رقم{c.Code} ليست في المخزن");
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
            if (makeOrderInWayDto.Driver.DriverId != null)
            {
                agnetPrint.DriverId = makeOrderInWayDto.Driver.DriverId;
            }
            else
            {
                if (string.IsNullOrEmpty(makeOrderInWayDto.Driver.DriverName))
                    throw new ConflictException("يجب اختيار السائق");
                var driver = await _driverRepo.FirstOrDefualt(c => c.Name == makeOrderInWayDto.Driver.DriverName);
                driver ??= new Driver()
                {
                    Name = makeOrderInWayDto.Driver.DriverName,
                };
                agnetPrint.Driver = driver;
            }
            await _uintOfWork.BegeinTransaction();
            var agnetOrderPrints = new List<AgentOrderPrint>();
            var agentPrintsDetials = new List<AgentPrintDetail>();
            await _uintOfWork.Repository<AgentPrint>().AddAsync(agnetPrint);
            List<OrderLog> logs = new List<OrderLog>();
            foreach (var item in orders)
            {
                OrderLog log = item;
                logs.Add(item);
                item.SystemNote = "نقل إلى الطريق";

                item.OrderPlace = OrderPlace.Way;

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
            await _uintOfWork.AddRange(logs);
            await _uintOfWork.Commit();
            return agnetPrint.Id;
        }
        public async Task<PagingResualt<IEnumerable<OrderDto>>> GetOrdersComeToMyBranch(PagingDto pagingDto, OrderFilter orderFilter)
        {
            var predicate = GetFilterAsLinq(orderFilter);
            predicate = predicate.And(c => c.NextBranchId == _currentBranchId && c.CurrentBranchId != _currentBranchId && c.OrderPlace == OrderPlace.Way && c.InWayToBranch && !c.IsReturnedByBranch);
            var pagingResult = await _repository.GetAsync(pagingDto, predicate, c => c.Country, c => c.Client);
            return new PagingResualt<IEnumerable<OrderDto>>()
            {
                Total = pagingResult.Total,
                Data = _mapper.Map<IEnumerable<OrderDto>>(pagingResult.Data)
            };
        }
        public async Task<int> TransferToSecondBranch(TransferToSecondBranchDto transferToSecondBranchDto)
        {
            var transaction = _uintOfWork.BegeinTransaction();
            try
            {
                var predicate = GetFilterAsLinq(transferToSecondBranchDto.SelectedOrdersWithFitlerDto);
                predicate = predicate.And(c => c.OrderPlace == OrderPlace.Store & c.NextBranchId != null && c.CurrentBranchId == _currentBranchId && c.InWayToBranch == false);
                var orders = await _repository.GetAsync(predicate, c => c.Country, c => c.Client);

                if (!orders.Any())
                {
                    throw new ConflictException("الشحنات غير موجودة");
                }
                if (orders.Any(c => c.CurrentBranchId != _currentBranchId || c.OrderPlace != OrderPlace.Store || c.NextBranchId == null))
                {
                    throw new ConflictException("هناك شحنات لا يمكن إرسالها ");
                }

                var destinationBranchId = orders.First().NextBranchId.Value;
                if (orders.Any(c => c.NextBranchId != destinationBranchId))
                {
                    throw new ConflictException("ليست جميع الشحنات لنفس الوجهة");
                }
                orders.ForEach(c =>
                {
                    c.OrderPlace = OrderPlace.Way;
                    c.InWayToBranch = true;
                });
                var transferToOtherBranch = new TransferToOtherBranch
                {
                    SourceBranchId = _currentBranchId,
                    DestinationBranchId = destinationBranchId,
                    CreatedOnUtc = DateTime.UtcNow,
                    PrinterName = _httpContextAccessorService.AuthoticateUserName()
                };
                if (transferToSecondBranchDto.Driver.DriverId != null)
                {
                    transferToOtherBranch.DriverId = transferToSecondBranchDto.Driver.DriverId.Value;
                }
                else
                {
                    var driver = await _driverRepo.FirstOrDefualt(c => c.Name == transferToSecondBranchDto.Driver.DriverName);
                    driver ??= new Driver()
                    {
                        Name = transferToSecondBranchDto.Driver.DriverName
                    };
                    transferToOtherBranch.Driver = driver;
                }
                var transferToOtherBranchDetials = new List<TransferToOtherBranchDetials>();
                foreach (var item in orders)
                {
                    transferToOtherBranchDetials.Add(new TransferToOtherBranchDetials()
                    {
                        Code = item.Code,
                        Total = item.Cost,
                        CountryName = item.Country.Name,
                        ClientName = item.Client.Name,
                        OrderDate = item.Date.Value,
                        Phone = item.RecipientPhones,
                        Note = item.Note,
                    });
                }
                transferToOtherBranch.TransferToOtherBranchDetials = transferToOtherBranchDetials;
                await _uintOfWork.Add(transferToOtherBranch);
                await _uintOfWork.Update(orders);
                await _uintOfWork.Commit();
                return transferToOtherBranch.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> GetTransferToSecondBranchReportAsString(int id)
        {

            var includes = new string[] { nameof(TransferToOtherBranch.TransferToOtherBranchDetials), nameof(TransferToOtherBranch.TransferToOtherBranchDetials), nameof(TransferToOtherBranch.DestinationBranch), nameof(TransferToOtherBranch.SourceBranch) };
            var report = await _transferToOtherBranchRepository.FirstOrDefualt(c => c.Id == id, includes);
            var path = _environment.WebRootPath + "/HtmlTemplate/TransferToOtherBranchTemplate.html";
            var readText = await File.ReadAllTextAsync(path);
            readText = readText.Replace("{{printNumber}}", report.Id.ToString());
            readText = readText.Replace("{{userName}}", report.PrinterName);
            readText = readText.Replace("{{dateOfPrint}}", report.CreatedOnUtc.ToString("yyyy-MM-dd"));
            readText = readText.Replace("{{timeOfPrint}}", report.CreatedOnUtc.ToString("HH:mm"));
            readText = readText.Replace("{{fromBranch}}", report.SourceBranch.Name);
            readText = readText.Replace("{{toBranch}}", report.DestinationBranch.Name);
            var c = 1;
            var rows = new StringBuilder();
            foreach (var item in report.TransferToOtherBranchDetials)
            {

                rows.Append(@"<tr style=""border: 1px black solid;padding: 5px;text-align: center;margin-bottom: 20%;overflow: auto;"">");
                rows.Append(@"<td style=""width: 3%;border: 1px black solid;padding: 5px;text-align: center;"">");
                rows.Append(c.ToString());
                rows.Append("</td>");
                rows.Append(@"<td style=""width: 5%;border: 1px black solid;padding: 5px;text-align: center;"">");
                rows.Append(item.Code);
                rows.Append("</td>");
                rows.Append(@"<td style=""width: 15%;border: 1px black solid;padding: 5px;text-align: center;"">");
                rows.Append(item.Total);
                rows.Append("</td>");
                rows.Append(@"<td style=""width: 15%;border: 1px black solid;padding: 5px;text-align: center;"">");
                rows.Append(item.OrderDate.ToString("yyyy-MM-dd"));
                rows.Append("</td>");
                rows.Append(@"<td style=""width: 10%;border: 1px black solid;padding: 5px;text-align: center;"">");
                rows.Append(item.CountryName);
                rows.Append("</td>");
                rows.Append(@"<td style=""width: 12%;border: 1px black solid;padding: 5px;text-align: center;"">");
                rows.Append(item.ClientName);
                rows.Append("</td>");
                rows.Append("</tr>");
                c++;
            }
            readText = readText.Replace("{orders}", rows.ToString());
            return readText;
        }
        public async Task<PagingResualt<IEnumerable<TransferToSecondBranchReportDto>>> GetPrintsTransferToSecondBranch(PagingDto pagingDto, int destinationBranchId)
        {
            var predicate = PredicateBuilder.New<TransferToOtherBranch>(true);
            predicate = predicate.And(c => c.DestinationBranchId == destinationBranchId);
            predicate = predicate.And(c => c.SourceBranchId == _currentBranchId);
            var includes = new string[] { nameof(TransferToOtherBranch.DestinationBranch), nameof(TransferToOtherBranch.Driver) };
            var data = await _transferToOtherBranchRepository.GetAsync(paging: pagingDto, filter: predicate, propertySelectors: includes, orderBy: c => c.OrderByDescending(t => t.Id));
            return new PagingResualt<IEnumerable<TransferToSecondBranchReportDto>>()
            {
                Total = data.Total,
                Data = _mapper.Map<IEnumerable<TransferToSecondBranchReportDto>>(data.Data)
            };

        }

        public async Task<PagingResualt<IEnumerable<ReceiptOfTheOrderStatusDto>>> GetReceiptOfTheOrderStatus(PagingDto Paging, string code)
        {
            PagingResualt<IEnumerable<ReceiptOfTheOrderStatus>> response;
            var includes = new string[] { nameof(ReceiptOfTheOrderStatus.Recvier) };
            if (string.IsNullOrEmpty(code))
            {

                response = await _receiptOfTheOrderStatusRepository.GetAsync(Paging, includes, orderBy: c => c.OrderByDescending(r => r.Id));
            }
            else
            {
                response = await _receiptOfTheOrderStatusRepository.GetAsync(Paging, c => c.ReceiptOfTheOrderStatusDetalis.Any(c => c.OrderCode == code), includes, orderBy: c => c.OrderByDescending(r => r.Id));

            }

            var ids = response.Data.Select(c => c.Id);

            var types = (await _receiptOfTheOrderStatusDetalisRepository.Select(c => ids.Contains(c.ReceiptOfTheOrderStatusId), c => new { c.Id, c.ReceiptOfTheOrderStatusId, c.OrderPlace })).GroupBy(c => c.ReceiptOfTheOrderStatusId).ToDictionary(c => c.Key, c => c.ToList());

            var dtos = _mapper.Map<List<ReceiptOfTheOrderStatusDto>>(response.Data);
            dtos.ForEach((Action<ReceiptOfTheOrderStatusDto>)(c =>
            {
                if (types.ContainsKey(c.Id))
                {
                    var orderPlaced = Enumerable.ToHashSet<string>(Enumerable.SelectMany(types.Where(t => t.Key == c.Id), c => (IEnumerable<string>)Enumerable.Select(c.Value, c => EnumExtension.GetDescription<int>((int)c.OrderPlace))));
                    c.Types = string.Join(',', orderPlaced);
                }
            }));
            return new PagingResualt<IEnumerable<ReceiptOfTheOrderStatusDto>>()
            {
                Total = response.Total,
                Data = dtos
            };
        }
        ExpressionStarter<Order> GetFilterAsLinq(SelectedOrdersWithFitlerDto selectedOrdersWithFitlerDto)
        {
            var predicate = PredicateBuilder.New<Order>(true);
            if (selectedOrdersWithFitlerDto.SelectedIds?.Any() == true)
            {
                predicate = predicate.And(c => selectedOrdersWithFitlerDto.SelectedIds.Contains(c.Id));
                return predicate;
            }
            if (selectedOrdersWithFitlerDto.OrderFilter != null)
                predicate = GetFilterAsLinq(selectedOrdersWithFitlerDto.OrderFilter);
            if (selectedOrdersWithFitlerDto.IsSelectedAll)
            {
                if (selectedOrdersWithFitlerDto.ExceptIds?.Any() == true)
                {
                    predicate = predicate.And(c => !selectedOrdersWithFitlerDto.ExceptIds.Contains(c.Id));
                }
            }
            return predicate;
        }
        ExpressionStarter<Order> GetFilterAsLinq(OrderFilter filter)
        {
            var predicate = PredicateBuilder.New<Order>(true);
            if (filter.CountryId.HasValue)
                predicate = predicate.And(c => c.CountryId == filter.CountryId);
            if (!string.IsNullOrEmpty(filter.Code))
                predicate = predicate.And(c => c.Code == filter.Code);
            if (filter.ClientId.HasValue)
            {
                predicate = predicate.And(c => c.ClientId == filter.ClientId);
            }
            if (filter.RegionId.HasValue)
            {
                predicate = predicate.And(c => c.RegionId == filter.RegionId);
            }
            if (filter.RecipientName != string.Empty && filter.RecipientName != null)
            {
                predicate = predicate.And(c => c.RecipientName.StartsWith(filter.RecipientName));
            }
            if (filter.MoneyPalced.HasValue)
            {
                predicate = predicate.And(c => c.MoneyPlace == (MoneyPalce)filter.MoneyPalced);
            }
            if (filter.Orderplaced.HasValue)
            {
                predicate = predicate.And(c => c.OrderPlace == filter.Orderplaced);
            }
            if (filter.Phone != string.Empty && filter.Phone != null)
            {
                predicate = predicate.And(c => c.RecipientPhones.Contains(filter.Phone));
            }
            if (filter.AgentId.HasValue)
            {
                predicate = predicate.And(c => c.AgentId == filter.AgentId);
            }
            if (filter.IsClientDiliverdMoney.HasValue)
            {
                predicate = predicate.And(c => c.IsClientDiliverdMoney == filter.IsClientDiliverdMoney);
            }
            if (filter.ClientPrintNumber.HasValue)
            {
                predicate = predicate.And(c => c.OrderClientPaymnets.Any(op => op.ClientPayment.Id == filter.ClientPrintNumber));
            }
            if (filter.AgentPrintNumber.HasValue)
            {
                predicate = predicate.And(c => c.AgentOrderPrints.Any(c => c.AgentPrint.Id == filter.AgentPrintNumber));
            }
            if (filter.CreatedDate.HasValue)
            {
                var date = filter.CreatedDate.Value.Date;
                var nextDate = date.AddDays(1);
                predicate = predicate.And(c => c.Date.Value.Date >= date&&c.Date.Value<nextDate);
            }
            if (filter.Note != "" && filter.Note != null)
            {
                predicate = predicate.And(c => c.Note.Contains(filter.Note));
            }
            if (filter.AgentPrintStartDate.HasValue)
            {
                ///TODO :
                ///chould check this query 
                predicate = predicate.And(c => c.AgentOrderPrints.Select(c => c.AgentPrint).OrderBy(c => c.Id).LastOrDefault().Date >= filter.AgentPrintStartDate);

            }
            if (filter.AgentPrintEndDate.HasValue)
            {
                ///TODO :
                ///chould check this query 
                predicate = predicate.And(c => c.AgentOrderPrints.Select(c => c.AgentPrint).OrderBy(c => c.Id).LastOrDefault().Date <= filter.AgentPrintEndDate);
            }
            if (!string.IsNullOrEmpty(filter.CreatedBy?.Trim()))
            {
                predicate = predicate.And(c => c.CreatedBy == filter.CreatedBy);
            }
            if (filter.CreatedDateRangeFilter != null)
            {
                if (filter.CreatedDateRangeFilter.Start.HasValue)
                    predicate = predicate.And(c => c.Date >= filter.CreatedDateRangeFilter.Start);
                if (filter.CreatedDateRangeFilter.End.HasValue)
                    predicate = predicate.And(c => c.Date <= filter.CreatedDateRangeFilter.End);
            }
            if (filter.HaveScoundBranch.HasValue)
            {
                if (filter.HaveScoundBranch == true)
                    predicate = predicate.And(c => c.TargetBranchId != null);
                else
                    predicate = predicate.And(c => c.TargetBranchId == null);
            }
            if (filter.OrderState.HasValue)
            {
                predicate = predicate.And(c => c.OrderState == filter.OrderState);
            }
            if (filter.OriginalBranchId.HasValue)
            {
                predicate = predicate.And(c => c.BranchId == filter.OriginalBranchId);
            }
            if (filter.SecoundBranchId.HasValue)
            {
                predicate = predicate.And(c => c.TargetBranchId == filter.SecoundBranchId);
            }
            if (filter.CurrentBranchId.HasValue)
            {
                predicate = predicate.And(c => c.CurrentBranchId == filter.CurrentBranchId);
            }
            if (filter.NextBranchId.HasValue)
            {
                predicate = predicate.And(c => c.NextBranchId == filter.NextBranchId);
            }
            return predicate;
        }
        public async Task<PagingResualt<IEnumerable<OrderDto>>> GetOrderFiltered(PagingDto pagingDto, OrderFilter orderFilter)
        {
            var includes = new string[] { nameof(Order.Client), nameof(Order.Agent), nameof(Order.Region), nameof(Order.Country), $"{nameof(Order.OrderClientPaymnets)}.{nameof(OrderClientPaymnet.ClientPayment)}", $"{nameof(Order.AgentOrderPrints)}.{nameof(AgentOrderPrint.AgentPrint)}", nameof(Order.Branch), nameof(Order.CurrentBranch), nameof(Order.NextBranch) };
            var pagingResult = await _repository.GetAsync(pagingDto, GetFilterAsLinq(orderFilter), includes, null);
            return new PagingResualt<IEnumerable<OrderDto>>()
            {
                Data = _mapper.Map<OrderDto[]>(pagingResult.Data),
                Total = pagingResult.Total
            };
        }
        public async Task CreateOrders(IEnumerable<CreateMultipleOrder> createMultipleOrders)
        {
            var countriesIds = createMultipleOrders.Select(c => c.CountryId).ToArray();
            var countries = await _countryCashedService.GetAsync(c => countriesIds.Contains(c.Id), c => c.BranchToCountryDeliverryCosts);
            var branches = await _branchRepository.GetAll();

            var currentBrach = branches.First(c => c.Id == _currentBranchId);
            var agnets = await _uintOfWork.Repository<User>().Select(c => createMultipleOrders.Select(c => c.AgentId).Contains(c.Id), c => new { c.Id, c.Salary });
            await _uintOfWork.BegeinTransaction();
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
            try
            {

                var midCountrires = (await _mediatorCountry.GetAsync(c => c.FromCountryId == currentBrach.Id && countriesIds.Contains(c.ToCountryId))).ToList();
                var orders = createMultipleOrders.Select(item =>
                 {
                     var order = _mapper.Map<Order>(item);
                     if (order.AgentId != null)
                     {
                         order.AgentCost = agnets.FirstOrDefault(c => c.Id == order.AgentId)?.Salary ?? 0;
                     }
                     order.Date = item.Date;
                     order.CreatedBy = currentUser;
                     order.CurrentBranchId = _currentBranchId;
                     var targetBranch = branches.FirstOrDefault(c => c.Id == order.CountryId && c.Id != _currentBranchId);
                     if (targetBranch != null)
                     {
                         order.TargetBranchId = targetBranch.Id;
                         order.NextBranchId = targetBranch.Id;
                         if (order.AgentId != null)
                             throw new ConflictException("لا يمكن اختيار مندوب إذا كان الطلب موجه إلى فرع آخر");
                         var midCountry = midCountrires.FirstOrDefault(c => c.FromCountryId == currentBrach.Id && c.ToCountryId == targetBranch.Id);
                         if (midCountry != null)
                         {
                             order.NextBranchId = midCountry.MediatorCountryId;
                         }
                     }
                     else
                     {
                         var midCountry = midCountrires.FirstOrDefault(c => c.FromCountryId == currentBrach.Id && c.ToCountryId == order.CountryId);
                         if (midCountry != null)
                         {
                             order.TargetBranchId = midCountry.MediatorCountryId;
                             order.NextBranchId = midCountry.MediatorCountryId;
                             if (order.AgentId.HasValue)
                             {
                                 throw new ConflictException("لا يمكن اختيار مندوب إذا كان الطلب موجه إلى فرع آخر");
                             }
                         }
                         else if (!order.AgentId.HasValue)
                         {
                             throw new ConflictException("يجب اختيار المندوب");
                         }
                     }
                     return order;
                 }).ToList();
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
        public async Task CreateOrder(CreateOrderFromEmployee createOrdersFromEmployee)
        {
            var country = await _countryCashedService.GetById(createOrdersFromEmployee.CountryId);
            var currentBrach = await _branchRepository.GetById(_currentBranchId);
            var order = _mapper.Map<CreateOrderFromEmployee, Order>(createOrdersFromEmployee);
            await _uintOfWork.BegeinTransaction();
            try
            {
                order.CurrentBranchId = _currentBranchId;
                var targetBranch = await _branchRepository.FirstOrDefualt(c => c.Id == country.Id && c.Id != _currentBranchId);
                if (targetBranch != null)
                {
                    order.TargetBranchId = targetBranch.Id;
                    order.NextBranchId = targetBranch.Id;
                    if (order.AgentId.HasValue)
                    {
                        throw new ConflictException("لا يمكن اختيار مندوب إذا كان الطلب موجه إلى فرع آخر");
                    }


                    var midCountry = await _mediatorCountry.FirstOrDefualt(c => c.FromCountryId == currentBrach.Id && c.ToCountryId == targetBranch.Id);

                    if (midCountry != null)
                    {

                        order.NextBranchId = midCountry.MediatorCountryId;
                    }
                    order.AgentId = null;
                }
                else
                {
                    var midCountry = await _mediatorCountry.FirstOrDefualt(c => c.FromCountryId == currentBrach.Id && c.ToCountryId == order.CountryId);
                    if (midCountry != null)
                    {
                        order.NextBranchId = midCountry.MediatorCountryId;
                        order.TargetBranchId = midCountry.MediatorCountryId;
                        if (order.AgentId.HasValue)
                        {
                            throw new ConflictException("لا يمكن اختيار مندوب إذا كان الطلب موجه إلى فرع آخر");
                        }
                    }
                    else if (!order.AgentId.HasValue)
                    {
                        throw new ConflictException("يجب اختيار المندوب");

                    }
                    else
                    {
                        order.AgentCost = (await _uintOfWork.Repository<User>().FirstOrDefualt(c => c.Id == order.AgentId))?.Salary ?? 0;
                    }
                }



                order.CreatedBy = currentUser;
                if (await _uintOfWork.Repository<Order>().Any(c => c.Code == order.Code && c.ClientId == order.ClientId))
                {
                    throw new ConflictException($"الكود{order.Code} مكرر");
                }
                if (createOrdersFromEmployee.RegionId == null)
                {
                    if (!string.IsNullOrWhiteSpace(createOrdersFromEmployee.RegionName))
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

                }
                order.OrderState = OrderState.Processing;
                //order.DeliveryCost = country.DeliveryCost;
                if (order.MoneyPlace == MoneyPalce.Delivered)
                {
                    order.IsClientDiliverdMoney = true;

                }
                else
                {
                    order.IsClientDiliverdMoney = false;
                }
                await _uintOfWork.Add(order);

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
            var includes = new string[] { $"{nameof(Order.Client)}.{nameof(Client.ClientPhones)}", nameof(Order.Country) };
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
                if (item.OrderPlace > OrderPlace.Way)
                {
                    item.OrderState = OrderState.Finished;
                    if (item.MoneyPlace == MoneyPalce.InsideCompany)
                    {
                        item.MoneyPlace = MoneyPalce.Delivered;
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
                    MoneyPlace = item.MoneyPlace,
                    OrderPlace = item.OrderPlace,
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
        //public async Task<IEnumerable<PayForClientDto>> GetDeleiverMoneyForClientForPrint(DeleiverMoneyForClientDto2 deleiverMoneyForClientDto2, PagingDto pagingDto)
        //{
        //    var includes = new string[] { $"{nameof(Order.Country)}.{nameof(Country.BranchToCountryDeliverryCosts)}" };
        //    var predicate = PredicateBuilder.New<Order>(c => c.ClientId == deleiverMoneyForClientDto2.Filter.ClientId && deleiverMoneyForClientDto2.Filter.OrderPlacedId.Contains(c.OrderPlace) && c.AgentId != null);
        //    if (deleiverMoneyForClientDto2.SelectedIds?.Any() == true)
        //    {
        //        predicate = predicate.And(c => deleiverMoneyForClientDto2.SelectedIds.Contains(c.Id));
        //    }
        //    else if (deleiverMoneyForClientDto2.IsSelectedAll)
        //    {
        //        if (deleiverMoneyForClientDto2.Filter.ClientDoNotDeleviredMoney && !deleiverMoneyForClientDto2.Filter.IsClientDeleviredMoney)
        //        {
        //            predicate = predicate.And(c => c.IsClientDiliverdMoney == false);
        //        }
        //        else if (!deleiverMoneyForClientDto2.Filter.ClientDoNotDeleviredMoney && deleiverMoneyForClientDto2.Filter.IsClientDeleviredMoney)
        //        {
        //            predicate = predicate.And(c => c.OrderState == OrderState.ShortageOfCash);
        //        }
        //        else if (deleiverMoneyForClientDto2.Filter.ClientDoNotDeleviredMoney && deleiverMoneyForClientDto2.Filter.IsClientDeleviredMoney)
        //        {
        //            predicate = predicate.And(c => c.OrderState == OrderState.ShortageOfCash || c.IsClientDiliverdMoney == false);
        //        }
        //        if (deleiverMoneyForClientDto2.ExceptIds?.Any() == true)
        //        {
        //            predicate = predicate.And(c => !deleiverMoneyForClientDto2.ExceptIds.Contains(c.Id));
        //        }
        //    }
        //    var orders = await _repository.GetAsync(pagingDto, predicate, includes);
        //    return _mapper.Map<IEnumerable<PayForClientDto>>(orders);

        //}
        public async Task<int> DeleiverMoneyForClient(DeleiverMoneyForClientDto2 deleiverMoneyForClientDto2)
        {
            var includes = new string[] { $"{nameof(Order.Country)}.{nameof(Country.BranchToCountryDeliverryCosts)}" };
            var predicate = PredicateBuilder.New<Order>(c => c.ClientId == deleiverMoneyForClientDto2.Filter.ClientId && deleiverMoneyForClientDto2.Filter.OrderPlacedId.Contains(c.OrderPlace) && c.AgentId != null);
            if (deleiverMoneyForClientDto2.Filter.TableSelection.SelectedIds?.Any() == true)
            {
                predicate = predicate.And(c => deleiverMoneyForClientDto2.Filter.TableSelection.SelectedIds.Contains(c.Id));
            }
            else if (deleiverMoneyForClientDto2.Filter.TableSelection.IsSelectedAll)
            {
                if (deleiverMoneyForClientDto2.Filter.ClientDoNotDeleviredMoney && !deleiverMoneyForClientDto2.Filter.IsClientDeleviredMoney)
                {
                    predicate = predicate.And(c => c.IsClientDiliverdMoney == false);
                }
                else if (!deleiverMoneyForClientDto2.Filter.ClientDoNotDeleviredMoney && deleiverMoneyForClientDto2.Filter.IsClientDeleviredMoney)
                {
                    predicate = predicate.And(c => c.OrderState == OrderState.ShortageOfCash);
                }
                else if (deleiverMoneyForClientDto2.Filter.ClientDoNotDeleviredMoney && deleiverMoneyForClientDto2.Filter.IsClientDeleviredMoney)
                {
                    predicate = predicate.And(c => c.OrderState == OrderState.ShortageOfCash || c.IsClientDiliverdMoney == false);
                }
                if (deleiverMoneyForClientDto2.Filter.TableSelection.ExceptIds?.Any() == true)
                {
                    predicate = predicate.And(c => !deleiverMoneyForClientDto2.Filter.TableSelection.ExceptIds.Contains(c.Id));
                }
            }
            var orders = await _repository.GetByFilterInclue(predicate, includes);
            if (!orders.Any())
            {
                throw new Exception("No order selected");
            }
            var clientId = orders.First().ClientId;
            var client = await _uintOfWork.Repository<Client>().FirstOrDefualt(c => c.Id == clientId, new string[] { nameof(Client.ClientPhones) });
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

            if (!orders.All(c => c.OrderPlace == OrderPlace.CompletelyReturned || c.OrderPlace == OrderPlace.Unacceptable))
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
                var points = item.Country.BranchToCountryDeliverryCosts.First(c => c.BranchId == _currentBranchId).Points;
                if (!item.IsClientDiliverdMoney)
                {
                    if (!(item.OrderPlace == OrderPlace.CompletelyReturned || item.OrderPlace == OrderPlace.Delayed))
                    {

                        totalPoints += points;
                    }
                }
                else
                {
                    if ((item.OrderPlace == OrderPlace.CompletelyReturned || item.OrderPlace == OrderPlace.Delayed))
                    {
                        totalPoints -= points;
                    }
                }

                if (item.OrderPlace > OrderPlace.Way)
                {
                    item.OrderState = OrderState.Finished;
                    if (item.MoneyPlace == MoneyPalce.InsideCompany)
                    {
                        item.MoneyPlace = MoneyPalce.Delivered;
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
                    MoneyPlace = item.MoneyPlace,
                    OrderPlace = item.OrderPlace,
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
            if (deleiverMoneyForClientDto2.PointsSettingId != null)
            {

                var pointSetting = await _uintOfWork.Repository<PointsSetting>().FirstOrDefualt(c => c.Id == deleiverMoneyForClientDto2.PointsSettingId);
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
            //semaphore.Release();
            return clientPayment.Id;
        }
        public async Task<int> DeleiverMoneyForClient(DeleiverMoneyForClientDto deleiverMoneyForClientDto)
        {
            var includes = new string[] { $"{nameof(Order.Client)}.{nameof(Client.ClientPhones)}", $"{nameof(Order.Country)}.{nameof(Country.BranchToCountryDeliverryCosts)}" };
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
            if (!orders.All(c => c.OrderPlace == OrderPlace.CompletelyReturned || c.OrderPlace == OrderPlace.Unacceptable))
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
                var points = item.Country.BranchToCountryDeliverryCosts.First(c => c.BranchId == _currentBranchId).Points;
                if (!item.IsClientDiliverdMoney)
                {
                    if (!(item.OrderPlace == OrderPlace.CompletelyReturned || item.OrderPlace == OrderPlace.Delayed))
                    {

                        totalPoints += points;
                    }
                }
                else
                {
                    if ((item.OrderPlace == OrderPlace.CompletelyReturned || item.OrderPlace == OrderPlace.Delayed))
                    {
                        totalPoints -= points;
                    }
                }

                if (item.OrderPlace > OrderPlace.Way)
                {
                    item.OrderState = OrderState.Finished;
                    if (item.MoneyPlace == MoneyPalce.InsideCompany)
                    {
                        item.MoneyPlace = MoneyPalce.Delivered;
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
                    MoneyPlace = item.MoneyPlace,
                    OrderPlace = item.OrderPlace,
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
                orders = await _repository.GetAsync(c => c.Date <= date && c.AgentId == frozenOrder.AgentId && c.OrderPlace == OrderPlace.Way, c => c.Client, c => c.Region, c => c.Agent, c => c.Country);
            }
            else
            {
                orders = await _repository.GetAsync(c => c.Date <= date && c.OrderPlace == OrderPlace.Way, c => c.Client, c => c.Region, c => c.Agent, c => c.Country);
            }
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> GetById(int id)
        {
            var order = await _repository.GetByIdIncludeAllForEmployee(id);
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<PagingResualt<IEnumerable<PayForClientDto>>> OrdersDontFinished(OrderDontFinishedFilter orderDontFinishedFilter, PagingDto pagingDto)
        {
            var result = await _repository.OrdersDontFinished(orderDontFinishedFilter, pagingDto);
            return new PagingResualt<IEnumerable<PayForClientDto>>()
            {
                Total = result.Total,
                Data = _mapper.Map<IEnumerable<PayForClientDto>>(result.Data)
            };
        }

        public async Task<IEnumerable<OrderDto>> NewOrderDontSned()
        {
            var includes = new string[] { $"{nameof(Order.Client)}.{nameof(Client.ClientPhones)}", $"{nameof(Order.Client)}.{nameof(Client.Country)}", nameof(Order.Region), $"{nameof(Order.Country)}.{nameof(Country.AgentCountries)}.{nameof(AgentCountry.Agent)}", $"{nameof(Order.OrderItems)}.{nameof(OrderItem.OrderTpye)}" };
            var orders = await _repository.GetByFilterInclue(c => c.IsSend == false && c.OrderPlace == OrderPlace.Client, includes);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<IEnumerable<OrderDto>> OrderAtClient(OrderFilter orderFilter)
        {
            var orders = await _repository.OrderAtClient(orderFilter);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<PayForClientDto> GetByCodeAndClient(int clientId, string code)
        {
            var includes = new string[] { $"{nameof(Order.OrderClientPaymnets)}.{nameof(OrderClientPaymnet.ClientPayment)}", $"{nameof(Order.AgentOrderPrints)}.{nameof(AgentOrderPrint.AgentPrint)}" };
            var order = await _repository.FirstOrDefualt(c => c.ClientId == clientId && c.Code == code, includes);
            if (order == null)
                throw new ConflictException("الشحنة غير موجودة");
            if (order.IsClientDiliverdMoney && order.OrderState != OrderState.ShortageOfCash)
            {
                throw new ConflictException("تم تسليم كلفة الشحنة من قبل");
            }
            if (order.OrderPlace == OrderPlace.Client)
            {
                throw new ConflictException("الشحنة عند العميل");
            }
            if (order.OrderPlace == OrderPlace.Store)
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
            orders.ToList().ForEach(c => c.OrderState = OrderState.Finished);
            await _repository.Update(orders);
        }
        public async Task<EarningsDto> GetEarnings(PagingDto pagingDto, DateFiter dateFiter)
        {
            var predicate = PredicateBuilder.New<Order>(true);
            predicate = predicate.And(c => c.OrderState == OrderState.Finished && c.OrderPlace != OrderPlace.CompletelyReturned);
            if (dateFiter.FromDate.HasValue)
                predicate = predicate.And(c => c.Date >= dateFiter.FromDate);
            if (dateFiter.ToDate.HasValue)
                predicate = predicate.And(c => c.Date <= dateFiter.ToDate);
            var pagingResualt = await _repository.GetAsync(pagingDto, predicate);
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

        public async Task Accept(OrderIdAndAgentId idsDto)
        {
            var order = await _repository.GetById(idsDto.OrderId);
            if (!await _agentCountryRepository.Any(c => c.CountryId == order.CountryId && c.AgentId == idsDto.AgentId))
            {
                throw new ConflictException("تضارب المندوب و المدينة");
            }
            var country = await _countryCashedService.GetById(order.CountryId);
            var currentBrach = await _branchRepository.GetById(_currentBranchId);
            var targetBranch = await _branchRepository.FirstOrDefualt(c => c.Id == country.Id && c.Id != _currentBranchId);
            if (targetBranch != null)
            {
                order.TargetBranchId = targetBranch.Id;
                order.NextBranchId = targetBranch.Id;
                if (order.AgentId.HasValue)
                {
                    throw new ConflictException("لا يمكن اختيار مندوب إذا كان الطلب موجه إلى فرع آخر");
                }


                var midCountry = await _mediatorCountry.FirstOrDefualt(c => c.FromCountryId == currentBrach.Id && c.ToCountryId == targetBranch.Id);

                if (midCountry != null)
                {

                    order.NextBranchId = midCountry.MediatorCountryId;
                }
                order.AgentId = null;
            }
            else
            {
                var midCountry = await _mediatorCountry.FirstOrDefualt(c => c.FromCountryId == currentBrach.Id && c.ToCountryId == order.CountryId);
                if (midCountry != null)
                {
                    order.NextBranchId = midCountry.MediatorCountryId;
                    order.TargetBranchId = midCountry.MediatorCountryId;
                    if (order.AgentId.HasValue)
                    {
                        throw new ConflictException("لا يمكن اختيار مندوب إذا كان الطلب موجه إلى فرع آخر");
                    }
                }
                else if (!idsDto.AgentId.HasValue)
                {
                    throw new ConflictException("يجب اختيار المندوب");
                }
                else
                {
                    order.AgentId = idsDto.AgentId.Value;
                    order.AgentCost = (await _uintOfWork.Repository<User>().FirstOrDefualt(c => c.Id == order.AgentId))?.Salary ?? 0;
                }
            }
            order.OrderPlace = OrderPlace.Store;
            order.IsSend = true;
            await _repository.Update(order);
        }

        public async Task AcceptMultiple(IEnumerable<OrderIdAndAgentId> idsDtos)
        {

            var orders = await _repository.GetAsync(c => idsDtos.Select(dto => dto.OrderId).Contains(c.Id));

            var countriesIds = orders.Select(c => c.CountryId).ToArray();
            var countries = await _countryCashedService.GetAsync(c => countriesIds.Contains(c.Id), c => c.BranchToCountryDeliverryCosts);
            var branches = await _branchRepository.GetAll();

            var currentBrach = branches.First(c => c.Id == _currentBranchId);
            var agentIds = idsDtos.Where(c => c.AgentId.HasValue).Select(c=>c.AgentId).ToList();
            
            var agnets = await _uintOfWork.Repository<User>().Select(c => agentIds.Contains(c.Id), c => new { c.Id, c.Salary });

            var midCountrires = (await _mediatorCountry.GetAsync(c => c.FromCountryId == currentBrach.Id && countriesIds.Contains(c.ToCountryId))).ToList();
            var agentsIds = orders.Select(c => c.AgentId);
            var agents = await _userRepository.GetAsync(c => agentsIds.Contains(c.Id));

            foreach (var item in idsDtos)
            {
                var order = orders.First(c => c.Id == item.OrderId);
                if (item.AgentId.HasValue)
                {
                    order.AgentId = item.AgentId;
                    order.AgentCost = agnets.FirstOrDefault(c => c.Id == order.AgentId)?.Salary ?? 0;
                }

                var targetBranch = branches.FirstOrDefault(c => c.Id == order.CountryId && c.Id != _currentBranchId);
                if (targetBranch != null)
                {
                    order.TargetBranchId = targetBranch.Id;
                    order.NextBranchId = targetBranch.Id;
                    if (order.AgentId != null)
                        throw new ConflictException("لا يمكن اختيار مندوب إذا كان الطلب موجه إلى فرع آخر");
                    var midCountry = midCountrires.FirstOrDefault(c => c.FromCountryId == currentBrach.Id && c.ToCountryId == targetBranch.Id);
                    if (midCountry != null)
                    {
                        order.NextBranchId = midCountry.MediatorCountryId;
                    }
                }
                else
                {
                    var midCountry = midCountrires.FirstOrDefault(c => c.FromCountryId == currentBrach.Id && c.ToCountryId == order.CountryId);
                    if (midCountry != null)
                    {
                        order.TargetBranchId = midCountry.MediatorCountryId;
                        order.NextBranchId = midCountry.MediatorCountryId;
                        if (order.AgentId.HasValue)
                        {
                            throw new ConflictException("لا يمكن اختيار مندوب إذا كان الطلب موجه إلى فرع آخر");
                        }
                    }
                    else if (!order.AgentId.HasValue)
                    {
                        throw new ConflictException("يجب اختيار المندوب");
                    }
                }
                order.OrderPlace = OrderPlace.Store;
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
            var order = await _uintOfWork.Repository<Order>().FirstOrDefualt(c => c.Id == orderReSend.Id);
            if (order.CurrentBranchId != _currentBranchId)
            {
                throw new ConflictException("الشحنة ليست في مخزنك");
            }
            await _uintOfWork.BegeinTransaction();
            OrderLog log = order;
            await _uintOfWork.Add(log);
            order.CountryId = orderReSend.CountryId;
            order.RegionId = orderReSend.RegionId;
            order.AgentId = orderReSend.AgnetId;
            if (order.OldCost != null)
            {
                order.Cost = (decimal)order.OldCost;
                order.OldCost = null;
            }
            //order.IsClientDiliverdMoney = false;

            order.OrderState = OrderState.Processing;
            order.OrderPlace = OrderPlace.Store;
            order.DeliveryCost = orderReSend.DeliveryCost;
            order.MoneyPlace = MoneyPalce.OutSideCompany;
            order.AgentCost = (await _userRepository.FirstOrDefualt(c => c.Id == order.AgentId)).Salary ?? 0;
            order.SystemNote = "إعادة الإرسال";
            order.UpdatedBy = _userService.AuthoticateUserName();
            order.UpdatedDate = DateTime.UtcNow;
            await _repository.Update(order);
            await _uintOfWork.Commit();
        }
        public async Task<OrderDto> MakeOrderCompletelyReturned(int id)
        {

            var order = await _uintOfWork.Repository<Order>().FirstOrDefualt(c => c.Id == id);
            OrderLog log = order;
            await _uintOfWork.BegeinTransaction();
            if (order.OrderPlace != OrderPlace.Store)
            {
                throw new ConflictException("الشحنة ليست في المخزن");
            }
            order.OrderPlace = OrderPlace.CompletelyReturned;
            order.MoneyPlace = MoneyPalce.InsideCompany;
            order.OldCost = order.Cost;
            order.Cost = 0;
            if (order.OldDeliveryCost == null)
                order.OldDeliveryCost = order.DeliveryCost;
            order.DeliveryCost = 0;
            order.AgentCost = 0;
            order.UpdatedDate = DateTime.UtcNow;
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
            var orderInStor = orders.Where(c => c.OrderPlace == OrderPlace.Store).ToList();
            orders = orders.Except(orderInStor).ToList();

            var fOrder = orders.Where(c => c.OrderPlace == OrderPlace.CompletelyReturned || c.OrderPlace == OrderPlace.Unacceptable || (c.OrderPlace == OrderPlace.Delivered && (c.MoneyPlace == MoneyPalce.InsideCompany || c.MoneyPlace == MoneyPalce.Delivered))).ToList();
            orders = orders.Except(fOrder).ToList();
            var orderInCompany = orders.Where(c => c.MoneyPlace == MoneyPalce.InsideCompany).ToList();
            orders = orders.Except(orderInCompany).ToList();
            if (orders.Count() == 0)
            {
                if (lastOrderAdded.OrderPlace == OrderPlace.Store)
                {
                    throw new ConflictException("الشحنة ما زالت في المخزن");
                }
                if (lastOrderAdded.MoneyPlace == MoneyPalce.InsideCompany)
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
            await _uintOfWork.BegeinTransaction();
            var agnet = await _uintOfWork.Repository<User>().GetById(transferOrderToAnotherAgnetDto.NewAgentId);
            var orders = await _uintOfWork.Repository<Order>().GetAsync(c => transferOrderToAnotherAgnetDto.Ids.Contains(c.Id));
            var logs = new List<OrderLog>();
            var user = _userService.AuthoticateUserName();
            var utc = DateTime.UtcNow;
            orders.ForEach(c =>
            {
                logs.Add(c);
                c.SystemNote = "نقل الطلبات إلى مندوب آخر";
                c.UpdatedBy = user;
                c.UpdatedDate = utc;
                c.AgentId = agnet.Id;
                c.AgentCost = agnet.Salary ?? 0;
            });
            await _uintOfWork.AddRange(logs);
            await _uintOfWork.UpdateRange(orders);
            await _uintOfWork.Commit();
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
                order.OrderState = OrderState.Processing;
                order.MoneyPlace = MoneyPalce.OutSideCompany;
                order.OrderPlace = OrderPlace.Store;
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
            var includes = new string[] { nameof(ClientPayment.Discounts), nameof(ClientPayment.Receipts), $"{nameof(ClientPayment.ClientPaymentDetails)}" };
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
                exprtion = exprtion.And(c => c.DestinationName.Contains(clientName));
            }
            if (!string.IsNullOrEmpty(code))
            {
                exprtion = exprtion.And(c => c.ClientPaymentDetails.Any(c => c.Code.StartsWith(code)));
            }
            var includes = new string[] { $"{nameof(ClientPayment.ClientPaymentDetails)}" };
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
            var orders = await _repository.GetAsync(c => c.AgentRequestStatus == (int)AgentRequestStatusEnum.Pending, c => c.Agent);
            return _mapper.Map<ApproveAgentEditOrderRequestDto[]>(orders);
        }

        public async Task DisAproveOrderRequestEditState(int[] ids)
        {
            var orders = await _repository.GetAsync(c => ids.Contains(c.Id));
            orders.ForEach(c =>
            {
                c.AgentRequestStatus = (int)AgentRequestStatusEnum.DisApprove;
                c.NewOrderPlace = null;
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
                order.OrderPlace = order.NewOrderPlace.Value;
                order.AgentRequestStatus = (int)AgentRequestStatusEnum.Approve;

                order.SystemNote = "OrderRequestEditStateCount";
                if (order.IsClientDiliverdMoney)
                {
                    switch (order.OrderPlace)
                    {
                        case OrderPlace.Delivered:
                            {
                                if (Decimal.Compare(order.Cost, order.NewCost.Value) != 0)
                                {
                                    order.OldCost ??= order.Cost;
                                    order.Cost = order.NewCost.Value;
                                }
                                var payForClient = order.ShouldToPay();


                                if (Decimal.Compare(payForClient, (order.ClientPaied ?? 0)) != 0)
                                {
                                    order.OrderState = OrderState.ShortageOfCash;
                                    if (order.MoneyPlace == MoneyPalce.Delivered)
                                    {
                                        order.MoneyPlace = MoneyPalce.InsideCompany;
                                    }
                                }
                                else
                                {
                                    order.OrderState = OrderState.Finished;
                                }

                            }
                            break;
                        case OrderPlace.CompletelyReturned:
                            {
                                if (order.OldCost == null)
                                    order.OldCost = order.Cost;
                                order.Cost = 0;
                                order.AgentCost = 0;
                                order.OrderState = OrderState.ShortageOfCash;
                            }
                            break;
                        case OrderPlace.Unacceptable:
                            {
                                if (order.OldCost == null)
                                {
                                    order.OldCost = order.Cost;
                                }
                                order.OrderState = OrderState.ShortageOfCash;
                            }
                            break;
                        case OrderPlace.PartialReturned:
                            {
                                if (order.OldCost == null)
                                    order.OldCost = order.Cost;
                                order.Cost = order.NewCost.Value;
                                order.OrderState = OrderState.ShortageOfCash;
                            }
                            break;
                    }
                }
                else
                {
                    switch (order.OrderPlace)
                    {
                        case OrderPlace.PartialReturned:
                        case OrderPlace.Delivered:
                            {
                                if (order.Cost != order.NewCost.Value)
                                {
                                    if (order.OldCost == null)
                                        order.OldCost = order.Cost;
                                    order.Cost = order.NewCost.Value;
                                }
                            }
                            break;
                        case OrderPlace.CompletelyReturned:
                            {
                                if (order.OldCost == null)
                                {
                                    order.OldCost = order.Cost;
                                }
                                order.Cost = 0;
                                order.OldDeliveryCost ??= order.DeliveryCost;
                                order.DeliveryCost = 0;
                                order.AgentCost = 0;
                            }
                            break;
                        case OrderPlace.Unacceptable:
                            {
                                order.OldCost ??= order.Cost;
                                order.Cost = 0;
                            }
                            break;

                    }
                }
                if (order.OrderState != OrderState.Finished && order.OrderPlace != OrderPlace.Way)
                {
                    var clientNotigaction = notficationsGroup.Where(c => c.ClientId == order.ClientId && c.OrderPlace == order.OrderPlace && c.MoneyPlace == order.MoneyPlace).FirstOrDefault();
                    if (clientNotigaction == null)
                    {
                        clientNotigaction = new Notfication()
                        {
                            ClientId = order.ClientId,
                            OrderPlace = order.NewOrderPlace.Value,
                            MoneyPlace = MoneyPalce.WithAgent,
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
                    Note = $"الطلب {order.Code} اصبح {order.GetOrderPlaced().Name} و موقع المبلغ  {order.GetMoneyPlaced().Name}",
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
            var orders = await _repository.GetAsync(c => c.CurrentBranchId == _currentBranchId && c.Code == code && c.OrderPlace != OrderPlace.Delivered && c.OrderPlace != OrderPlace.PartialReturned && c.OrderPlace != OrderPlace.Client, c => c.Client, c => c.Agent, c => c.Region, c => c.Country);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task ReSendMultiple(List<OrderReSend> orderReSends)
        {
            var ordersIds = orderReSends.Select(c => c.Id);
            var orders = await _uintOfWork.Repository<Order>().GetAsync(c => ordersIds.Contains(c.Id));
            if (orders.Any(c => c.CurrentBranchId != _currentBranchId))
            {
                throw new ConflictException("الشحنة ليست في مخزنك");
            }
            await _uintOfWork.BegeinTransaction();
            var agentIds = orderReSends.Select(c => c.AgnetId).Distinct();
            var agents = await _userRepository.Select(c => new { c.Id, c.Salary }, c => agentIds.Contains(c.Id));
            List<OrderLog> logs = new List<OrderLog>();
            foreach (var orderReSend in orderReSends)
            {
                var order = orders.Single(c => c.Id == orderReSend.Id);
                OrderLog log = order;
                logs.Add(log);
                order.CountryId = orderReSend.CountryId;
                order.RegionId = orderReSend.RegionId;
                order.AgentId = orderReSend.AgnetId;
                if (order.OldCost != null)
                {
                    order.Cost = (decimal)order.OldCost;
                    order.OldCost = null;
                }
                //order.IsClientDiliverdMoney = false;
                order.OrderState = OrderState.Processing;
                order.OrderPlace = OrderPlace.Store;
                order.DeliveryCost = orderReSend.DeliveryCost;
                order.MoneyPlace = MoneyPalce.OutSideCompany;
                order.AgentCost = agents.SingleOrDefault(c => c.Id == order.AgentId)?.Salary ?? 0;
                order.SystemNote = "إعادة الإرسال";
                order.UpdatedBy = _userService.AuthoticateUserName();
                order.UpdatedDate = DateTime.UtcNow;
            }
            await _uintOfWork.AddRange(logs);
            await _uintOfWork.UpdateRange(orders);
            await _uintOfWork.Commit();
        }
        public async Task<PagingResualt<IEnumerable<OrderDto>>> GetInStockToTransferToSecondBranch(SelectedOrdersWithFitlerDto selectedOrdersWithFitlerDto)
        {
            var predicate = GetFilterAsLinq(selectedOrdersWithFitlerDto);
            predicate = predicate.And(c => c.OrderPlace == OrderPlace.Store & c.NextBranchId != null && c.CurrentBranchId == _currentBranchId && c.InWayToBranch == false);
            var pagingResult = await _repository.GetAsync(selectedOrdersWithFitlerDto.Paging, predicate, c => c.Country, c => c.Client);
            return new PagingResualt<IEnumerable<OrderDto>>()
            {
                Total = pagingResult.Total,
                Data = _mapper.Map<IEnumerable<OrderDto>>(pagingResult.Data)
            };
        }
        public async Task ReceiveOrdersToMyBranch(ReceiveOrdersToMyBranchDto receiveOrdersToMyBranchDto)
        {
            var predicate = GetFilterAsLinq(receiveOrdersToMyBranchDto);
            predicate = predicate.And(c => c.NextBranchId == _currentBranchId && c.CurrentBranchId != _currentBranchId && c.OrderPlace == OrderPlace.Way && c.InWayToBranch && !c.IsReturnedByBranch);
            var orders = await _uintOfWork.Repository<Order>().GetAsync(predicate);
            List<OrderLog> logs = new List<OrderLog>();
            logs.AddRange(orders.Select(c => (OrderLog)c));
            var agentsIds = receiveOrdersToMyBranchDto.CustomOrderAgent.Select(c => c.AgentId);
            if (receiveOrdersToMyBranchDto.AgentId.HasValue)
                agentsIds = agentsIds.Append(receiveOrdersToMyBranchDto.AgentId.Value);
            var agents = await _userRepository.GetAsync(c => agentsIds.Contains(c.Id));
            await _uintOfWork.BegeinTransaction();
            foreach (var order in orders)
            {
                order.CurrentBranchId = _currentBranchId;
                order.InWayToBranch = false;
                order.OrderPlace = OrderPlace.Store;
                order.UpdatedBy = _httpContextAccessorService.AuthoticateUserName();
                order.SystemNote = "استقبال الشحنات لفرع";
                order.UpdatedDate = DateTime.UtcNow;
                if (order.NextBranchId != order.TargetBranchId)
                {
                    order.NextBranchId = order.TargetBranchId;
                }
                else
                {
                    var customValues = receiveOrdersToMyBranchDto.CustomOrderAgent.FirstOrDefault(c => c.OrderId == order.Id);
                    if (customValues != null)
                    {
                        order.DeliveryCost = customValues.DeliveryCost;
                        order.Cost = customValues.Cost;
                        order.AgentId = customValues.AgentId;
                        order.RegionId = customValues.RegionId;
                    }
                    else
                    {
                        order.RegionId = receiveOrdersToMyBranchDto.RegionId;
                        order.AgentId = receiveOrdersToMyBranchDto.AgentId;

                    }

                    order.AgentCost = agents.First(c => c.Id == order.AgentId).Salary.Value;
                    order.NextBranchId = null;
                }
            }
            await _uintOfWork.Update(orders);
            await _uintOfWork.AddRange(logs);
            await _uintOfWork.Commit();
        }
        public async Task ReceiveOrdersToMyBranch(IEnumerable<SetOrderAgentToMyBranchDto> receiveOrdersToMyBranchDtos)
        {
            var ids = receiveOrdersToMyBranchDtos.Select(c => c.OrderId);
            var orders = await _repository.GetAsync(c => ids.Contains(c.Id));
            var agentIds = receiveOrdersToMyBranchDtos.Select(c => c.AgentId);
            var agents = await _userRepository.GetAsync(c => agentIds.Contains(c.Id));
            receiveOrdersToMyBranchDtos.ForEach(rmb =>
            {
                var order = orders.First(c => c.Id == rmb.OrderId);
                order.CurrentBranchId = _currentBranchId;
                order.OrderPlace = OrderPlace.Store;
                if (order.NextBranchId == order.TargetBranchId)
                {
                    order.DeliveryCost = rmb.DeliveryCost;
                    order.Cost = rmb.Cost;
                    order.RegionId = rmb.RegionId;
                    order.AgentId = rmb.AgentId;
                    order.AgentCost = agents.First(c => c.Id == rmb.AgentId).Salary.Value;
                    order.NextBranchId = null;
                }
                else
                {
                    order.NextBranchId = order.TargetBranchId;
                }
                order.InWayToBranch = false;
            });
            await _repository.Update(orders);
        }

        public async Task<PagingResualt<IEnumerable<OrderDto>>> GetOrdersReturnedToSecondBranch(SelectedOrdersWithFitlerDto selectedOrdersWithFitlerDto)
        {
            var predicate = this.GetFilterAsLinq(selectedOrdersWithFitlerDto);
            predicate = predicate.And(c => c.CurrentBranchId == _currentBranchId && (c.OrderPlace == OrderPlace.CompletelyReturned || c.OrderPlace == OrderPlace.PartialReturned || c.OrderPlace == OrderPlace.Unacceptable) && c.MoneyPlace == MoneyPalce.InsideCompany && c.InWayToBranch == false);
            var data = await _repository.GetAsync(selectedOrdersWithFitlerDto.Paging, predicate, c => c.Client, c => c.Country);
            return new PagingResualt<IEnumerable<OrderDto>>()
            {
                Total = data.Total,
                Data = _mapper.Map<IEnumerable<OrderDto>>(data.Data)
            };
        }

        public async Task<int> SendOrdersReturnedToSecondBranch(ReturnOrderToMainBranchDto returnOrderToMainBranchDto)
        {
            var predicate = GetFilterAsLinq(returnOrderToMainBranchDto);
            predicate = predicate.And(c => c.CurrentBranchId == _currentBranchId && (c.OrderPlace == OrderPlace.CompletelyReturned || c.OrderPlace == OrderPlace.PartialReturned || c.OrderPlace == OrderPlace.Unacceptable) && c.MoneyPlace == MoneyPalce.InsideCompany && c.InWayToBranch == false);
            var orders = await _repository.GetAsync(predicate, c => c.Country, c => c.Client);
            if (orders.Any(c => c.CurrentBranchId != _currentBranchId))
                throw new ConflictException("الشحنة ليست في فرعك");
            if (orders.Any(c => !c.IsOrderReturn()))
                throw new ConflictException("الشحنة ليست مرتجعة");
            if (orders.Any(c => !c.IsOrderInMyStore()))
                throw new ConflictException("الشحنة ليست في المخزن");
            var mainBranchId = orders.First().BranchId;
            if (orders.Any(c => c.BranchId != mainBranchId))
            {
                throw new ConflictException("ليست جميع الشحنات مرتجعة إلى نفس الفرع");
            }
            orders.ForEach(c =>
            {
                c.InWayToBranch = true;
            });
            var report = new SendOrdersReturnedToMainBranchReport()
            {
                BranchId = _currentBranchId,
                PrinterName = _httpContextAccessorService.AuthoticateUserName(),
                MainBranchId = mainBranchId,
                CreatedOnUtc = DateTime.UtcNow,
                CreatedBy = _httpContextAccessorService.AuthoticateUserName(),
            };
            if (returnOrderToMainBranchDto.Driver.DriverId.HasValue)
            {
                report.DriverId = returnOrderToMainBranchDto.Driver.DriverId.Value;
            }
            else
            {
                var driver = await _driverRepo.FirstOrDefualt(c => c.Name == returnOrderToMainBranchDto.Driver.DriverName);
                driver ??= new Driver()
                {
                    Name = returnOrderToMainBranchDto.Driver.DriverName
                };
                report.Driver = driver;
            }
            orders.ForEach(c =>
            {
                report.SendOrdersReturnedToMainBranchDetalis.Add(new SendOrdersReturnedToMainBranchDetali()
                {
                    OrderCode = c.Code,
                    ClientName = c.Client.Name,
                    CountryName = c.Country.Name,
                    DeliveryCost = c.DeliveryCost,
                    Note = c.Note
                });
            });
            await _uintOfWork.BegeinTransaction();
            await _uintOfWork.UpdateRange(orders);
            await _uintOfWork.Add(report);
            await _uintOfWork.Commit();
            return report.Id;
        }

        public async Task<string> GetSendOrdersReturnedToSecondBranchReportAsString(int id)
        {
            var path = _environment.WebRootPath + "/HtmlTemplate/SendOrdersReturnedToSecondBranchReport.html";
            var readTextTask = File.ReadAllTextAsync(path);

            var report = await _sendOrdersReturnedToMainBranchReportRepository.FirstOrDefualt(c => c.Id == id, c => c.Driver, c => c.SendOrdersReturnedToMainBranchDetalis, c => c.MainBranch, c => c.Branch);
            var readText = await readTextTask;

            readText = readText.Replace("{{printNumber}}", report.Id.ToString());
            readText = readText.Replace("{{userName}}", report.PrinterName);
            readText = readText.Replace("{{dateOfPrint}}", report.CreatedOnUtc.ToString("yyyy-MM-dd"));
            readText = readText.Replace("{{timeOfPrint}}", report.CreatedOnUtc.ToString("HH:mm"));
            readText = readText.Replace("{{fromBranch}}", report.Branch.Name);
            readText = readText.Replace("{{toBranch}}", report.MainBranch.Name);
            readText = readText.Replace("{{DriverName}}", report.Driver.Name);
            var c = 1;
            var rows = new StringBuilder();
            foreach (var item in report.SendOrdersReturnedToMainBranchDetalis)
            {

                rows.Append(@"<tr style=""border: 1px black solid;padding: 5px;text-align: center;margin-bottom: 20%;overflow: auto;"">");
                rows.Append(@"<td style=""width: 3%;border: 1px black solid;padding: 5px;text-align: center;"">");
                rows.Append(c.ToString());
                rows.Append("</td>");
                rows.Append(@"<td style=""width: 5%;border: 1px black solid;padding: 5px;text-align: center;"">");
                rows.Append(item.OrderCode);
                rows.Append("</td>");
                rows.Append(@"<td style=""width: 15%;border: 1px black solid;padding: 5px;text-align: center;"">");
                rows.Append(item.CountryName);
                rows.Append("</td>");
                rows.Append(@"<td style=""width: 15%;border: 1px black solid;padding: 5px;text-align: center;"">");
                rows.Append(item.ClientName);
                rows.Append("</td>");
                rows.Append(@"<td style=""width: 10%;border: 1px black solid;padding: 5px;text-align: center;"">");
                rows.Append(item.DeliveryCost);
                rows.Append("</td>");
                rows.Append(@"<td style=""width: 12%;border: 1px black solid;padding: 5px;text-align: center;"">");
                rows.Append(item.Note);
                rows.Append("</td>");
                rows.Append("</tr>");
                c++;
            }
            readText = readText.Replace("{orders}", rows.ToString());
            return readText;
        }
        public async Task<PagingResualt<IEnumerable<OrderDto>>> GetOrdersReturnedToMyBranch(PagingDto pagingDto, int currentBranchId)
        {
            var predicate = PredicateBuilder.New<Order>(c => c.BranchId == _currentBranchId && c.CurrentBranchId == currentBranchId && c.InWayToBranch);
            var pagingResualt = await _repository.GetAsync(pagingDto, predicate, c => c.Client, c => c.Country, c => c.Region, c => c.Agent);
            return new PagingResualt<IEnumerable<OrderDto>>()
            {
                Total = pagingResualt.Total,
                Data = _mapper.Map<IEnumerable<OrderDto>>(pagingResualt.Data)
            };
        }
        public async Task ReceiveReturnedToMyBranch(SelectedOrdersWithFitlerDto selectedOrdersWithFitlerDto)
        {

            var predicate = GetFilterAsLinq(selectedOrdersWithFitlerDto);
            predicate = predicate.And(c => c.BranchId == _currentBranchId && c.CurrentBranchId != _currentBranchId && c.InWayToBranch);
            var orders = await _repository.GetAsync(predicate);
            orders.ForEach(c =>
            {
                c.CurrentBranchId = _currentBranchId;
                c.InWayToBranch = false;
            });
            await _repository.Update(orders);
        }
        public async Task DisApproveReturnedToMyBranch(OrderIdAndNote orderIdAndNote)
        {
            var order = await _repository.GetById(orderIdAndNote.OrderId);
            order.InWayToBranch = false;
            order.Note = orderIdAndNote.Note;
            await _repository.Update(order);
        }
        public async Task DisApproveReturnedToMyBranch(int id)
        {
            var order = await _repository.GetById(id);
            order.InWayToBranch = false;
            await _repository.Update(order);
        }

        public async Task<PagingResualt<IEnumerable<TransferToSecondBranchDetialsReportDto>>> GetPrintTransferToSecondBranchDetials(PagingDto paging, int id)
        {
            var data = await _transferToOtherBranchDetialsRepository.GetAsync(paging, c => c.TransferToOtherBranchId == id);
            return new PagingResualt<IEnumerable<TransferToSecondBranchDetialsReportDto>>()
            {
                Total = data.Total,
                Data = _mapper.Map<IEnumerable<TransferToSecondBranchDetialsReportDto>>(data.Data)
            };
        }

        public async Task DisApproveOrderComeToMyBranch(int id)
        {
            var order = await _repository.GetById(id);
            order.IsReturnedByBranch = true;
            await _repository.Update(order);
        }
        public async Task<PagingResualt<IEnumerable<OrderDto>>> GetDisApprovedOrdersReturnedByBranch(PagingDto pagingDto)
        {
            var orders = await _repository.GetAsync(pagingDto, c => c.IsReturnedByBranch == true && c.CurrentBranchId == _currentBranchId, c => c.Country, c => c.Client);
            return new PagingResualt<IEnumerable<OrderDto>>()
            {
                Total = orders.Total,
                Data = _mapper.Map<IEnumerable<OrderDto>>(orders.Data)
            };
        }

        public async Task SetDisApproveOrdersReturnByBranchInStore(SelectedOrdersWithFitlerDto selectedOrdersWithFitlerDto)
        {
            var predicate = GetFilterAsLinq(selectedOrdersWithFitlerDto);
            predicate = predicate.And(c => c.IsReturnedByBranch == true && c.CurrentBranchId == _currentBranchId);
            var orders = await _repository.GetAsync(predicate);
            orders.ForEach(c =>
            {
                c.IsReturnedByBranch = false;
                c.InWayToBranch = false;
                c.OrderPlace = OrderPlace.Store;
            });
            await _repository.Update(orders);
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByAgentRegionAndCode(GetOrdersByAgentRegionAndCodeQuery getOrderByAgentRegionAndCode)
        {
            var orders = await _repository.GetAsync(c => c.Code == getOrderByAgentRegionAndCode.Code && c.CountryId == getOrderByAgentRegionAndCode.CountryId && c.AgentId == getOrderByAgentRegionAndCode.AgentId,
                c => c.Region, c => c.Country, c => c.Country, c => c.Client, c => c.Agent);
            if (!orders.Any())
            {
                throw new ConflictException("الشحنة غير موجودة");
            }
            //get last order to check last return erorr message for the last order 
            var lastOrder = orders.Last();
            ////execpt finished order 
            var finishedOrder = orders.Where(c => c.OrderPlace == OrderPlace.CompletelyReturned || c.OrderPlace == OrderPlace.Unacceptable || (c.OrderPlace == OrderPlace.Delivered && (c.MoneyPlace == MoneyPalce.InsideCompany || c.MoneyPlace == MoneyPalce.Delivered)));
            orders = orders.Except(finishedOrder);
            if (!orders.Any())
            {
                if (lastOrder.OrderPlace == OrderPlace.Store)
                {
                    throw new ConflictException("الشحنة ما زالت في المخزن");
                }
                if (lastOrder.MoneyPlace == MoneyPalce.InsideCompany)
                {
                    throw new ConflictException("الشحنة داخل الشركة");
                }
                throw new ConflictException("الشحنة غير موجودة");
            }
            return _mapper.Map<IEnumerable<OrderDto>>(orders);

        }

        public async Task<PagingResualt<IEnumerable<OrderDto>>> GetInStockToTransferWithAgent(PagingDto pagingDto, OrderFilter orderFilter)
        {
            var predicate = GetFilterAsLinq(orderFilter);
            predicate = predicate.And(c => c.OrderPlace == OrderPlace.Store && c.CurrentBranchId == _currentBranchId && c.MoneyPlace == MoneyPalce.OutSideCompany && c.OrderState == OrderState.Processing);
            var includes = new string[] { nameof(Order.Client), nameof(Order.Agent), nameof(Order.Region), nameof(Order.Country), $"{nameof(Order.OrderClientPaymnets)}.{nameof(OrderClientPaymnet.ClientPayment)}", $"{nameof(Order.AgentOrderPrints)}.{nameof(AgentOrderPrint.AgentPrint)}", nameof(Order.Branch), nameof(Order.CurrentBranch) };
            var orders = await _repository.GetAsync(pagingDto, predicate, includes);
            return new PagingResualt<IEnumerable<OrderDto>>()
            {
                Total = orders.Total,
                Data = _mapper.Map<IEnumerable<OrderDto>>(orders.Data)
            };
        }

    }


}
