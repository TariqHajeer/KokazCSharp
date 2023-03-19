using System.Threading.Tasks;
using System.Collections.Generic;
using Quqaz.Web.Models;
using Quqaz.Web.Dtos.NotifcationDtos;

namespace Quqaz.Web.Services.Interfaces
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
