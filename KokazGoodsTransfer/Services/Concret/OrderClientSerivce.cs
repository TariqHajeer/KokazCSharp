using AutoMapper;
using KokazGoodsTransfer.CustomException;
using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
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
        private readonly NotificationHub _notificationHub;
        private readonly IHttpContextAccessorService _contextAccessorService;
        private readonly IMapper _mapper;
        public OrderClientSerivce(IRepository<Order> repository, NotificationHub notificationHub, IHttpContextAccessorService contextAccessorService, IMapper mapper, IRepository<OrderType> orderTypeRepository)
        {
            _repository = repository;
            _notificationHub = notificationHub;
            _contextAccessorService = contextAccessorService;
            _mapper = mapper;
            _orderTypeRepository = orderTypeRepository;
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

        public async Task Delete(int id)
        {
            var order = await _repository.GetById(id);
            if (order.IsSend == true)
                throw new ConflictException("");
            await _repository.Delete(order);

        }

        public async Task<PagingResualt<IEnumerable<OrderDto>>> Get(PagingDto pagingDto, COrderFilter orderFilter)
        {
            var predicate = PredicateBuilder.New<Order>(c=>c.ClientId==_contextAccessorService.AuthoticateUserId());
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

        public async Task<IEnumerable<OrderDto>> NonSendOrder()
        {
            var orders = await _repository.GetAsync(c => c.IsSend == false && c.ClientId == _contextAccessorService.AuthoticateUserId(), c => c.Country, c => c.MoenyPlaced, c => c.Orderplaced);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
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
