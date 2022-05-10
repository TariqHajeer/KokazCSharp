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
        public NotificationService(IUintOfWork uintOfWork, IMapper mapper, NotificationHub notificationHub)
        {
            _uintOfWork = uintOfWork;
            _mapper = mapper;
            _notificationHub = notificationHub;
        }

        public async Task SendOrderReciveNotifcation(IEnumerable<Order> orders)
        {
            var moneyPlacedes = await _uintOfWork.Repository<MoenyPlaced>().GetAll();
            var outSideCompny = moneyPlacedes.First(c => c.Id == (int)MoneyPalcedEnum.OutSideCompany).Name;
            List<Notfication> totalNotfications = new List<Notfication>();
            List<Notfication> detailNotifications = new List<Notfication>();
            foreach (Order order in orders)
            {
                if (order.OrderStateId != (int)OrderStateEnum.Finished && order.OrderplacedId != (int)OrderplacedEnum.Way)
                {
                    var clientNotigaction = totalNotfications.Where(c => c.ClientId == order.ClientId && c.OrderPlacedId == order.OrderplacedId && c.MoneyPlacedId == order.MoenyPlacedId).FirstOrDefault();
                    if (clientNotigaction == null)
                    {
                        int moenyPlacedId = order.MoenyPlacedId;
                        if (moenyPlacedId == (int)MoneyPalcedEnum.WithAgent)
                            moenyPlacedId = (int)MoneyPalcedEnum.OutSideCompany;
                        clientNotigaction = new Notfication()
                        {
                            ClientId = order.ClientId,
                            OrderPlacedId = moenyPlacedId,
                            MoneyPlacedId = order.MoenyPlacedId,
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
                var moneyPlacedName = order.MoenyPlaced.Name;
                if (order.MoenyPlacedId == (int)MoneyPalcedEnum.WithAgent)
                    moneyPlacedName = outSideCompny;
                detailNotifications.Add(new Notfication()
                {
                    Note = $"الطلب {order.Code} اصبح {order.Orderplaced.Name} و موقع المبلغ  {order.MoenyPlaced.Name}",
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
    }
}
