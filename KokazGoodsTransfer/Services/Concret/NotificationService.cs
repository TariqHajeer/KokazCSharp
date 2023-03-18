using AutoMapper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.NotifcationDtos;
using KokazGoodsTransfer.HubsConfig;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Static;
using KokazGoodsTransfer.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Concret
{
    public class NotificationService : INotificationService
    {
        private readonly IUintOfWork _uintOfWork;
        private readonly IMapper _mapper;
        private readonly NotificationHub _notificationHub;
        private readonly IOrderRepository _orderRepository;
        private readonly IRepository<EditRequest> _editRequestRepository;
        private readonly IRepository<PaymentRequest> _paymentRequestRepository;
        private readonly IRepository<Notfication> _repository;
        private readonly IHttpContextAccessorService _contextAccessorService;
        public NotificationService(IUintOfWork uintOfWork, IMapper mapper, NotificationHub notificationHub, IOrderRepository orderRepository, IRepository<EditRequest> editRequestRepository, IRepository<PaymentRequest> paymentRequestRepository, IRepository<Notfication> repository, IHttpContextAccessorService contextAccessorService)
        {
            _uintOfWork = uintOfWork;
            _mapper = mapper;
            _notificationHub = notificationHub;
            _orderRepository = orderRepository;
            _editRequestRepository = editRequestRepository;
            _paymentRequestRepository = paymentRequestRepository;
            _repository = repository;
            _contextAccessorService = contextAccessorService;
        }

        public async Task<AdminNotification> GetAdminNotification()
        {
            var newOrdersCount = await _orderRepository.Count(c => c.IsSend == true && c.OrderPlace == OrderPlace.Client);
            var newOrdersDontSendCount = await _orderRepository.Count(c => c.IsSend == false && c.OrderPlace == OrderPlace.Client);
            var orderRequestEditStateCount = await _orderRepository.Count(c => c.AgentRequestStatus == (int)AgentRequestStatusEnum.Pending);
            var newEditRquests = await _editRequestRepository.Count(c => c.Accept == null);
            var newPaymentRequetsCount = await _paymentRequestRepository.Count(c => c.Accept == null);
            var adminNotification = new AdminNotification()
            {
                NewOrdersCount = newOrdersCount,
                NewOrdersDontSendCount = newOrdersDontSendCount,
                OrderRequestEditStateCount = orderRequestEditStateCount,
                NewEditRquests = newEditRquests,
                NewPaymentRequetsCount = newPaymentRequetsCount
            };
            return adminNotification;
        }

        public async Task SendOrderReciveNotifcation(IEnumerable<Order> orders)
        {
            var outSideCompny = Consts.MoneyPlaceds.Single(c => c.Id == (int)MoneyPalced.OutSideCompany).Name;
            List<Notfication> totalNotfications = new List<Notfication>();
            List<Notfication> detailNotifications = new List<Notfication>();
            foreach (Order order in orders)
            {
                if (order.OrderStateId != (int)OrderStateEnum.Finished && order.OrderPlace != OrderPlace.Way)
                {
                    var clientNotigaction = totalNotfications.Where(c => c.ClientId == order.ClientId && (OrderPlace)c.OrderPlacedId == order.OrderPlace && c.MoneyPlacedId == (int)order.MoneyPlace).FirstOrDefault();
                    if (clientNotigaction == null)
                    {
                        int moenyPlacedId = (int)order.MoneyPlace;
                        if (moenyPlacedId == (int)MoneyPalced.WithAgent)
                            moenyPlacedId = (int)MoneyPalced.OutSideCompany;
                        clientNotigaction = new Notfication()
                        {
                            ClientId = order.ClientId,
                            OrderPlacedId = (int)order.OrderPlace,
                            MoneyPlacedId = moenyPlacedId,
                            IsSeen = false,
                            OrderCount = 1
                        };
                        totalNotfications.Add(clientNotigaction);
                    }
                    else
                    {
                        clientNotigaction.OrderCount++;
                    }
                }
                var moneyPlacedName = order.GetMoneyPlaced().Name;
                if (order.MoneyPlace == MoneyPalced.WithAgent)
                    moneyPlacedName = outSideCompny;
                detailNotifications.Add(new Notfication()
                {
                    Note = $"الطلب {order.Code} اصبح {order.GetOrderPlaced().Name} و موقع المبلغ  {order.GetMoneyPlaced().Name}",
                    ClientId = order.ClientId
                });
            }
            await _uintOfWork.AddRange(detailNotifications);
            await _uintOfWork.AddRange(totalNotfications);
            var unionNotification = totalNotfications.Union(detailNotifications);
            var grouping = unionNotification.GroupBy(c => c.ClientId);

            foreach (var item in grouping)
            {
                var key = item.Key.ToString();
                var notficationDtos = item.Select(c => _mapper.Map<NotificationDto>(c));
                await _notificationHub.AllNotification(key.ToString(), notficationDtos.ToArray());
            }

        }
        public async Task SendClientNotification()
        {
            var notification = await _repository.GetAsync(c => c.ClientId == _contextAccessorService.AuthoticateUserId() && c.IsSeen == false, c => c.MoneyPlaced, c => c.OrderPlaced);
            var dto = _mapper.Map<NotificationDto[]>(notification);
            await _notificationHub.AllNotification(_contextAccessorService.AuthoticateUserId().ToString(), dto);
        }

        public async Task SeeNotifactions(int[] ids)
        {
            var notifications = await _repository.GetAsync(c => ids.Contains(c.Id));
            foreach (var item in notifications)
            {
                item.IsSeen = true;
            }
            await _repository.Update(notifications);
        }
        public async Task<IEnumerable<NotificationDto>> GetClientNotifcations()
        {
            var notifications = await _repository.GetAsync(c => c.IsSeen != true && c.ClientId == _contextAccessorService.AuthoticateUserId(), c => c.MoneyPlaced, c => c.OrderPlaced);
            var dtos = _mapper.Map<IEnumerable<NotificationDto>>(notifications.OrderByDescending(c => c.Id));
            return dtos.OrderBy(c => c.Note).ThenBy(c => c.Id);
        }

        public async Task<int> NewNotfiactionCount()
        {
            return await _repository.Count(c => c.IsSeen != true && c.ClientId == _contextAccessorService.AuthoticateUserId());
        }
    }
}
