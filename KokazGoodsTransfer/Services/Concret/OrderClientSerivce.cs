using AutoMapper;
using KokazGoodsTransfer.CustomException;
using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.Clients;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.NotifcationDtos;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.HubsConfig;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Static;
using KokazGoodsTransfer.Services.Interfaces;
using LinqKit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Concret
{
    public class OrderClientSerivce : IOrderClientSerivce
    {
        private readonly IRepository<Order> _repository;
        private readonly IRepository<OrderType> _orderTypeRepository;
        private readonly IRepository<MoenyPlaced> _moneyPlacedRepository;
        private readonly NotificationHub _notificationHub;
        private readonly IHttpContextAccessorService _contextAccessorService;
        private readonly IUintOfWork _UintOfWork;
        private readonly IMapper _mapper;
        public OrderClientSerivce(IRepository<Order> repository, NotificationHub notificationHub, IHttpContextAccessorService contextAccessorService, IMapper mapper, IRepository<OrderType> orderTypeRepository, IRepository<MoenyPlaced> moneyPlacedRepository, IUintOfWork uintOfWork)
        {
            _repository = repository;
            _notificationHub = notificationHub;
            _contextAccessorService = contextAccessorService;
            _mapper = mapper;
            _orderTypeRepository = orderTypeRepository;
            _moneyPlacedRepository = moneyPlacedRepository;
            _UintOfWork = uintOfWork;
        }

        public async Task<bool> CheckOrderTypesIdsExists(int[] ids)
        {
            ids = ids.Distinct().ToArray();
            var orderTypeIds = await _orderTypeRepository.Select(c => c.Id, c => ids.Contains(c.Id));
            if (orderTypeIds.Count() == ids.Length)
                return true;
            return false;
        }

        public Task<bool> CodeExist(string code)
        {
            return _repository.Any(c => c.Code == code && c.ClientId == _contextAccessorService.AuthoticateUserId());
        }

        public async Task CorrectOrderCountry(List<KeyValuePair<int, int>> pairs)
        {
            var ids = pairs.Select(c => c.Key).ToList();
            var cids = pairs.Select(c => c.Value).ToList();


            var ordersFromExcel = await _UintOfWork.Repository<OrderFromExcel>().GetAsync(c => ids.Contains(c.Id));
            var countries = await _UintOfWork.Repository<Country>().GetAsync(c => cids.Contains(c.Id));
            await _UintOfWork.BegeinTransaction();
            List<Order> orders = new List<Order>();
            foreach (var item in pairs)
            {
                var ofe = ordersFromExcel.FirstOrDefault(c => c.Id == item.Key);
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
                    ClientId = _contextAccessorService.AuthoticateUserId(),
                    CreatedBy = _contextAccessorService.AuthoticateUserName(),
                    DeliveryCost = country.DeliveryCost,
                    IsSend = false,
                };
                orders.Add(order);
            }
            await _UintOfWork.RemoveRange(ordersFromExcel);
            await _UintOfWork.AddRange(orders);
            await _UintOfWork.Commit();

            var newOrdersDontSendCount = await _UintOfWork.Repository<Order>().Count(c => c.IsSend == false && c.OrderplacedId == (int)OrderplacedEnum.Client);
            AdminNotification adminNotification = new AdminNotification()
            {
                NewOrdersDontSendCount = newOrdersDontSendCount
            };
            await _notificationHub.AdminNotifcation(adminNotification);
        }

        public async Task<List<string>> Validate(CreateOrderFromClient createOrderFromClient)
        {
            List<string> erros = new List<string>();
            if (await CodeExist(createOrderFromClient.Code))
            {
                erros.Add("الكود موجود مسبقاً");
            }
            if (createOrderFromClient.RecipientPhones.Length == 0)
            {
                erros.Add("رقم الهاتف مطلوب");
            }

            if (createOrderFromClient.OrderItem?.Any() == true)
            {
                if (createOrderFromClient.OrderItem.Any(c => c.OrderTypeId == null && string.IsNullOrEmpty(c.OrderTypeName.Trim())))
                {
                    erros.Add("يجب وضع اسم نوع الشحنة");
                }
                var orderTypesIds = createOrderFromClient.OrderItem.Where(c => c.OrderTypeId != null).Select(c => c.OrderTypeId.Value).ToArray();
                if (await CheckOrderTypesIdsExists(orderTypesIds))
                {
                    erros.Add("النوع غير موجود");
                }
            }
            return erros;
        }
        public async Task<OrderResponseClientDto> Create(CreateOrderFromClient createOrderFromClient)
        {
            var validation = await Validate(createOrderFromClient);
            if (validation.Count > 0)
            {
                throw new ConflictException(validation);
            }
            var order = _mapper.Map<Order>(createOrderFromClient);
            var country = await _UintOfWork.Repository<Country>().FirstOrDefualt(c => c.Id == order.CountryId);
            order.DeliveryCost = country.DeliveryCost;
            order.ClientId = _contextAccessorService.AuthoticateUserId();
            order.CreatedBy = _contextAccessorService.AuthoticateUserName();
            order.CurrentCountry = (await _UintOfWork.Repository<Country>().FirstOrDefualt(c => c.IsMain == true)).Id;
            await _UintOfWork.BegeinTransaction();
            await _UintOfWork.Add(order);
            var orderItems = createOrderFromClient.OrderItem;
            if (orderItems?.Any() == true)
            {
                var orderTypesNames = orderItems.Where(c => c.OrderTypeId == null).Select(c => c.OrderTypeName).Distinct();
                var orderTypes = await _UintOfWork.Repository<OrderType>().GetAsync(c => orderTypesNames.Any(ot => ot == c.Name));

                foreach (var item in orderItems)
                {
                    int orderTypeId;
                    if (item.OrderTypeId.HasValue)
                        orderTypeId = item.OrderTypeId.Value;
                    else
                    {
                        var simi = orderTypes.FirstOrDefault(c => c.Name == item.OrderTypeName);
                        if (simi != null)
                            orderTypeId = simi.Id;
                        else
                        {
                            ///TODO : make it faster 
                            var orderType = new OrderType()
                            {
                                Name = item.OrderTypeName
                            };
                            await _UintOfWork.Add(orderType);
                            orderTypeId = orderType.Id;
                        }
                    }
                    await _UintOfWork.Add(new OrderItem()
                    {
                        Count = item.Count,
                        OrderId = order.Id,
                        OrderTpyeId = orderTypeId
                    });

                }
            }
            await _UintOfWork.Commit();
            var newOrdersDontSendCount = await _UintOfWork.Repository<Order>().Count(c => c.IsSend == false && c.OrderplacedId == (int)OrderplacedEnum.Client);
            AdminNotification adminNotification = new AdminNotification()
            {
                NewOrdersDontSendCount = newOrdersDontSendCount
            };
            await _notificationHub.AdminNotifcation(adminNotification);
            return _mapper.Map<OrderResponseClientDto>(order);
        }

        public async Task Delete(int id)
        {
            var order = await _repository.GetById(id);
            if (order.IsSend == true)
                throw new ConflictException("");
            await _repository.Delete(order);

        }

        public async Task<PagingResualt<IEnumerable<OrderDto>>> Get(PagingDto pagingDto, COrderFilter orderFilter)
        {
            var predicate = PredicateBuilder.New<Order>(c => c.ClientId == _contextAccessorService.AuthoticateUserId());
            if (orderFilter.CountryId != null)
            {
                predicate = predicate.And(c => c.CountryId == orderFilter.CountryId);
            }
            if (orderFilter.Code != string.Empty && orderFilter.Code != null)
            {
                predicate = predicate.And(c => c.Code.StartsWith(orderFilter.Code));
            }

            if (orderFilter.RegionId != null)
            {
                predicate = predicate.And(c => c.RegionId == orderFilter.RegionId);
            }
            if (orderFilter.RecipientName != string.Empty && orderFilter.RecipientName != null)
            {
                predicate = predicate.And(c => c.RecipientName.StartsWith(orderFilter.RecipientName));
            }
            if (orderFilter.MonePlacedId != null)
            {
                predicate = predicate.And(c => c.MoenyPlacedId == orderFilter.MonePlacedId);
            }
            if (orderFilter.OrderplacedId != null)
            {
                predicate = predicate.And(c => c.OrderplacedId == orderFilter.OrderplacedId);
            }
            if (orderFilter.Phone != string.Empty && orderFilter.Phone != null)
            {
                predicate = predicate.And(c => c.RecipientPhones.Contains(orderFilter.Phone));
            }
            if (orderFilter.IsClientDiliverdMoney != null)
            {
                predicate = predicate.And(c => c.IsClientDiliverdMoney == orderFilter.IsClientDiliverdMoney);
            }
            if (orderFilter.ClientPrintNumber != null)
            {
                predicate = predicate.And(c => c.OrderClientPaymnets.Any(op => op.ClientPayment.Id == orderFilter.ClientPrintNumber));
            }
            var includes = new string[] { "Country", "Orderplaced", "MoenyPlaced", "OrderItems.OrderTpye", "OrderClientPaymnets.ClientPayment" };

            var pagingResult = await _repository.GetAsync(pagingDto, predicate, includes);
            return new PagingResualt<IEnumerable<OrderDto>>()
            {
                Total = pagingResult.Total,
                Data = _mapper.Map<IEnumerable<OrderDto>>(pagingResult.Data)
            };
        }

        public async Task<OrderDto> GetById(int id)
        {
            var inculdes = new string[] { "Agent", "Country", "Orderplaced", "MoenyPlaced", "OrderItems.OrderTpye", "OrderClientPaymnets.ClientPayment", "OrderClientPaymnets.ClientPaymentDetails" };
            var order = await _repository.FirstOrDefualt(c => c.Id == id, inculdes);
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<IEnumerable<OrderDto>> NonSendOrder()
        {
            var orders = await _repository.GetAsync(c => c.IsSend == false && c.ClientId == _contextAccessorService.AuthoticateUserId(), c => c.Country, c => c.MoenyPlaced, c => c.Orderplaced);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<IEnumerable<PayForClientDto>> OrdersDontFinished(OrderDontFinishFilter orderDontFinishFilter)
        {
            var outSideCompany = await _moneyPlacedRepository.GetById((int)MoneyPalcedEnum.OutSideCompany);
            var predicate = PredicateBuilder.New<Order>(false);
            if (orderDontFinishFilter.ClientDoNotDeleviredMoney)
            {
                var pr1 = PredicateBuilder.New<Order>(true);
                pr1.And(c => c.IsClientDiliverdMoney == false);
                pr1.And(c => orderDontFinishFilter.OrderPlacedId.Contains(c.OrderplacedId));
                pr1.And(c => c.ClientId == _contextAccessorService.AuthoticateUserId());
                predicate.Or(pr1);
            }
            if (orderDontFinishFilter.IsClientDeleviredMoney)
            {
                var pr2 = PredicateBuilder.New<Order>(true);
                pr2.And(c => c.OrderStateId == (int)OrderStateEnum.ShortageOfCash);
                pr2.And(c => orderDontFinishFilter.OrderPlacedId.Contains(c.OrderplacedId));
                pr2.And(c => c.ClientId == _contextAccessorService.AuthoticateUserId());
                predicate.Or(pr2);
            }
            var includes = new string[] { "Region", "Country", "Orderplaced", "MoenyPlaced", "Agent", "OrderClientPaymnets.ClientPayment", "AgentOrderPrints.AgentPrint" };
            var orders = await _repository.GetByFilterInclue(predicate, includes);
            orders.ForEach(o =>
            {
                if (o.MoenyPlacedId == (int)MoneyPalcedEnum.WithAgent)
                {
                    o.MoenyPlacedId = (int)MoneyPalcedEnum.OutSideCompany;
                    o.MoenyPlaced = outSideCompany;
                }
            });
            return _mapper.Map<IEnumerable<PayForClientDto>>(orders);
        }

        public async Task<IEnumerable<OrderFromExcel>> OrdersNeedToRevision()
        {
            return await _UintOfWork.Repository<OrderFromExcel>().GetAsync(c => c.ClientId == _contextAccessorService.AuthoticateUserId());
        }

        public async Task Send(int[] ids)
        {
            var orders = await _repository.GetAsync(c => ids.Contains(c.Id));
            foreach (var item in orders)
            {
                item.IsSend = true;
            }
            await _repository.Update(orders);
            var newOrdersCount = await _repository.Count(c => c.IsSend == true && c.OrderplacedId == (int)OrderplacedEnum.Client);
            var newOrdersDontSendCount = await _repository.Count(c => c.IsSend == false && c.OrderplacedId == (int)OrderplacedEnum.Client);
            var adminNotification = new AdminNotification()
            {
                NewOrdersCount = newOrdersCount,
                NewOrdersDontSendCount = newOrdersDontSendCount
            };
            await _notificationHub.AdminNotifcation(adminNotification);
        }
    }
}
