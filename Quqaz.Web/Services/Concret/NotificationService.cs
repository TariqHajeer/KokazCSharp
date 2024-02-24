using AutoMapper;
using FirebaseAdmin.Messaging;
using Quqaz.Web.DAL.Helper;
using Quqaz.Web.DAL.Infrastructure.Interfaces;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.NotifcationDtos;
using Quqaz.Web.HubsConfig;
using Quqaz.Web.Models;
using Quqaz.Web.Models.Static;
using Quqaz.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Services.Concret
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
        public Task SendNotification(List<Models.Notfication> notifications, bool withNotification = true)
        {
            if (notifications == null || !notifications.Any())
            {
                return Task.CompletedTask;
            }

            var tasks = new List<Task>();
            foreach (var notification in notifications)
            {
                var clientTokens = notification.Client.FCMTokens?.Select(t => t.Token).ToList();
                if (clientTokens == null || !clientTokens.Any())
                {
                    continue;
                }

                tasks.Add(SendNotification(clientTokens, notification, withNotification));
            }

            return Task.WhenAll(tasks);
        }
        public Task SendNotification(List<string> clientToken, Models.Notfication notification, bool withNotification = true)
        {
            var registrationTokens = clientToken;
            if (registrationTokens == null || registrationTokens.Count == 0)
            {
                return Task.CompletedTask;
            }
            var message = new MulticastMessage()
            {
                Tokens = registrationTokens,
                Data = new Dictionary<string, string>()
                {
                    { "id", notification.Id.ToString() }
                }

            };
            if (withNotification)
            {
                message.Notification = new Notification()
                {
                    Body = notification.Title,
                    Title = notification.Body,
                };
            }
            return FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
        }
        public async Task SendNotification(List<string> clientToken, int id, bool withNotification = true)
        {
            var registrationTokens = clientToken;
            if (registrationTokens == null || registrationTokens.Count == 0)
            {
                return;
            }
            var notification = await _repository.GetById(id);

            var message = new MulticastMessage()
            {
                Tokens = registrationTokens,
                Data = new Dictionary<string, string>()
                {
                    { "id", id.ToString() }
                }

            };
            if (withNotification)
            {
                message.Notification = new Notification()
                {
                    Body = notification.Title,
                    Title = notification.Body,
                };
            }
            await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message).ConfigureAwait(true);
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
            var outSideCompny = Consts.MoneyPlaceds.Single(c => c.Id == (int)MoneyPalce.OutSideCompany).Name;
            List<Notfication> totalNotfications = new List<Notfication>();
            List<Notfication> detailNotifications = new List<Notfication>();
            foreach (Order order in orders)
            {
                if (order.OrderState != OrderState.Finished && order.OrderPlace != OrderPlace.Way)
                {
                    var clientNotigaction = totalNotfications.Where(c => c.ClientId == order.ClientId && (OrderPlace)c.OrderPlace == order.OrderPlace && c.MoneyPlace == order.MoneyPlace).FirstOrDefault();
                    if (clientNotigaction == null)
                    {
                        var moenyPlacedId = order.MoneyPlace;
                        if (moenyPlacedId == MoneyPalce.WithAgent)
                            moenyPlacedId = MoneyPalce.OutSideCompany;
                        clientNotigaction = new Notfication()
                        {
                            ClientId = order.ClientId,
                            OrderPlace = order.OrderPlace,
                            MoneyPlace = moenyPlacedId,
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
                if (order.MoneyPlace == MoneyPalce.WithAgent)
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
            var notification = await _repository.GetAsync(c => c.ClientId == _contextAccessorService.AuthoticateUserId() && c.IsSeen == false);
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
            var notifications = await _repository.GetAsync(c => c.IsSeen != true && c.ClientId == _contextAccessorService.AuthoticateUserId());
            var dtos = _mapper.Map<IEnumerable<NotificationDto>>(notifications.OrderByDescending(c => c.Id));
            return dtos.OrderBy(c => c.Note).ThenBy(c => c.Id);
        }

        public async Task<int> NewNotfiactionCount()
        {
            return await _repository.Count(c => c.IsSeen != true && c.ClientId == _contextAccessorService.AuthoticateUserId());
        }

        public async Task<PagingResualt<List<NewNotificationDto>>> GetNotifications(PagingDto paging)
        {
            var notification = await _repository.GetAsync(paging: paging, propertySelectors: null, orderBy: c => c.OrderBy(c => c.CreatedDate));
            return new PagingResualt<List<NewNotificationDto>>()
            {
                Total = notification.Total,
                Data = _mapper.Map<List<NewNotificationDto>>(notification.Data)
            };
        }

        public async Task CreateRange(List<CreateNotificationDto> createNotificationDtos)
        {
            var currentDate = DateTime.UtcNow;
            var notification = createNotificationDtos.Select(c => new Models.Notfication()
            {
                ClientId = c.ClientId,
                Body = c.Body,
                Title = c.Title,
                CreatedDate = currentDate
            });
            await _repository.AddRangeAsync(notification);
            notification = await _repository.GetAsync(c => notification.Select(c => c.Id).Contains(c.Id), c => c.Client.FCMTokens);
            await SendNotification(notification.ToList(), true);
        }
    }
}
