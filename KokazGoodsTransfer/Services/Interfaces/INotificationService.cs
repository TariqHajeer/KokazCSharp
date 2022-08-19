using System.Threading.Tasks;
using System.Collections.Generic;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Dtos.NotifcationDtos;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface INotificationService
    {
        Task SendOrderReciveNotifcation(IEnumerable<Order> orders);
        Task<AdminNotification> GetAdminNotification();
        Task SendClientNotification();
        Task SeeNotifactions(int[] ids);
        Task<IEnumerable<NotificationDto>> GetClientNotifcations();
        Task<int> NewNotfiactionCount();
    }
}
